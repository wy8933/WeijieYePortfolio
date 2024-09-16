using UnityEngine;

public enum ResourceDrop { 
    Asteroid,
    AilienShip
}

public class ResourceProvider : MonoBehaviour
{
    public ResourceDrop resourceDrop;
    public float resourceMultiplier;
    private void OnDestroy()
    {
        //provide material
        if (resourceDrop == ResourceDrop.Asteroid) {
            ResourceManager.Instance.UpdateRareMineral(5 * resourceMultiplier);
        }

        if (resourceDrop == ResourceDrop.AilienShip) {
            ResourceManager.Instance.UpdateRefugee(Settings.Instance.RefugeeDropModifier);
            float dropChance = Random.Range(0f, 1f);

            if (dropChance <= 0.2f * Settings.Instance.RoomDropMultiplier)
            {
                int random = Random.Range(0, RoomStore.Instance.roomItems.Count);

                RoomStore.Instance.PlayerInteraction.CarriedBox = Instantiate(RoomStore.Instance.Box, RoomStore.Instance.PlayerInteraction.HoldPosition.position, RoomStore.Instance.PlayerInteraction.HoldPosition.rotation);
                RoomStore.Instance.PlayerInteraction.CarriedBox.RoomType = RoomStore.Instance.roomItems[random].room;
                RoomStore.Instance.PlayerInteraction.CarriedBox.UpdateNumberOfRooms(1);
            }
        }
    }
}
