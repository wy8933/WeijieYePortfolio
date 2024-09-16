using UnityEngine;

namespace Scripts.Shared {
    public class Health : MonoBehaviour {
        public float MaxHealth;
        private float _currentHealth;

        public bool IsDestroyed => (_currentHealth <= 0);

        public void Awake() {
            _currentHealth = MaxHealth;
        }

        public void MaxHealthUpdate(int amount) {
            MaxHealth += amount;
            _currentHealth += amount;
        }

        public void CurrentHealthUpdate(float amount) {
            _currentHealth += amount;
        }

        public float GetHealth() {
            return _currentHealth;
        }
    }
}