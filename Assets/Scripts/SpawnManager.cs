using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private List<Vector3> spawnPositions;
    private int enemyCount;
    public int maxEnemyCount = 10;

    [SerializeField] private GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        spawnPositions = new List<Vector3>();
        spawnPositions.Add(new Vector3(5,0,5));
        spawnPositions.Add(new Vector3(25,0,5));
        spawnPositions.Add(new Vector3(5,0,25));
        spawnPositions.Add(new Vector3(50,0,50));
        StartCoroutine(enemySpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator enemySpawn()
    {
        yield return new WaitForSeconds(3);
        while (enemyCount < maxEnemyCount)
        {
            Instantiate(enemyPrefab, randomSpawnPoint(), Quaternion.identity);
            enemyCount++;
            yield return new WaitForSeconds(5);
        }
        
    }

    private Vector3 randomSpawnPoint()
    {
        return spawnPositions[Random.Range(0, spawnPositions.Capacity - 1)];
    }
}
