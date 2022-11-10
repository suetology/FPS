using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        idle,
        walk,
        sprint,
        crouch
    }

    public CharacterController characterController { get; private set; }

    private Vector3 moveDirection;

    [SerializeField] private float basicMoveSpeed = 3f;
    [HideInInspector] public float moveSpeed;

    private float gravity = 20f;
    [SerializeField] private float jumpForce = 10f;

    private float verticalVelocity;

    public PlayerState playerState;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if(characterController.velocity.sqrMagnitude == 0 || !characterController.isGrounded)
        {
            playerState = PlayerState.idle;
        }
        MovePlayer();
        PlayerJump();
    }

    private void MovePlayer()
    {
        moveDirection = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= moveSpeed * Time.deltaTime;

        ApplyGravity();

        characterController.Move(moveDirection);
    }

    private void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;

        moveDirection.y = verticalVelocity * Time.deltaTime;
    }

    private void PlayerJump()
    {
        if (characterController.isGrounded && Input.GetAxis(Axis.JUMP) != 0)
        {
            verticalVelocity = jumpForce;
        }
    }
    public void ResetPlayerSpeed()
    {
        moveSpeed = basicMoveSpeed;

        playerState = PlayerState.walk;
    }






}
