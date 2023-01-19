using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static int enemyCount = 0;
    private int maxEnemyCount = 5;
    public static int powerUpCount = 0;
    private int maxPowerUpCount = 5;
    public static List<GameObject> enemySpawnList = new List<GameObject>();
    public static List<GameObject> powerUpList = new List<GameObject>();
    public static List<GameObject> pickUpList = new List<GameObject>();
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;
    public GameObject magazinePrefab;
    private Player _player;
    private GameObject _nextLevelButton;
    public static bool gameOver;
    private GameObject startMenu;

    // 67,71  115,71  115, 111
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _nextLevelButton = transform.GetChild(0).gameObject;
        startMenu = GameObject.Find("Canvas");
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerUpSpawn());
        StartCoroutine(EnemyChallengeSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        startScreenCheck();
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
    
    private IEnumerator EnemyChallengeSpawn()
    {
        while (true)
        {
            if (ChallengeMode.challengeStart)
            {
                yield return new WaitForSeconds(1);
                enemySpawnList.Add(Instantiate(enemyPrefab, randomSpawnPoint(true), enemyPrefab.transform.rotation));
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
                powerUpList.Add(Instantiate(powerUpPrefab, randomSpawnPoint(false), powerUpPrefab.transform.rotation));
                pickUpList.Add(Instantiate(magazinePrefab, randomSpawnPoint(false), powerUpPrefab.transform.rotation));
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
            if (ChallengeMode.challengeStart)
            {
                returnVector =  new Vector3(Random.Range(67,115), enemyPrefab.transform.position.y, Random.Range(71, 111));
            }
            else
            {
                returnVector =  new Vector3(Random.Range(0,50), enemyPrefab.transform.position.y, Random.Range(0, 50));
            }
        }
        else
        {
            returnVector =  new Vector3(Random.Range(67,115), powerUpPrefab.transform.position.y, Random.Range(71, 111));
        }
        return returnVector;
    }
    
    private void startScreenCheck()
    {
        if (!gameOver)
        {
            if (Vector3.Distance(_player.transform.position, transform.GetChild(0).position) < 2.5f)
            {
                startMenu.transform.GetChild(10).gameObject.SetActive(true);
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
                startMenu.transform.GetChild(10).gameObject.SetActive(false);
            }
        }
    }
}
