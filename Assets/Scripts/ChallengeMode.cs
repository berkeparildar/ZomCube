using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeMode : MonoBehaviour
{
    private GameObject _player;
    public static bool challengeStart;
    private GameObject startMenu;
    private GameObject _barrier;
    private bool isClicked;
    private Player _movementScript;
    public static int challengeScore = 0;
    private bool movePlayer;
    private bool movePlayerAfterFinish;
    private Text challengeResult;
    private GameObject bossBarrier;

    // Start is called before the first frame update
    void Start()
    {
        bossBarrier = GameObject.Find("boss_barrier");
        _player = GameObject.Find("Player");
        challengeResult = GameObject.Find("challenge_result").GetComponent<Text>();
        startMenu = GameObject.Find("Canvas");
        _movementScript = _player.GetComponent<Player>();
        _barrier = GameObject.Find("Challenge_Area").transform.GetChild(0).gameObject;
        StartCoroutine(MovePlayer());
    }

    // Update is called once per frame
    void Update()
    {
        startScreenCheck(isClicked);
    }

    private IEnumerator MovePlayer()
    {
        while (true)
        {
            if (movePlayer)
            {
                _movementScript.enabled = false;
                yield return new WaitForSeconds(0.5f);
                _player.transform.position = new Vector3(90, 2.25f, 90);
                startMenu.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Score: " + challengeScore;
                yield return new WaitForSeconds(0.5f);
                _movementScript.enabled = true;
                movePlayer = false;
            }
            else if (movePlayerAfterFinish)
            {
                _movementScript.enabled = false;
                if (challengeScore < 100)
                {
                    challengeResult.text = "Challenge Failed!";
                    challengeScore = 0;
                }
                else
                {
                    challengeResult.text = "Challenge Passed!";
                    bossBarrier.SetActive(false);
                    challengeScore = 0;
                }
                yield return new WaitForSeconds(0.5f);
                _player.transform.position = new Vector3(90, 2.25f, 62);
                yield return new WaitForSeconds(0.5f);
                _movementScript.enabled = true;
                movePlayerAfterFinish = false;
                challengeResult.text = "";
            }
            else
            {
                yield return null;
            }
        }
        
    }

    private void startScreenCheck(bool clicked)
    {
        if (!SpawnManager.gameOver)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 2.5f && !clicked)
            {
                startMenu.transform.GetChild(3).gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    challengeStart = true;
                    // _movementScript.enabled = false;
                    // _player.transform.position = new Vector3(90, 2.25f, 90);
                    // _movementScript.enabled = true;
                    movePlayer = true;
                    _barrier.SetActive(true);
                    isClicked = true;
                    startMenu.transform.GetChild(3).gameObject.SetActive(false);
                    startMenu.transform.GetChild(4).gameObject.SetActive(true);
                }
            }
            else
            {
                startMenu.transform.GetChild(3).gameObject.SetActive(false);
                return;
            }
        }
    }

    public void FinishChallenge()
    {
        challengeStart = false;
        // _movementScript.enabled = false;
        // _player.transform.position = new Vector3(90, 2.25f, 62);
        // _movementScript.enabled = true;
        movePlayerAfterFinish = true;
        _barrier.SetActive(false);
        isClicked = false;
        startMenu.transform.GetChild(4).gameObject.SetActive(false);
        foreach (var VARIABLE in SpawnManager.enemySpawnList)
        {
            Destroy(VARIABLE);
        }
        foreach (var VARIABLE in SpawnManager.powerUpList)
        {
            Destroy(VARIABLE);
        }
    }

    public void IncreaseScore()
    {
        challengeScore += 5;
        startMenu.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Score: " + challengeScore;
    }
}
