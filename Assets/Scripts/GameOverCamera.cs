using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCamera : MonoBehaviour
{
    private GameObject health;
    private GameObject bullet;
    private GameObject action;
    private GameObject gameOver;
    private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        health = GameObject.Find("health_UI");
        bullet = GameObject.Find("bullet_UI");
        action = GameObject.Find("action_bar");
        boss = GameObject.Find("boss_UI");
        gameOver = GameObject.Find("Canvas").transform.GetChild(8).gameObject;
        health.SetActive(false);
        bullet.SetActive(false);
        action.SetActive(false);
        if (!boss.GetComponent<Boss>().isDead)
        {
            boss.SetActive(false);
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
            gameOver.SetActive(true);
            yield return new WaitForSeconds(1);
            gameOver.SetActive(false);
            yield return new WaitForSeconds(1);
        }
    }
}
