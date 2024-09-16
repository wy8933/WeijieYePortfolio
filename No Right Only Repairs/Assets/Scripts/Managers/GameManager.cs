using Cinemachine;
using Scripts.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager> {
    [Header("Game Settings")]
    public List<BaseRoom> InitialRoomsPrefabs;
    public List<BaseRoom> RoomPrefabs;
    public Transform ShipTransform;
    public int MaxPopulationCount = 10;
    public int CrewQuartersMod = 5;
    public int GeneratorChargeRange = 3;

    private float _sessionTimer = 0.0f;
    public float SessionTimer => _sessionTimer;

    public bool inBattle = true;

    [Header("Managers")]
    public InteractionHandler PlayerPodInteraction;
    public GameObject Ship;
    private ShipGrid _shipGrid;

    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Enemy Settings")]
    public GameObject AlienShipPrefab;
    public GameObject AsteroidPrefab;
    public float AttackInterval = 30f; 
    public float SpawnDistance = 50f;

    public int currentElectionCycle = 0;
    private int _currentPopulationCount = 0;

    private void Start() {
        _shipGrid = Ship.GetComponent<ShipGrid>();
        StartCoroutine(InitializeGame());
        StartCoroutine(SpawnRoutine());
    }

    private void Update() {
        _sessionTimer += Time.deltaTime;

        if (UIManager.Instance.gameProgress.value == 1) {
            Settings.Instance.GoodwillOverall = GoodWillManager.Instance.Goodwill;
            SceneManager.LoadScene("GoodEnd");
        }
    }

    private IEnumerator InitializeGame() {
        yield return new WaitForSeconds(0.1f);
        ElectionManager.Instance.TriggerElection();
        Time.timeScale = 0;
        _sessionTimer = 0;
        SetupInitialRooms();
        // Initialize UI.
    }



    public void ToggleGamePause() {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            UIManager.Instance.pausePanel.SetActive(true);
        }
        else {
            Time.timeScale = 1;
            UIManager.Instance.pausePanel.SetActive(false);
            UIManager.Instance.settingPanel.SetActive(false);
        }
    }
    public void StopGameTimeScale()
    {
        Time.timeScale = 0;
    }
    public void StartGameTimeScale()
    {
        Time.timeScale = 1;
    }

    private void SetupInitialRooms() {
        int section = -1;

        for (int i = 0; i < InitialRoomsPrefabs.Count; i++) {
            BaseRoom newRoom = Instantiate(InitialRoomsPrefabs[i], ShipTransform);
            AddRoomAtPosition(new Vector3Int(section++, 0), newRoom);
        }
    }

    public void AddRoomAtPosition(Vector3Int cellPosition, BaseRoom newRoom) {
        if (_shipGrid.AddRoom(cellPosition, newRoom)) {
            // Update UI with room count.
        } else {
            Destroy(newRoom);
        }
    }

    public bool GenerateRoomClusterAtPosition(Vector3 worldPosition, int numberOfRooms) {
        Vector3Int cellPosition = _shipGrid.Grid.WorldToCell(worldPosition);

        if (_shipGrid.IsCloseEnough(cellPosition)) {
            List<BaseRoom> roomsToAdd = new List<BaseRoom>();

            for (int i = 0; i < numberOfRooms; i++) {
                roomsToAdd.Add(Instantiate(RoomPrefabs[Random.Range(0,RoomPrefabs.Count - 1)], ShipTransform));
            }

            _shipGrid.SpawnRoomsAroundCellPosition(cellPosition, roomsToAdd);

            return true;
        }

        return false;
    }

    public bool GenerateRoomClusterOfTypeAtPosition(Vector3 worldPosition, int numberOfRooms, BaseRoom roomToAdd) {
        Debug.Log(roomToAdd.gameObject.GetComponent<Health>());
        Vector3Int cellPosition = _shipGrid.Grid.WorldToCell(worldPosition);

        if (_shipGrid.IsCloseEnough(cellPosition)) {
            for (int i = 0; i < numberOfRooms; i++) {
                _shipGrid.SpawnRoomsAroundCellPosition(cellPosition, new List<BaseRoom>() { roomToAdd });
            }

            return true;
        }

        return false;
    }

    public BaseRoom PickUpTile(Vector3 worldPosition) {
        return _shipGrid.PickUpTile(worldPosition);
    }

    public void HandleRoomDestroyed(Vector3Int cellPosition) {
        _shipGrid.RoomDestroyed(cellPosition);
        
        // Update UI with room count.

        // Trigger any other logic for when a room is destroyed, such as spawning enemies or triggering events.
        CheckGameOverCondition();
    }

    public bool IsPodOutsideCircle(Vector3 circleCenter, float radius) {
        return PlayerPodInteraction.IsPodOutsideCircle(circleCenter, radius);
    }

    public void MovePodOutsideShipBounds(Vector3 circleCenter, float radius) {
        PlayerPodInteraction.MovePodOutsideShipBounds(circleCenter, radius);
    }

    public Transform GetPodTransform() {
        return PlayerPodInteraction.PodTransform;
    }

    public Vector3Int GetRoomPosition(BaseRoom room) {
        return _shipGrid.GetRoomPosition(room);
    }

    public void RemoveRoom(Vector3Int cellPosition) {
        _shipGrid.RemoveRoom(cellPosition);   
    }

    public void CheckGameOverCondition() {
        if (_shipGrid.RoomCount == 0) {
            // Show Game Over screen.
        }
    }

    public void SetZoomLevelOne() {
        virtualCamera.m_Lens.OrthographicSize = 7.5f;
    }

    public void SetZoomLevelTwo() {
        virtualCamera.m_Lens.OrthographicSize = 20;
    }

    public void CrewQuartersActivated() {
        MaxPopulationCount += CrewQuartersMod;
    }

    public void CrewQuartersDeactivated() {
        MaxPopulationCount -= CrewQuartersMod;
    }

    public void ActivatedGeneratorRoom(GeneratorRoom generatorRoom) {
        _shipGrid.ActivatedGeneratorRoom(generatorRoom);
    }

    public void DeactivatedGeneratorRoom(GeneratorRoom generatorRoom) {
        _shipGrid.DeactivatedGeneratorRoom(generatorRoom);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(AttackInterval);

            // Randomly decide whether to spawn aliens or asteroids
            bool spawnAlien = Random.value > 0.5f; // 50% chance to spawn either

            if (spawnAlien)
            {
                int numberOfAliens = Random.Range(2, 4);
                for (int i = 0; i < numberOfAliens; i++)
                {
                    SpawnAlienShip();
                }
            }
            else
            {
                int numberOfAsteroids = Random.Range(2, 4);
                for (int i = 0; i < numberOfAsteroids; i++)
                {
                    SpawnAsteroid();
                }
            }
        }
    }

    private void SpawnAlienShip()
    {
        Vector3 spawnPosition = ShipTransform.position + (Vector3)(Random.insideUnitCircle.normalized * SpawnDistance);
        Instantiate(AlienShipPrefab, spawnPosition, Quaternion.identity);
    }

    private void SpawnAsteroid()
    {
        Vector3 spawnPosition = ShipTransform.position + (Vector3)(Random.insideUnitCircle.normalized * SpawnDistance);
        Instantiate(AsteroidPrefab, spawnPosition, Quaternion.identity);
    }

    public List<BaseRoom> GetListOfRoomsTooCloseToRoomsOfSameType(RoomTypes roomType, int range) {
        List<BaseRoom> roomList = new List<BaseRoom>();
        foreach (var cellPosition in _shipGrid.GetCellPositionsForRoomsOfType(roomType)) {
            List<BaseRoom> nearbyRooms = _shipGrid.GetRoomsInRangeOfCellPosition(range, cellPosition);
            if (nearbyRooms.Where(x => x.RoomType == roomType).ToList().Count > 0) {
                roomList.Add(_shipGrid.GetRoomAtPosition(cellPosition));
            }

        }

        return roomList;
    }

    public List<BaseRoom> GetListOfRoomsOfTypeInRangeOfRoomsOfType(RoomTypes roomType1, RoomTypes roomType2, int range) {
        List<BaseRoom> roomList = new List<BaseRoom>();
        foreach (var cellPosition in _shipGrid.GetCellPositionsForRoomsOfType(roomType1)) {
            List<BaseRoom> nearbyRooms = _shipGrid.GetRoomsInRangeOfCellPosition(range, cellPosition);
            if (nearbyRooms.Where(x => x.RoomType == roomType2).ToList().Count > 0) {
                roomList.Add(_shipGrid.GetRoomAtPosition(cellPosition));
            }

        }

        return roomList;
    }

    public int GetRoomCountOfRoomType(RoomTypes roomType) {
        return _shipGrid.GetRoomCountOfRoomType(roomType);
    }

    public bool ShipHasMajorityBufferRooms() { return _shipGrid.IsMajorityBufferRooms; }
}
