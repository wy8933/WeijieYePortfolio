using UnityEngine;

[CreateAssetMenu(fileName = "Room Item", menuName = "ScriptableObjects/Room Item")]
public class SO_RoomItem : ScriptableObject
{
    public string itemName;
    public int itemID;
    public Sprite itemIcon;
    public string description;
    public int cost;
    public BaseRoom room;
}
