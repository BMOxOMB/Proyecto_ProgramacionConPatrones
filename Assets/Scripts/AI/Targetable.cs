using UnityEngine;

namespace RTSGame.AI
{
    /// <summary>
    /// Marks an entity as a potential target for AI units.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class Targetable : MonoBehaviour
    {
        [SerializeField] private Team team = Team.Player;
        [SerializeField] private bool isBuilding = false;
        [SerializeField] private bool isTownCenter = false;

        public Team Team
        {
            get => team;
            set => team = value;
        }

        public bool IsBuilding => isBuilding;
        public bool IsTownCenter => isTownCenter;
        public Transform TargetTransform => transform;
        public Health Health { get; private set; }
        public bool IsAlive => Health != null && Health.CurrentHP > 0f;

        private void Awake()
        {
            Health = GetComponent<Health>();
        }
    }
}
