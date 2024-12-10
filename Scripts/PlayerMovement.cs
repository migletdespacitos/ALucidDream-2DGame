using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//this class represents the movement mechanics for the player (Referenced to InfoGamer Among Us Unity Video on Player Movement)

public class PlayerMovement : MonoBehaviour
{
    // Components
    Rigidbody RB;
    Transform Avatar;
    Animator Anim;

    // Input Manager
    public InputAction WASD;

    // Movement
    [SerializeField] float movementSpeed;

    private void OnEnable()
    {
        // Enable input action
        WASD.Enable();
    }

    private void OnDisable()
    {
        // Disable input action
        WASD.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        Avatar = transform.GetChild(0);
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if input is enabled and not paused
        if (!PauseManager.isPaused)
        {
            Vector2 movementInput = WASD.ReadValue<Vector2>();

            if (movementInput.x != 0)
            {
                Avatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
            }

            Anim.SetFloat("Speed", movementInput.magnitude);
        }
    }

    private void FixedUpdate()
    {
        // Check if input is enabled and not paused
        if (!PauseManager.isPaused)
        {
            Vector2 movementInput = WASD.ReadValue<Vector2>();
            RB.velocity = movementInput * movementSpeed;
        }
    }
}