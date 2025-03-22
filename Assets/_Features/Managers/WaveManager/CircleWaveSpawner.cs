
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CircleWaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab do zumbi
    public Transform targetPosition; // O ponto central (posição do player)
    public int numberOfEnemies = 10; // Quantidade de zumbis
    public float startRadius = 10f; // Raio inicial do círculo
    public float shrinkSpeed = 5f; // Velocidade de fechamento do círculo
    public float minRadius = 2f; // Raio mínimo antes de os zumbis desaparecerem

    private List<EnemyCircularMovement> enemies = new List<EnemyCircularMovement>();
    private WaveManager wm;

    public void Start()
    {
        wm = GetComponent<WaveManager>();
    }
    public void StartCircleWave()
    {
        targetPosition = GameObject.FindWithTag("Player").transform;
        SpawnEnemies();
        wm.circleWaveNumber++;
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float angle = i * (360f / numberOfEnemies);
            // isso é 3D e não 2d//Vector3 spawnPosition = targetPosition.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * startRadius;
            Vector3 spawnPosition = targetPosition.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * startRadius;

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyCircularMovement movement = enemy.AddComponent<EnemyCircularMovement>();
            movement.Initialize(targetPosition, startRadius, shrinkSpeed, minRadius);
            enemies.Add(movement);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, startRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minRadius);
    }
}

