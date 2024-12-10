using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Referenced from Trever Mock
public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(GameData data);
}
