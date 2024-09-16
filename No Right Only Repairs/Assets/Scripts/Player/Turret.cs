using UnityEngine;

public class Turret : MonoBehaviour
{
    public float attackingRange;
    public float attackingCooldown;

    private float _timer;

    public GameObject bulletPrefab;
    public GameObject turret;

    public Transform ShootingPoint;

    private GameObject _closestEnemy;

    public bool IsActive { get; set; } = true;

    private void FixedUpdate()
    {
        if (!IsActive) { return; }

        // Update the cooldown timer
        if (_timer < attackingCooldown * Settings.Instance.FireCooldownMultiplier)
            _timer += Time.fixedDeltaTime;

        // Find the closest enemy
        FindClosestEnemy();

        // If an enemy is within range, rotate and shoot
        if (_closestEnemy != null)
        {
            RotateTurretTowardsEnemy();
            if (_timer >= attackingCooldown * Settings.Instance.FireCooldownMultiplier)
            {
                ShootBullet();
                _timer = 0f;
            }
        }
    }

    /// <summary>
    /// find the closest gameobject with the Enemy tag
    /// </summary>
    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestEnemyDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float currentEnemyDistance = Vector2.Distance(enemy.transform.position, transform.position);

            if (currentEnemyDistance < attackingRange && currentEnemyDistance < closestEnemyDistance)
            {
                closestEnemyDistance = currentEnemyDistance;
                _closestEnemy = enemy;
            }
        }
    }

    /// <summary>
    /// rotate the turret towards the closest enemy
    /// </summary>
    private void RotateTurretTowardsEnemy()
    {
        Vector2 direction = (_closestEnemy.transform.position - ShootingPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        turret.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    /// <summary>
    /// shoot a bullet based on the turret's rotation
    /// </summary>
    private void ShootBullet()
    {
        float angle = turret.transform.rotation.eulerAngles.z;
        Instantiate(bulletPrefab, ShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, angle + 90)));
        SoundManager.Instance.PlaySound(SoundName.TurretShots);
    }

    public void DestroyTurret() {
        Destroy(gameObject);
    }
}
