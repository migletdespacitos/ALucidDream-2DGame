using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Referenced from Trever Mock
public class SaveSlot : MonoBehaviour
{
    [Header("SaveSlot")]
    [SerializeField]
    private string saveId = "";
    [Space(5f)]

    [Header("Content")]
    [SerializeField]
    private GameObject noDataContent;
    [SerializeField]
    private GameObject hasDataContent;
    [SerializeField]
    private TMP_Text shardsCollectedText;
    [SerializeField]
    private TMP_Text expEarnedText;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        // there is no data for this saveId
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        // there is data for this saveId
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            shardsCollectedText.text = "SHARDS COLLECTED: " + data.shardsCollected;
            expEarnedText.text = "MOBS DEFEATED: " + data.expEarned;
        }
    }

    public string GetSaveId()
    {
        return this.saveId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
