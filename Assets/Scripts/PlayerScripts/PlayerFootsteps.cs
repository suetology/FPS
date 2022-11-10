//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private AudioSource footstepSound;

    [SerializeField] private AudioClip[] footstepClip;

    private CharacterController characterController;

    private float walkVolume = 0.5f;
    private float crouchVolume = 0.3f;
    private float sprintVolume = 1f;

    private float accumulatedDistance;

    private float walkStepDistance = 0.4f;
    private float sprintStepDistance = 0.25f;
    private float crouchStepDistance = 0.6f;

    void Awake()
    {
        footstepSound = GetComponent<AudioSource>();
        characterController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        PlayFootstepSound();
    }

    private void PlayFootstepSound()
    {
        if(!characterController.isGrounded)
        {
            return;
        }

        if(characterController.velocity.sqrMagnitude > 0)
        {
            accumulatedDistance += Time.deltaTime;

            switch(playerMovement.playerState)
            {
                case PlayerMovement.PlayerState.walk:
                    if (accumulatedDistance > walkStepDistance && characterController.isGrounded)
                    {
                        footstepSound.volume = walkVolume;
                        footstepSound.clip = footstepClip[Random.Range(0, footstepClip.Length)];
                        footstepSound.Play();

                        accumulatedDistance = 0;
                    }
                    break;

                case PlayerMovement.PlayerState.sprint:
                    if (accumulatedDistance > sprintStepDistance && characterController.isGrounded)
                    {
                        footstepSound.volume = sprintVolume;
                        footstepSound.clip = footstepClip[Random.Range(0, footstepClip.Length)];
                        footstepSound.Play();

                        accumulatedDistance = 0;
                    }
                    break;

                case PlayerMovement.PlayerState.crouch:
                    if (accumulatedDistance > crouchStepDistance && characterController.isGrounded)
                    {
                        footstepSound.volume = crouchVolume;
                        footstepSound.clip = footstepClip[Random.Range(0, footstepClip.Length)];
                        footstepSound.Play();

                        accumulatedDistance = 0;
                    }
                    break;
            }

        }
        else
        {
            accumulatedDistance = 0;
        }
    }
}
