using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomStore : MonoBehaviourSingleton<RoomStore>
{
    public List<SO_RoomItem> roomItems; 
    public Button[] roomButtons; 
    public InteractionHandler PlayerInteraction;
    public BoxWithRooms Box;

    public void AssignRandomRooms()
    {
        ResetButtons();
        for (int i = 0; i < roomButtons.Length; i++)
        {
            SO_RoomItem randomRoomItem = roomItems[Random.Range(0, roomItems.Count)];

            roomButtons[i].gameObject.SetActive(true);
            SetButtonData(roomButtons[i], randomRoomItem);
        }
    }

    private void SetButtonData(Button button, SO_RoomItem roomItem)
    {
        button.GetComponentInChildren<TMP_Text>().text = "Cost: " + roomItem.cost * Settings.Instance.TradingMultiplier; 
        button.GetComponent<Image>().sprite = roomItem.itemIcon;
        button.GetComponent<TooltipInfo>().description = roomItem.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => PurchaseRoom(button,roomItem));
    }

    public void PurchaseRoom(Button button, SO_RoomItem roomItem)
    {
        if (ResourceManager.Instance.rareMineral >= roomItem.cost * Settings.Instance.TradingMultiplier)
        {
            ResourceManager.Instance.UpdateRareMineral(-roomItem.cost * Settings.Instance.TradingMultiplier);
            button.interactable = false;
            Debug.Log(roomItem.itemName + " purchased! Remaining money: " + ResourceManager.Instance.rareMineral);

            AddRoomToPlayer(roomItem);
        }
        else
        {
            Debug.Log("Not enough money to purchase " + roomItem.itemName);
        }
    }

    public void AddRoomToPlayer(SO_RoomItem roomItem)
    {
        Debug.Log(roomItem.itemName + " added to the player's ship.");
        PlayerInteraction.PutBoxInPodHands(Box, roomItem.room);
    }

    public void ResetButtons() {
        foreach (Button button in roomButtons) {
            button.interactable = true;
        }
    }
}
