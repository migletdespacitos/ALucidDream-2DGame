using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Referenced from Trever Mock

[System.Serializable]
public class GameData
{
    public int deathCount;

    public int shardsCollected;

    public int expEarned;

    public int currentScene;
    
    // public Vector3 playerPosition;

    // Values defined in this constructor will be the default values
    // the game starts with when there is no data to load
    public GameData()
    {
        this.shardsCollected = 0;
        this.deathCount = 0;
        this.expEarned = 0;
        this.currentScene = 2;
        // this.playerPosition = Vector3.zero;
    }
}
