using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventsManager : MonoBehaviourSingleton<RandomEventsManager>
{
    private delegate void EventMethod();
    private List<EventMethod> eventMethods;


    private float eventChance = 1.0f;
    public int popUpCount = 2;
    private int _popUpRemain;

    private float totalGameSecond;

    [Header("Event Spawn Enemies")]
    public float distanceFormShip;
    public GameObject asteroid;
    public int numberOfAsteroid;

    public GameObject alien;
    public int numberOfAlien;

    void Start()
    {
        //list of random event
        eventMethods = new List<EventMethod>
        {
            AlienEvent,
            AsteroidEvent,
            ShopOutpostEvent,
            EmbassyOutpostEvent
        };
    }
    public void InitRandomEvent() {
        totalGameSecond = 1 / UIManager.Instance.gameProgressSpeed;
        _popUpRemain = popUpCount;
        StartCoroutine(CheckForRandomEvent());
    }

    private IEnumerator CheckForRandomEvent()
    {
        _popUpRemain -= 1;
        yield return new WaitForSeconds(totalGameSecond / (UIManager.Instance.electionEventNum + 1) / (popUpCount + 1));

        //only call random event if there is pop up remain
        if (_popUpRemain > -1)
        {
            if (Random.value < eventChance)
            {
                TriggerRandomEvent();
            }
        }
        else {
            Reset();
        }

        StartCoroutine(CheckForRandomEvent());
    }

    private void TriggerRandomEvent()
    {
        Debug.Log("A random event has been triggered!");
        int randomIndex = Random.Range(0, eventMethods.Count);
        eventMethods[randomIndex]?.Invoke();
    }
    public void ShopOutpostEvent()
    {
        SoundManager.Instance.PlayShopBGM();
        GameManager.Instance.StopGameTimeScale();
        UIManager.Instance.ShowShopOutpost();
        UpgradeStore.Instance.AssignRandomPerks();
        RoomStore.Instance.AssignRandomRooms();
    }

    public void EmbassyOutpostEvent() {
        SoundManager.Instance.PlayShopBGM();
        GameManager.Instance.StopGameTimeScale();
        UIManager.Instance.ShowEmbassyOutpost();
        UpgradeStore.Instance.AssignRandomPerks();
        Embassy.Instance.ShowTradeRefugees();
    }

    public void AsteroidEvent() {
        float angleOffset = GetRandomDirectionAngle();

        StartCoroutine(SpawnEnemiesInArc(asteroid, numberOfAsteroid * GameManager.Instance.currentElectionCycle, 10f, 90f, angleOffset,2));
        RandomEventPopup.Instance.ShowEventPopUp("Asteroid detected! Enemies are emerging!");
    }

    public void AlienEvent() {
        float angleOffset = GetRandomDirectionAngle();

        StartCoroutine(SpawnEnemiesInArc(alien, numberOfAlien*GameManager.Instance.currentElectionCycle, 10f, 90f, angleOffset, 2));
        RandomEventPopup.Instance.ShowEventPopUp("Alien Ship detected! Enemies are emerging!");
    }
    private IEnumerator SpawnEnemiesInArc(GameObject enemyPrefab, int totalEnemyCount, float initialRadius, float arcAngle, float angleOffset, float radiusIncrement)
    {
        int enemiesPerLayer = 9;
        int layers = Mathf.CeilToInt((float)totalEnemyCount / enemiesPerLayer);

        for (int layer = 0; layer < layers; layer++)
        {
            float currentRadius = initialRadius + layer * radiusIncrement;
            int enemiesInThisLayer = Mathf.Min(enemiesPerLayer, totalEnemyCount - layer * enemiesPerLayer);
            for (int i = 0; i < enemiesInThisLayer; i++)
            {
                //find the angle
                float angleStep = arcAngle / (enemiesInThisLayer);
                float currentAngle = angleOffset + (angleStep * i);

                //find the spawn position around the ship
                var spawnPos = CalculateSpawnPosition(currentAngle, currentRadius);

                //spawn the enemy at the calculated position
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private Vector3 CalculateSpawnPosition(float angle, float radius)
    {
        Vector3 shipPosition = GameManager.Instance.Ship.transform.position;
        angle *= Mathf.Deg2Rad;

        //find the position in 2D space
        float x = shipPosition.x + Mathf.Cos(angle) * radius;
        float y = shipPosition.y + Mathf.Sin(angle) * radius;

        //return the calculated position
        return new Vector3(x, y, 0);
    }

    private float GetRandomDirectionAngle()
    {
        int direction = Random.Range(0, 4);
        float angleOffset = 0f;
        switch (direction)
        {
            case 0: // North
                angleOffset = 45f;
                break;
            case 1: // East
                angleOffset = 135f; 
                break;
            case 2: // South
                angleOffset = 225f; 
                break;
            case 3: // West
                angleOffset = 315f;
                break;
        }
        return angleOffset;
    }

    public void Reset()
    {
        _popUpRemain = popUpCount;
    }

}
