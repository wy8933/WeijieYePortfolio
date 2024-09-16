using UnityEngine;

[CreateAssetMenu(fileName = "Power Up", menuName = "ScriptableObjects/Power Up")]
public class SO_PowerUp : ScriptableObject
{
    public string itemName;
    public int itemID;
    public Sprite itemIcon;
    public string description;
    public PowerUpType powerUpType;
    public int cost;
}
