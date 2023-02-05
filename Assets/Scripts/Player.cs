using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private CharacterController _charController;
    private bool _isJumping;
    private Vector3 _currentJumpVelocity;
    public float health = 100;
    private float _speed = 8;
    private Text _healthText;
    private ChallengeMode _challengeMode;
    public bool isAlive = true;
    private Text _powerUpUI;
    public bool isPoisoned;
    private int _poisonMultiplier = 0;
    private Boss _boss;
    private AudioSource _audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        _boss = GameObject.Find("Boss").GetComponent<Boss>();
        _audioSource = GetComponent<AudioSource>();
        _powerUpUI = GameObject.Find("power_up_UI").GetComponent<Text>();
        _challengeMode = GameObject.Find("challenge_start").GetComponent<ChallengeMode>();
        _healthText = GameObject.Find("health_Text").GetComponent<Text>();
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckAlive();
    }

    private IEnumerator PoisonDamage()
    {
        while (true)
        {
            if (isPoisoned)
            {
                if (_boss.isDead)
                {
                    isPoisoned = false;
                    _powerUpUI.text = "";
                }
                health -= 5 * _poisonMultiplier;
                _powerUpUI.color = Color.red;
                _powerUpUI.text = "Poisoned! x" + _poisonMultiplier;
                Debug.Log(_poisonMultiplier);
                _healthText.text = health.ToString();
            }
            yield return new WaitForSeconds(5);
        }
    }

    public void TakePoisonDamage()
    {
        if (_poisonMultiplier == 0)
        {
            StartCoroutine(PoisonDamage());
            isPoisoned = true;
        }
        _poisonMultiplier++;
    }

    private void Movement()
    {
        var walkInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var currentSpeed = walkInput * _speed;
        var moveVelocity = transform.TransformDirection(currentSpeed);
        if (Input.GetButtonDown("Jump") && _charController.isGrounded)
        {
            if (!_isJumping)
            {
                _isJumping = true;
                _currentJumpVelocity = new Vector3(0, 5, 0);
            }
        }

        if (_isJumping)
        {
            _charController.Move(new Vector3(moveVelocity.x, _currentJumpVelocity.y, moveVelocity.z) * Time.deltaTime);
            _currentJumpVelocity += Physics.gravity * Time.deltaTime;
            if (_charController.isGrounded)
            {
                _isJumping = false;
            }
        }
        else
        {
            _charController.SimpleMove(moveVelocity);
        }
    }
    
    private IEnumerator SpeedPowerUp()
    {
        _speed = 12;
        _powerUpUI.color = Color.blue;
        _powerUpUI.text = "Speed UP!";
        yield return new WaitForSeconds(5);
        _speed = 8;
        _powerUpUI.text = "";
    }
    
    private IEnumerator HealthPowerUp()
    {
        if (health <= 80)
        {
            health += 20;
            _healthText.text = health.ToString();
        }
        else
        {
            health = 100;
            _healthText.text = health.ToString();
        }
        _powerUpUI.color = Color.green;
        _powerUpUI.text = "Health UP!";
        yield return new WaitForSeconds(5);
        _powerUpUI.text = "";
    }

    public void ActivateSpeedPowerUp()
    {
        StartCoroutine(SpeedPowerUp());
    }

    public void ActivateHealthPowerUp()
    {
        StartCoroutine(HealthPowerUp());
    }

    public void TakeDamage()
    {
        _audioSource.Play();
        health -= 20;
        _healthText.text = health.ToString();
        CheckAlive();
    }

    private void CheckAlive()
    {
        if (health <= 0)
        {
            if (ChallengeMode.challengeStart)
            {
                _challengeMode.FinishChallenge();
                health = 100;
                _healthText.text = health.ToString();
            }
            else
            {
                isAlive = false;
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
