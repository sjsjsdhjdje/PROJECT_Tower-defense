using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject enemyPrefab;
        public int count = 5;
    }

    [System.Serializable]
    public class Batch
    {
        public EnemyGroup[] groups;

        public int repeatCount = 1;
        public float interval = 10f;
    }

    [System.Serializable]
    public class Wave
    {
        public Batch[] batches;
        public float delayAfterWave = 10f;
    }

    public Wave[] waves;
    public Transform[] waypoints;

    public float startDelay = 3f;
    public float spawnInterval = 0.3f;

    private bool started = false;

    public void StartWaves()
    {
        if (started) return;

        started = true;
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        // Ожидание перед первой волной
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < waves.Length; i++)
        {
            Debug.Log("Началась волна " + (i + 1));

            // Перебираем партии внутри волны
            for (int b = 0; b < waves[i].batches.Length; b++)
            {
                Batch batch = waves[i].batches[b];

                // Повторяем партию нужное количество раз
                for (int repeat = 0; repeat < batch.repeatCount; repeat++)
                {
                    List<GameObject> enemies = new List<GameObject>();

                    // Формируем список врагов
                    foreach (EnemyGroup group in batch.groups)
                    {
                        if (group.enemyPrefab == null)
                            continue;

                        for (int j = 0; j < group.count; j++)
                        {
                            enemies.Add(group.enemyPrefab);
                        }
                    }

                    // Случайно перемешиваем врагов
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        int randomIndex = UnityEngine.Random.Range(
                            j,
                            enemies.Count
                        );

                        GameObject temp = enemies[j];
                        enemies[j] = enemies[randomIndex];
                        enemies[randomIndex] = temp;
                    }

                    // Создаём врагов по очереди
                    foreach (GameObject prefab in enemies)
                    {
                        GameObject enemy = Instantiate(
                            prefab,
                            waypoints[0].position,
                            Quaternion.identity
                        );

                        EnemyMovement movement =
                            enemy.GetComponent<EnemyMovement>();

                        if (movement != null)
                        {
                            movement.waypoints = waypoints;
                        }

                        yield return new WaitForSeconds(spawnInterval);
                    }

                    // Проверяем, есть ли следующая партия
                    bool hasNextRepeat =
                        repeat < batch.repeatCount - 1;

                    bool hasNextBatch =
                        b < waves[i].batches.Length - 1;

                    // Делаем паузу перед следующей партией
                    if (hasNextRepeat || hasNextBatch)
                    {
                        yield return new WaitForSeconds(batch.interval);
                    }
                }
            }

            Debug.Log("Волна " + (i + 1) + " завершена");

            // Пауза между волнами
            if (i < waves.Length - 1)
            {
                yield return new WaitForSeconds(
                    waves[i].delayAfterWave
                );
            }
        }

        Debug.Log("Все волны завершены!");
    }
}
