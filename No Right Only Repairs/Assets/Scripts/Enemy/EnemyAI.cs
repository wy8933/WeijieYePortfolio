using Scripts.Shared;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float shootingCooldown = 1f;

    private EnemyMovement _enemyMovement;
    private float _timer;

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (_timer <= shootingCooldown) {
            _timer += Time.deltaTime;
        }

        if (_enemyMovement.IsInRange() && _timer >= shootingCooldown)
        {
            ShootBullet();
            _timer -= shootingCooldown;
        }

        if (GetComponent<Health>().GetHealth() <= 0) {
            Destroy(gameObject);
        }
    }

    void ShootBullet()
    {
        if (bulletPrefab != null && shootingPoint != null)
        {
            Vector2 direction = (_enemyMovement.GetTarget().position - shootingPoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.Euler(new Vector3(0,0,angle)));
            SoundManager.Instance.PlaySound(SoundName.PirateShipShootsAlternate);
        }
    }

    private void OnDestroy()
    {
        int randomDeathSound = Random.Range(0,4);

        switch (randomDeathSound) {
            case 0:
                SoundManager.Instance.PlaySound(SoundName.Death1);
                break;
            case 1:
                SoundManager.Instance.PlaySound(SoundName.Death2);
                break;
            case 2:
                SoundManager.Instance.PlaySound(SoundName.Death3);
                break;
            case 3:
                SoundManager.Instance.PlaySound(SoundName.Death4);
                break;

        }
    }
}
