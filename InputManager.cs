using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool alt_input;
    public bool jump_input;
    public bool t_input;

public void Awake()
{
    animatorManager = GetComponent<AnimatorManager>();
    playerLocomotion = GetComponent<PlayerLocomotion>();
}
    private void OnEnable() 
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.ALT.performed += i => alt_input = true;
            playerControls.PlayerActions.ALT.canceled += i => alt_input = false;
            playerControls.PlayerActions.T.performed += i => t_input = true;

            playerControls.PlayerActions.Jump.performed += i => jump_input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable(){

        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleDodgeInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if(alt_input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if(jump_input)
        {
            jump_input = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleDodgeInput()
    {
        if(t_input)
        {
            t_input = false;
            playerLocomotion.HandleDodge();
        }
    }
}
