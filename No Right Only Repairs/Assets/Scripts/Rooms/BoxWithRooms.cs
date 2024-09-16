using TMPro;
using UnityEngine;

public class BoxWithRooms : MonoBehaviour
{
    public int NumberOfRooms = 5;  // Number of rooms the box will generate
    public TMP_Text NumberOfRoomsText;  // Reference to the Text UI element
    public BaseRoom RoomType;

    private void Start() {
        // Display the number of rooms on the box
        if (NumberOfRoomsText != null) {
            NumberOfRoomsText.text = NumberOfRooms.ToString();
        }
    }

    public void UpdateNumberOfRooms(int numberOfRooms) {
        NumberOfRooms = numberOfRooms;
        NumberOfRoomsText.text = numberOfRooms.ToString();
    }
}
