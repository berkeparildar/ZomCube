using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeMode : MonoBehaviour
{
    private GameObject _player;
    public static bool challengeStart;
    private GameObject _startMenu;
    private GameObject _barrier;
    private bool _isClicked;
    private Player _movementScript;
    public static int challengeScore = 0;
    private bool _movePlayer;
    private bool _movePlayerAfterFinish;
    private Text _challengeResult;
    private GameObject _bossBarrier;

    // Start is called before the first frame update
    private void Start()
    {
        _bossBarrier = GameObject.Find("BossBarrier");
        _player = GameObject.Find("Player");
        _challengeResult = GameObject.Find("challenge_result").GetComponent<Text>();
        _startMenu = GameObject.Find("Canvas");
        _movementScript = _player.GetComponent<Player>();
        _barrier = GameObject.Find("ChallengeArea").transform.GetChild(0).gameObject;
        StartCoroutine(MovePlayer());
    }

    // Update is called once per frame
    private void Update()
    {
        StartScreenCheck(_isClicked);
    }

    private IEnumerator MovePlayer()
    {
        while (true)
        {
            if (_movePlayer)
            {
                _movementScript.enabled = false;
                yield return new WaitForSeconds(0.5f);
                _player.transform.position = new Vector3(66, 2.25f, -25);
                _startMenu.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Score: " + challengeScore;
                yield return new WaitForSeconds(0.5f);
                _movementScript.enabled = true;
                _movePlayer = false;
            }
            else if (_movePlayerAfterFinish)
            {
                _movementScript.enabled = false;
                if (challengeScore < 100)
                {
                    _challengeResult.text = "Challenge Failed!";
                    challengeScore = 0;
                }
                else
                {
                    _challengeResult.text = "Challenge Passed!";
                    _bossBarrier.SetActive(false);
                    challengeScore = 0;
                }
                yield return new WaitForSeconds(0.5f);
                _player.transform.position = transform.position;
                yield return new WaitForSeconds(0.5f);
                _movementScript.enabled = true;
                _movePlayerAfterFinish = false;
                _challengeResult.text = "";
            }
            else
            {
                yield return null;
            }
        }
        
    }

    private void StartScreenCheck(bool clicked)
    {
        if (!SpawnManager.gameOver)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 2.5f && !clicked)
            {
                _startMenu.transform.GetChild(3).gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    challengeStart = true;
                    _movePlayer = true;
                    _barrier.SetActive(true);
                    _isClicked = true;
                    _startMenu.transform.GetChild(3).gameObject.SetActive(false);
                    _startMenu.transform.GetChild(4).gameObject.SetActive(true);
                }
            }
            else
            {
                _startMenu.transform.GetChild(3).gameObject.SetActive(false);
                return;
            }
        }
    }

    public void FinishChallenge()
    {
        challengeStart = false;
        _movePlayerAfterFinish = true;
        _barrier.SetActive(false);
        _isClicked = false;
        _startMenu.transform.GetChild(4).gameObject.SetActive(false);
        foreach (var enemy in SpawnManager.EnemySpawnList)
        {
            Destroy(enemy);
        }
        foreach (var powerUp in SpawnManager.PowerUpList)
        {
            Destroy(powerUp);
        }
    }

    public void IncreaseScore()
    {
        challengeScore += 5;
        _startMenu.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Score: " + challengeScore;
    }
}
