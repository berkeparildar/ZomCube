using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Player player;
    private bool hasTarget;
    private Vector3 startingPos;
    private int rangeValue = 10;
    private int health = 100 * NewGamePlus.level;
    private bool isInChallenge;
    private GameObject startMenu;
    private ChallengeMode ChallengeMode;

    // Start is called before the first frame update
    void Start()
    {
        startMenu = GameObject.Find("Canvas");
        player = GameObject.Find("Player").GetComponent<Player>();
        startingPos = transform.position;
        StartCoroutine(Patrol());
        StartCoroutine(AttackInRange());
        ChallengeMode = GameObject.Find("challenge_start").GetComponent<ChallengeMode>();
        if (transform.position.x is > 67 and < 115 && transform.position.z is > 71 and < 111)
        {
            isInChallenge = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInChallenge)
        {
            ChaseInChallenge();
        }
        else
        {
            lookForTarget();
        }
    }

    private IEnumerator AttackInRange()
    {
        while (true)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 2.5 && player.isAlive)
            {
                player.TakeDamage();
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }
    
    private void lookForTarget()
    {
        if (!SpawnManager.gameOver)
        {
            if (Vector3.Distance(player.gameObject.transform.position, transform.position) < rangeValue && player.isAlive)
            {
                hasTarget = true;
                rangeValue = 15;
                Vector3 targetDirection = new Vector3(player.transform.position.x, transform.position.y,
                    player.transform.position.z);
                transform.LookAt(targetDirection);
                Chase();
            }
            else
            {
                if (hasTarget) 
                {
                    rangeValue = 10;
                    transform.position = Vector3.MoveTowards(transform.position, startingPos, 4 * Time.deltaTime);
                    transform.LookAt(startingPos);
                    if (hasTarget && transform.position == startingPos)
                    {
                        hasTarget = false;
                    }
                }
            }
        }
    }
    
    private void ChaseInChallenge()
    {
        if (!SpawnManager.gameOver)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 50)
            {
                Vector3 targetDirection = new Vector3(player.transform.position.x, transform.position.y,
                    player.transform.position.z);
                transform.LookAt(targetDirection);
                Chase();
            }
        }
    }

    private void Chase()
    {
        Vector3 enemyMove = new Vector3(0, 0, 4);
        if (!SpawnManager.gameOver)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 2)
            {
                transform.Translate(enemyMove * Time.deltaTime);
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (isInChallenge)
            {
                ChallengeMode.IncreaseScore();
                Destroy(gameObject);
            }
            else
            {
                SpawnManager.enemyCount--;
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            yield return StartCoroutine(MoveForward());
            yield return StartCoroutine(TurnRight());
            yield return StartCoroutine(MoveForward());
            yield return StartCoroutine(TurnLeft());
            while (hasTarget)
            {
                yield return null;
            }
        }
    }

    private IEnumerator TurnRight()
    {
        float angleSpeed = 30;
        float totalAngle = Random.Range(30, 90);
        for (float angleSoFar = 0; angleSoFar < totalAngle; angleSoFar += angleSpeed * Time.deltaTime)
        {
            transform.Rotate(0, angleSpeed * Time.deltaTime, 0 );
            yield return null;
        }
    }
    
    private IEnumerator TurnLeft()
    {
        float angleSpeed = -30;
        float totalAngle = Random.Range(-90, -30);
        for (float angleSoFar = 0; angleSoFar > totalAngle; angleSoFar += angleSpeed * Time.deltaTime)
        {
            transform.Rotate(0, angleSpeed * Time.deltaTime, 0 );
            yield return null;
        }
    }
    
    private IEnumerator MoveForward()
    {
        Vector3 forward = Vector3.forward;

        for (float durationSoFar = 0; durationSoFar < 2.0f; durationSoFar += Time.deltaTime)
        {
            transform.Translate(forward * Time.deltaTime);
            yield return null;
        }
    }
}
