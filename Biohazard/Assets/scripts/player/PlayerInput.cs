using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerMovement movement;

    string lastState = "idle";

    private float lastAcidUse;
    bool canUseAcid = true;

    public float dissabledtimer;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Time.time > lastAcidUse + 8)
        {
            canUseAcid = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GetComponent<PlayerStateManager>().Chomp();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && canUseAcid)
        {
            PlayerStateManager.currentState = "vomitting";
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            PlayerStateManager.currentState = "idle";

            if (lastState == "vomitting")
            {
                GetComponent<PlayerStateManager>().AcidAttack();
                lastAcidUse = Time.time;
                canUseAcid = false;
            }
        }

        PlayerStateManager playerStatManager = GetComponent<PlayerStateManager>();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerStatManager.IsLungeCharging = true;

            dissabledtimer += Time.deltaTime * 5;
            if (dissabledtimer >= 20)
            {
                dissabledtimer = 20;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PlayerMovement.moveSpeed = 5;
            StartCoroutine("PlayerLungeAttack", dissabledtimer/10);
            playerStatManager.IsLungeCharging = false;
            playerStatManager.LungeAttack();

        }

        lastState = PlayerStateManager.currentState;
    }

    IEnumerator PlayerLungeAttack(float lungeTime)
    {
        movement.enabled = false;
        yield return new WaitForSeconds(lungeTime);
        movement.enabled = true;
        dissabledtimer = 0;
    }
}
