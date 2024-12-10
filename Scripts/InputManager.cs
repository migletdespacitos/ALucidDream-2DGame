using UnityEngine;
using UnityEngine.InputSystem;

//this class manages the inputs from the player

public class InputManager : MonoBehaviour
{
    public InputAction WASD;
    public bool inputEnabled = true;

    void OnEnable()
    {
        WASD.Enable();
    }

    void OnDisable()
    {
        WASD.Disable();
    }

    void Update()
    {
        if (PauseManager.isPaused)
        {
            inputEnabled = false;
        }
        else
        {
            inputEnabled = true;
        }
    }
}