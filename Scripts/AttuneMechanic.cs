using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class represents the Attune Mechanic for the player

public class AttuneMechanic : MonoBehaviour
{
    public bool isAttuned = false;
    public GameObject attuneText;

    void Update()
    {
        if (Time.timeScale == 1 && Input.GetKeyDown(KeyCode.Q))
        {
            if (IsPlayerAttuned())
            {
                DeAttune();
            }
            else
            {
                Attune();
            }
        }
    }

    public void Attune()
    {
        gameObject.tag = "Nightmare Player"; //Allows player to be able to attack Nightmare Mobs
        isAttuned = true;
        attuneText.SetActive(true);
    }

    public void DeAttune()
    {
        gameObject.tag = "Player";
        isAttuned = false;
        attuneText.SetActive(false);
    }

    public bool IsPlayerAttuned()
    {
        return isAttuned;
    }
}

