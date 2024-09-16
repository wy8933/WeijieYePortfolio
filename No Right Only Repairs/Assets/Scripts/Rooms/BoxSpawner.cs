using UnityEngine;

public class BoxSpawner : MonoBehaviour {
    public BoxWithRooms BoxPrefab;
    public Transform PlayerTransform;
    public float SpawnDistance = 30.0f;
    public float SpawnHeight = 10.0f;
    public float BoxSpeed = 5.0f;
    public float SpawnInterval = 2.0f;
    public float GrowthRate = 0.5f;

    private float _spawnTimer;

    void Update() {
        if (Time.time >= _spawnTimer) {
            SpawnBox();
            _spawnTimer = Time.time + SpawnInterval;
        }
    }

    void SpawnBox() {
        // Calculate the spawn position to the right of the player
        Vector3 spawnPosition = new Vector3(PlayerTransform.position.x + SpawnDistance,
                                            PlayerTransform.position.y + Random.Range(-SpawnHeight, SpawnHeight),
                                            0f);

        // Instantiate the box at the spawn position
        BoxWithRooms box = Instantiate(BoxPrefab, spawnPosition, Quaternion.identity);
        box.UpdateNumberOfRooms(Mathf.CeilToInt(GameManager.Instance.InitialRoomsPrefabs.Count * Mathf.Exp(GrowthRate * GameManager.Instance.SessionTimer)));

        // Calculate the direction from the box to the player
        Vector3 directionToPlayer = (PlayerTransform.position - spawnPosition).normalized;

        // Set the box's velocity to move it towards the player
        Rigidbody2D rb = box.GetComponent<Rigidbody2D>();
        rb.velocity = directionToPlayer * BoxSpeed;
    }
}