using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitButton()
    {
        if (FirebaseManager.instance.user != null) 
        { 
           Debug.Log("Still Signed In"); 
        }

        DataPersistenceManager.instance.SaveGame();

        GameManager.instance.ChangeScene(1);
    }
}
