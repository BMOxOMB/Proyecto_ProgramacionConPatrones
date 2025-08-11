using UnityEngine;
using UnityEngine.Events;

namespace RTSGame.AI
{
    /// <summary>
    /// Simple health component with damage and death event.
    /// </summary>
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHP = 100f;
        public float MaxHP => maxHP;
        public float CurrentHP { get; private set; }

        /// <summary>
        /// Invoked when health reaches zero.
        /// </summary>
        public UnityEvent OnDeath = new UnityEvent();

        private void Awake()
        {
            CurrentHP = maxHP;
        }

        /// <summary>
        /// Apply damage to this object. Negative values heal.
        /// </summary>
        public void ApplyDamage(float amount)
        {
            if (CurrentHP <= 0f) return;
            CurrentHP = Mathf.Clamp(CurrentHP - amount, 0f, maxHP);
            if (CurrentHP <= 0f)
            {
                OnDeath.Invoke();
            }
        }
    }
}
