using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static int enemyCount = 5;
    private int maxEnemyCount = 5;
    public static int powerUpCount = 1;
    private int maxPowerUpCount = 5;
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;

    void Start()
    {
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerUpSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator EnemySpawn()
    {
        while (true)
        {
            if (enemyCount < maxEnemyCount)
            {
                yield return new WaitForSeconds(4);
                Instantiate(enemyPrefab, randomSpawnPoint(true), enemyPrefab.transform.rotation);
                enemyCount++;
                yield return new WaitForSeconds(5);
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator PowerUpSpawn()
    {
        while (true)
        {
            if (powerUpCount < maxPowerUpCount)
            {
                Instantiate(powerUpPrefab, randomSpawnPoint(false), powerUpPrefab.transform.rotation);
                powerUpCount++;
                yield return new WaitForSeconds(9);
            }
            else
            {
                yield return null;
            }
        }
    }

    private Vector3 randomSpawnPoint(bool isEnemy)
    {
        Vector3 returnVector;
        if (isEnemy)
        {
            returnVector =  new Vector3(Random.Range(0,50), enemyPrefab.transform.position.y, Random.Range(0, 50));
        }
        else
        {
            returnVector =  new Vector3(Random.Range(0,50), powerUpPrefab.transform.position.y, Random.Range(0, 50));
        }
        return returnVector;
    }
}
