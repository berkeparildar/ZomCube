using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCamera : MonoBehaviour
{
    private GameObject _health;
    private GameObject _bullet;
    private GameObject _action;
    private GameObject _gameOver;
    private GameObject _boss;
    private GameObject _player;
    private GameObject _bossUI;

    // Start is called before the first frame update
    void Start()
    {
        _health = GameObject.Find("health_UI");
        _bullet = GameObject.Find("bullet_UI");
        _action = GameObject.Find("action_bar");
        _bossUI = GameObject.Find("boss_UI");
        _boss = GameObject.Find("Boss");
        _player = GameObject.Find("Player");
        _gameOver = GameObject.Find("Canvas").transform.GetChild(8).gameObject;
        _health.SetActive(false);
        _bullet.SetActive(false);
        _action.SetActive(false);
        _player.GetComponent<MouseMovement>().enabled = false;
        if (_boss.GetComponent<Boss>().hasTarget)
        {
            _bossUI.SetActive(false);
        }
        StartCoroutine(GameOverScreen());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 1 * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
            NewGamePlus.level = 1;
            SpawnManager.gameOver = false;
            Weapon.shotGunUnlock = false;
            Weapon.machineGunUnlock = false;
            Weapon.hasOneHealthKit = false;
            Weapon.hasTwoHealthKit = false;
        }
    }

    private IEnumerator GameOverScreen()
    {
        while (true)
        {
            _gameOver.SetActive(true);
            yield return new WaitForSeconds(1);
            _gameOver.SetActive(false);
            yield return new WaitForSeconds(1);
        }
    }
}
