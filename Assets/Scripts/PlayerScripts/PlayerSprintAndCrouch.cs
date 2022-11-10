using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float crouchSpeed = 2f;

    [SerializeField] private Transform lookRoot;

    private float currentHeight;
    [SerializeField] private float standHeight = 0f;
    [SerializeField] private float crouchHeight = -.5f;

    [SerializeField] private float standAndSitSpeed;

    private bool isCrouching;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lookRoot = transform.GetChild(0);
    }
    public void Update()
    {
        Sprint();
        Crouch();
    }
    private void Sprint()
    {
        if (playerMovement.characterController.isGrounded)
        {
            if (Input.GetAxis(Axis.SPRINT) != 0)
            {
                if(isCrouching)
                {
                    isCrouching = false;
                }

                playerMovement.moveSpeed = sprintSpeed;

                playerMovement.playerState = PlayerMovement.PlayerState.sprint;
            }
            else
            {
                playerMovement.ResetPlayerSpeed();
            }
        }
    }
    private void Crouch()
    {
        if (!isCrouching)
        {
            if(lookRoot.localPosition.y != standHeight)
            {
                currentHeight = Mathf.Lerp(this.currentHeight, standHeight, standAndSitSpeed * Time.deltaTime);
                lookRoot.localPosition = new Vector3(0, currentHeight, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && playerMovement.characterController.isGrounded)
            {
                playerMovement.moveSpeed = crouchSpeed;
                currentHeight = lookRoot.localPosition.y;

                isCrouching = true;
            }
        }
        else
        {
            if(playerMovement.characterController.velocity.sqrMagnitude > 0)
            {
                playerMovement.playerState = PlayerMovement.PlayerState.crouch;
            }

            if (lookRoot.localPosition.y != crouchHeight)
            {
                currentHeight = Mathf.Lerp(this.currentHeight, crouchHeight, standAndSitSpeed * Time.deltaTime);
                lookRoot.localPosition = new Vector3(0, currentHeight, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) || !playerMovement.characterController.isGrounded)
            {
                playerMovement.ResetPlayerSpeed();
                currentHeight = lookRoot.localPosition.y;

                isCrouching = false;
            }
        }
    }
   
}
