using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSGame.AI
{
    /// <summary>
    /// Spawns enemy units either continuously or in waves.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        private enum SpawnMode { Continuous, Waves }

        [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private SpawnMode mode = SpawnMode.Continuous;

        [Header("Continuous")]
        [SerializeField] private float spawnInterval = 5f;

        [Header("Waves")]
        [SerializeField] private int enemiesPerWave = 5;
        [SerializeField] private float timeBetweenWaves = 20f;
        [SerializeField] private float difficultyFactor = 1.1f;

        [Header("General")]
        [SerializeField] private int maxAlive = 25;
        [SerializeField] private Team team = Team.Enemy;

        private readonly List<Health> alive = new List<Health>();
        private int waveIndex = 0;

        private void Start()
        {
            StartCoroutine(mode == SpawnMode.Waves ? WaveRoutine() : ContinuousRoutine());
        }

        private IEnumerator ContinuousRoutine()
        {
            while (true)
            {
                if (alive.Count < maxAlive)
                {
                    SpawnEnemy();
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private IEnumerator WaveRoutine()
        {
            while (true)
            {
                int count = Mathf.RoundToInt(enemiesPerWave * Mathf.Pow(difficultyFactor, waveIndex));
                for (int i = 0; i < count; i++)
                {
                    SpawnEnemy();
                    yield return null; // spread spawns over frames
                }
                waveIndex++;
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        private void SpawnEnemy()
        {
            if (enemyPrefabs.Count == 0 || spawnPoints == null || spawnPoints.Length == 0)
                return;

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject instance = Instantiate(prefab, point.position, point.rotation);

            Targetable targetable = instance.GetComponent<Targetable>();
            if (targetable != null)
            {
                targetable.Team = team;
            }

            Health health = instance.GetComponent<Health>();
            if (health != null)
            {
                alive.Add(health);
                health.OnDeath.AddListener(() => alive.Remove(health));
            }
        }
    }
}
