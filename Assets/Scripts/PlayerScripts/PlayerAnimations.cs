using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator playerAnimator;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();

        playerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        switch(playerMovement.playerState)
        {
            case PlayerMovement.PlayerState.idle:
                playerAnimator.SetBool("Run", false);
                //playerAnimator.SetBool("Aim", false);
                playerAnimator.SetBool("Walk", false);
                break;

            case PlayerMovement.PlayerState.walk:
                playerAnimator.SetBool("Run", false);
                //playerAnimator.SetBool("Aim", false);
                playerAnimator.SetBool("Walk", true);
                break;

            case PlayerMovement.PlayerState.sprint:

                playerAnimator.SetBool("Run", true);
                //playerAnimator.SetBool("Aim", false);
                playerAnimator.SetBool("Walk", false);

   
                break;

            case PlayerMovement.PlayerState.crouch:
                playerAnimator.SetBool("Run", false);
                //playerAnimator.SetBool("Aim", false);
                playerAnimator.SetBool("Walk", true);
                break;
        }
    }


}
