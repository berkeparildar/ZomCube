using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private bool _hasTarget;
    private Vector3 _startingPos;
    private int _rangeValue = 10;
    public int health = 100 * NewGamePlus.level;
    private bool _isInChallenge;
    private GameObject _startMenu;
    private ChallengeMode _challengeMode;
    public AudioClip deathSound;
    private AudioSource _audioSource;
    public AudioClip attackSound;
    private bool _playOnceAttack = true;
    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;

    // Start is called before the first frame update
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _startMenu = GameObject.Find("Canvas");
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _startingPos = transform.position;
        StartCoroutine(Patrol());
        StartCoroutine(AttackInRange());
        _challengeMode = GameObject.Find("challenge_start").GetComponent<ChallengeMode>();
        if (transform.position.x is > 67 and < 115 && transform.position.z is > 71 and < 111)
        {
            _isInChallenge = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInChallenge)
        {
            ChaseInChallenge();
        }
        else
        {
            LookForTarget();
        }
    }

    private IEnumerator AttackInRange()
    {
        while (true)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 2.5 && _player.isAlive)
            {
                _player.TakeDamage();
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }
    
    private void LookForTarget()
    {
        if (!SpawnManager.gameOver)
        {
            if (Vector3.Distance(_player.gameObject.transform.position, transform.position) < _rangeValue && _player.isAlive)
            {
                if (_playOnceAttack)
                {
                    _audioSource.PlayOneShot(attackSound);
                    _playOnceAttack = false;
                }
                _hasTarget = true;
                _rangeValue = 15;
                var position = _player.transform.position;
                Vector3 targetDirection = new Vector3(position.x, transform.position.y,
                    position.z);
                transform.LookAt(targetDirection);
                Chase();
            }
            else
            {
                if (_hasTarget) 
                {
                    _rangeValue = 10;
                    transform.position = Vector3.MoveTowards(transform.position, _startingPos, 4 * Time.deltaTime);
                    transform.LookAt(_startingPos);
                    if (_hasTarget && transform.position == _startingPos)
                    {
                        _hasTarget = false;
                    }
                }
            }
        }
    }
    
    private void ChaseInChallenge()
    {
        if (!SpawnManager.gameOver)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 50)
            {
                if (_playOnceAttack)
                {
                    _audioSource.PlayOneShot(attackSound);
                    _playOnceAttack = false;
                }

                var position = _player.transform.position;
                Vector3 targetDirection = new Vector3(position.x, transform.position.y,
                    position.z);
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
            if (Vector3.Distance(_player.transform.position, transform.position) > 2)
            {
                transform.Translate(enemyMove * Time.deltaTime);
            }
        }
    }

    private IEnumerator PlayDeathSound()
    {
        _audioSource.PlayOneShot(deathSound);
        _meshRenderer.enabled = false;
        _boxCollider.enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (_isInChallenge)
            {
                _challengeMode.IncreaseScore();
                StartCoroutine(PlayDeathSound());
            }
            else
            {
                SpawnManager.enemyCount--;
                StartCoroutine(PlayDeathSound());
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
            while (_hasTarget)
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
