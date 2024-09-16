using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public Transform HoldPosition;
    public Transform GrabReticle;
    public BoxWithRooms BoxWithRoomsPrefab;

    public BoxWithRooms CarriedBox {  get; set; }
    public Transform PodTransform => transform;

    private void Update() {
        if (CarriedBox != null) {
            // Make the box follow the drone
            CarriedBox.transform.position = HoldPosition.position;
        }
    }

    public void PlaceBox() {
        if (CarriedBox != null) {
            // Generate rooms at the box's position
            if (CarriedBox.RoomType != null) {
                if (GameManager.Instance.GenerateRoomClusterOfTypeAtPosition(CarriedBox.transform.position, CarriedBox.NumberOfRooms, CarriedBox.RoomType)) {
                    SoundManager.Instance.PlaySound(SoundName.PlaceRoom);

                    // Destroy the box after placing it
                    Destroy(CarriedBox.gameObject);

                    // Clear the carried box reference
                    CarriedBox = null;
                }
            } else if (GameManager.Instance.GenerateRoomClusterAtPosition(CarriedBox.transform.position, CarriedBox.NumberOfRooms) == true) {
                // Play sounds
                SoundManager.Instance.PlaySound(SoundName.PlaceRoom);

                // Destroy the box after placing it
                Destroy(CarriedBox.gameObject);

                // Clear the carried box reference
                CarriedBox = null;  
            }
        }
    }

    public void PickUpTile() {
        if (CarriedBox == null) {
            BaseRoom pickup = GameManager.Instance.PickUpTile(GrabReticle.position);
            if (pickup != null) {
                // Play sounds
                SoundManager.Instance.PlaySound(SoundName.PickUpRoom);

                PutBoxInPodHands(BoxWithRoomsPrefab, pickup);
            }
        }
    }

    public void PutBoxInPodHands(BoxWithRooms box, BaseRoom roomType) {
        CarriedBox = Instantiate(box, HoldPosition.position, Quaternion.identity);
        CarriedBox.RoomType = roomType;
        CarriedBox.UpdateNumberOfRooms(1);
    }

    public bool IsPodOutsideCircle(Vector3 circleCenter, float radius) {
        float distance = Vector3.Distance(transform.position, circleCenter);
        return distance > radius;
    }

    public void MovePodOutsideShipBounds(Vector3 circleCenter, float radius) {
        float distance = Vector3.Distance(transform.position, circleCenter);

        if (distance <= radius) {
            Vector3 direction = (transform.position - circleCenter).normalized;
            
            // Move pod outside by 1 unit more than radius
            transform.position = circleCenter + direction * (radius + 1f); 
        }
    }
}
