using Scripts.Shared;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
    private Health _health;

    public int CollisionDamage = 1;

    private void Awake() {
        _health = GetComponent<Health>();
    }

    private void Update() {
        if (_health.IsDestroyed) {
            Destroy(gameObject);
        }
    }
}
