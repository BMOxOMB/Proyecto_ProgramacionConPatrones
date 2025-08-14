using UnityEngine;
using UnityEngine.Events;

namespace RTSGame.AI
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
    }

    /// <summary>
    /// Health component with damage, death events, and IDamageable implementation
    /// </summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHP = 100f;
        [SerializeField] private bool destroyOnDeath = true;
        [SerializeField] private float deathDelay = 0f;

        [Header("Events")]
        public UnityEvent<float> OnDamageTaken;  // Passes current health percentage
        public UnityEvent<float> OnHealed;       // Passes current health percentage
        public UnityEvent OnDeath;

        public float MaxHP => maxHP;
        public float CurrentHP { get; private set; }
        public float HealthPercentage => CurrentHP / maxHP;
        public bool IsDead => CurrentHP <= 0f;

        private void Awake()
        {
            CurrentHP = maxHP;
        }

        /// <summary>
        /// Implementation of IDamageable interface
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (IsDead || damage <= 0) return;

            ApplyDamage(damage);
        }

        /// <summary>
        /// Apply damage to this object. Negative values heal.
        /// </summary>
        public void ApplyDamage(float amount)
        {
            if (IsDead) return;

            float previousHP = CurrentHP;
            CurrentHP = Mathf.Clamp(CurrentHP - amount, 0f, maxHP);

            // Trigger appropriate events
            if (amount > 0)
            {
                OnDamageTaken?.Invoke(HealthPercentage);
            }
            else if (amount < 0)
            {
                OnHealed?.Invoke(HealthPercentage);
            }

            // Handle death
            if (CurrentHP <= 0f && previousHP > 0f)
            {
                Die();
            }
        }

        /// <summary>
        /// Heals the specified amount
        /// </summary>
        public void Heal(float amount)
        {
            ApplyDamage(-amount);
        }

        /// <summary>
        /// Fully heals the character
        /// </summary>
        public void FullHeal()
        {
            CurrentHP = maxHP;
            OnHealed?.Invoke(1f);
        }

        private void Die()
        {
            OnDeath?.Invoke();

            if (destroyOnDeath)
            {
                Destroy(gameObject, deathDelay);
            }
            else
            {
                enabled = false;
            }
        }

        // Example of how to hook up events in the inspector or code
        private void OnEnable()
        {
            OnDeath.AddListener(HandleDeath);
        }

        private void OnDisable()
        {
            OnDeath.RemoveListener(HandleDeath);
        }

        private void HandleDeath()
        {
            // Add any death handling logic here
            Debug.Log($"{gameObject.name} has died!");
        }
    }
}