using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static int enemyCount;
    [SerializeField] private int maxEnemyCount = 10;
    public static int powerUpCount;
    [SerializeField] private int maxPowerUpCount = 5;
    private static Transform[] _spawnPositions;
    public static readonly List<GameObject> EnemySpawnList = new List<GameObject>();
    public static readonly List<GameObject> PowerUpList = new List<GameObject>();
    public List<GameObject> pickUpList = new List<GameObject>();
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;
    public GameObject magazinePrefab;
    private Player _player;
    public static bool gameOver;
    private GameObject _startMenu;
    private Transform[] _challengePositions;

    // 67,71  115,71  115, 111
    private void Start()
    {
        _spawnPositions = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        _challengePositions = GameObject.Find("ChallengePoints").GetComponentsInChildren<Transform>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _startMenu = GameObject.Find("Canvas");
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerUpSpawn());
        StartCoroutine(EnemyChallengeSpawn());
    }

    // Update is called once per frame
    private void Update()
    {
        StartScreenCheck();
    }

    private IEnumerator EnemySpawn()
    {
        while (true)
        {
            if (enemyCount < maxEnemyCount)
            {
                yield return new WaitForSeconds(4);
                Instantiate(enemyPrefab, RandomSpawnPoint(true), enemyPrefab.transform.rotation);
                enemyCount++;
                yield return new WaitForSeconds(5);
            }
            else
            {
                yield return null;
            }
        }
    }
    
    private IEnumerator EnemyChallengeSpawn()
    {
        while (true)
        {
            if (ChallengeMode.challengeStart)
            {
                yield return new WaitForSeconds(2);
                var enemy = Instantiate(enemyPrefab, RandomSpawnPoint(true), enemyPrefab.transform.rotation);
                enemy.GetComponent<Enemy>().isInChallenge = true;
                EnemySpawnList.Add(enemy);
                yield return new WaitForSeconds(2);
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
            if (powerUpCount < maxPowerUpCount && ChallengeMode.challengeStart)
            {
                var rotation = powerUpPrefab.transform.rotation;
                PowerUpList.Add(Instantiate(powerUpPrefab, RandomSpawnPoint(false), rotation));
                pickUpList.Add(Instantiate(magazinePrefab, RandomSpawnPoint(false), rotation));
                powerUpCount++;
                yield return new WaitForSeconds(9);
            }
            else
            {
                yield return null;
            }
        }
    }

    private Vector3 RandomSpawnPoint(bool isEnemy)
    {
        Vector3 returnVector;
        if (isEnemy)
        {
            if (ChallengeMode.challengeStart)
            {
                var random = Random.Range(0, _challengePositions.Length - 1);
                returnVector = _challengePositions[random].position;
            }
            else
            {
                var random = Random.Range(0, _spawnPositions.Length - 1);
                returnVector = _spawnPositions[random].position;
            }
        }
        else
        {
            var random = Random.Range(0, _challengePositions.Length - 1);
            returnVector = _challengePositions[random].position;
        }
        return returnVector;
    }
    
    private void StartScreenCheck()
    {
        if (!gameOver)
        {
            if (Vector3.Distance(_player.transform.position, transform.GetChild(0).position) < 2.5f)
            {
                _startMenu.transform.GetChild(10).gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    NewGamePlus.IncreaseLevel();
                    Weapon.hasOneHealthKit = false;
                    Weapon.hasTwoHealthKit = false;
                    Weapon.shotGunUnlock = false;
                    Weapon.machineGunUnlock = false;
                    ChallengeMode.challengeScore = 0;
                    SceneManager.LoadScene(1);
                }
            }
            else
            {
                _startMenu.transform.GetChild(10).gameObject.SetActive(false);
            }
        }
    }
}
