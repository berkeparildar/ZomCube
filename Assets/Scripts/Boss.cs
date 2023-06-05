using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private int health = 1000 * NewGamePlus.level;
    private GameObject _canvas;
    public bool isDead;
    public bool hasTarget;
    private Player _player;
    private GameObject _spawnManager;
    private MeshRenderer _meshRenderer;
    private GameObject _barrier;
    public Material bossMat;
    public GameObject bullet;
    private Vector3 _enemyMove = new Vector3(0, 0, 2);

    private GameObject bossHealthBars;
    // Start is called before the first frame update
    private Color col;
    void Start()
    {
        _canvas = GameObject.Find("Canvas");
        _barrier = GameObject.Find("BossBarrier");
        _meshRenderer = GetComponent<MeshRenderer>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("SpawnManager");
        StartCoroutine(BossMovement());
        StartCoroutine(AttackInRange());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator BossMovement()
    {
        while (true)
        {
            if (!_barrier.activeSelf && Vector3.Distance(_player.transform.position, transform.position) < 40)
            {
                hasTarget = true;
                if (_canvas.transform.GetChild(7).gameObject.activeSelf == false)
                {
                    _canvas.transform.GetChild(7).gameObject.SetActive(true);
                    bossHealthBars = _canvas.transform.GetChild(7).GetChild(0).gameObject;
                }
                
                yield return StartCoroutine(LookForTarget());
                yield return StartCoroutine(Charge());
                yield return StartCoroutine(LaunchPoison());
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator Charge()
    {
        _meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(2);
        for (float durationSoFar = 0; durationSoFar < 1.0f; durationSoFar += Time.deltaTime)
        {
            transform.Translate(0,0,20  * Time.deltaTime);
            yield return null;
        }
        _meshRenderer.material = bossMat;
        yield return new WaitForSeconds(1);
    }
    
    private IEnumerator LookForTarget()
    {
        for (float durationSoFar = 0; durationSoFar < 10.0f; durationSoFar += Time.deltaTime)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 40)
            {
                Vector3 targetDirection = new Vector3(_player.transform.position.x, transform.position.y,
                    _player.transform.position.z);
                transform.LookAt(targetDirection);
                Chase();
            }
            yield return null;
        }
    }

    private IEnumerator LaunchPoison()
    {
        Vector3 targetDirection = new Vector3(_player.transform.position.x, transform.position.y,
            _player.transform.position.z);
        transform.LookAt(targetDirection);
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), transform.rotation);
        yield return new WaitForSeconds(1);
    }

    private void Chase()
    {
       
        if (Vector3.Distance(_player.transform.position, transform.position) > 2)
        {
            transform.Translate(_enemyMove * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        for (int i = 10 - 1; i >= 0; i--)
        {
            if (health <= i * 100 * NewGamePlus.level)
            {
                bossHealthBars.transform.GetChild(i).gameObject.SetActive(false);
            }

            if (health <= 0)
            {
                isDead = true;
                Destroy(gameObject);
                _spawnManager.transform.GetChild(0).gameObject.SetActive(true);
                bossHealthBars.transform.parent.gameObject.SetActive(false);
            }
        }
    }
    private IEnumerator AttackInRange()
    {
        while (true)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 8)
            {
                _player.health -= 100;
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }
}
