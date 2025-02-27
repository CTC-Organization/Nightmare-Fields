
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class WaveManager : MonoBehaviour
{
    [Header("Configurações da Wave")]

    public Transform[] spawnPoints;     // Pontos de spawn
    public float spawnDelay = 0.5f;     // Delay entre os spawns
    private List<GameObject> currentEnemies = new List<GameObject>();


    [Header("Configuração da progressão por wave")]
    public int currentDay = 1;          // Escolha o dia manualmente

    [SerializeField]
    float speedAugmentPerDay = .75f;
    [SerializeField]
    float healthAugmentPerDay = .2f;

    [Header("Configuração dos inimigos")]
    public GameObject bossPrefab;

    public GameObject enemyPrefab;      // Prefab do inimigo
    public int[] enemiesPerDay = { 20, 40, 60, 80, 1000 };
    public GameObject gargoylePrefab;      // Prefab do inimigo
    public int[] gargoylesPerDay = { 10, 20, 30, 40, 50 };
    [SerializeField] GameObject speedZombiePrefab;

    [Header("Eventos nas waves")]
    public int circleWaveNumber;

    [SerializeField] int nightmareModeProb = 10; // colocar 10-%

    void Start()
    {
        if (Random.Range(0, 100) < nightmareModeProb)
        {
            GameManager.instance.isNightmareMode = true;
        }
        currentDay = DayManager.dm.days;

        Debug.Log("currentDay :" + currentDay + "\nDayManager.dm.days: " + DayManager.dm.days);
        StartCoroutine(SpawnWave(currentDay));
    }

    IEnumerator SpawnWave(int day)
    {
        healthAugmentPerDay *= currentDay;
        int enemyCount = GetEnemiesForDay(day);
        Debug.Log($"🌊 Dia {day} - Inimigos: {enemyCount}");

        if (!GameManager.instance.isNightmareMode)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }
        }
        else
        {
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnOtherEnemy(speedZombiePrefab);
                yield return new WaitForSeconds(spawnDelay);
            }
        }


        if (currentDay == 4) // corrigir futuramente
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            currentEnemies.Add(enemy);
        }

        if (circleWaveNumber < 1 && day >= 3F)
        {
            GetComponent<CircleWaveSpawner>().StartCircleWave();
        }

        if (currentDay >= 5)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            currentEnemies.Add(enemy);
            int gargoylesCount = gargoylesPerDay[System.Math.Min(currentDay, 5) - 5];
            // int gargoylesCount = gargoylesPerDay[currentDay - 1];
            for (int i = 0; i < gargoylesCount; i++)
            {
                SpawnOtherEnemy(gargoylePrefab);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        currentEnemies.Add(enemy);


        enemy.GetComponent<AIPath>().maxSpeed += speedAugmentPerDay * currentDay; // dia 1 - 2.5f, 2- 3f, 3- 3.5f, 4 - 4f,5- 4.5f,6- 5f,7- 5.5f, 7-6f, 8-6.5f ,9 - 7, 10 - 7.5f
        // aumenta Speed a cada wave

        float lifeAugment = (float)System.Math.Floor(healthAugmentPerDay);
        enemy.GetComponent<EnemyHealth>().startingHealth += lifeAugment;
        enemy.GetComponent<EnemyHealth>().currentHealth += lifeAugment; // aumenta HP a cada wave
    }

    void SpawnOtherEnemy(GameObject otherEnemyPrefab)
    {
        if (spawnPoints.Length == 0 || otherEnemyPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject otherEnemy = Instantiate(otherEnemyPrefab, spawnPoint.position, Quaternion.identity);
        currentEnemies.Add(otherEnemy);
    }

    int GetEnemiesForDay(int day)
    {
        if (day > 0 && day <= enemiesPerDay.Length)
        {
            Debug.Log($"enimies per day {enemiesPerDay[day - 1]}");
            return enemiesPerDay[day - 1];
        }
        return enemiesPerDay[^1]; // Último valor como padrão
    }
}
