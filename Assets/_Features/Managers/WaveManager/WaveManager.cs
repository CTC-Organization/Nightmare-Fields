
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class WaveManager : MonoBehaviour
{
    [Header("Configurações da Wave")]
    public GameObject enemyPrefab;      // Prefab do inimigo
    public GameObject bossPrefab;
    public Transform[] spawnPoints;     // Pontos de spawn
    public float spawnDelay = 0.5f;     // Delay entre os spawns

    [Header("Configuração dos Dias")]
    public int currentDay = 1;          // Escolha o dia manualmente
    public int[] enemiesPerDay = { 20, 40, 60, 80, 1000 };
    [SerializeField]
    float speedAugmentPerDay = .75f;
    [SerializeField] float healthAugmentPerDay = .2f;
    public int circleWaveNumber;


    private List<GameObject> currentEnemies = new List<GameObject>();

    void Start()
    {
        currentDay = DayManager.dm.days;

        Debug.Log("currentDay :" + currentDay + "\nDayManager.dm.days: " + DayManager.dm.days);
        StartCoroutine(SpawnWave(currentDay));
    }

    IEnumerator SpawnWave(int day)
    {
        healthAugmentPerDay *= currentDay;
        int enemyCount = GetEnemiesForDay(day);
        Debug.Log($"🌊 Dia {day} - Inimigos: {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
            if (i == enemyCount / 2 && circleWaveNumber < 1 && day % 2 == 0) // spawn do circulo após spawnar metade do inimigos
            {// spawna circulo nos dias impares
                GetComponent<CircleWaveSpawner>().StartCircleWave();
            }
        }

        if (currentDay == 5) // corrigir futuramente
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            currentEnemies.Add(enemy);
        }
        // else if (currentDay == 10) // corrigir futuramente - SPAWNA Boss cospe fogo
        // {
        //     Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        //     GameObject enemy = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        //     currentEnemies.Add(enemy);
        // }
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
