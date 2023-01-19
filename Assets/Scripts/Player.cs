using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private CharacterController _charController;
    private bool _isJumping;
    private Vector3 _currentJumpVelocity;
    public float health = 100;
    private float speed = 8;
    private Text healthText;
    private ChallengeMode _challengeMode;
    public bool isAlive = true;
    private Text powerUpUI;
    public bool isPoisoned;
    private int poisonMultiplier = 0;
    private Boss boss;
    

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<Boss>();
        powerUpUI = GameObject.Find("power_up_UI").GetComponent<Text>();
        _challengeMode = GameObject.Find("challenge_start").GetComponent<ChallengeMode>();
        healthText = GameObject.Find("health_Text").GetComponent<Text>();
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        checkAlive();
    }

    private IEnumerator PoisonDamage()
    {
        while (true)
        {
            if (isPoisoned)
            {
                if (boss.isDead)
                {
                    isPoisoned = false;
                    powerUpUI.text = "";
                }
                health -= 5 * poisonMultiplier;
                powerUpUI.color = Color.red;
                powerUpUI.text = "Poisoned! x" + poisonMultiplier;
                Debug.Log(poisonMultiplier);
                healthText.text = health.ToString();
            }
            yield return new WaitForSeconds(5);
        }
    }

    public void TakePoisonDamage()
    {
        if (poisonMultiplier == 0)
        {
            StartCoroutine(PoisonDamage());
            isPoisoned = true;
        }
        poisonMultiplier++;
    }

    private void Movement()
    {
        var walkInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var currentSpeed = walkInput * speed;
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
        speed = 12;
        powerUpUI.color = Color.blue;
        powerUpUI.text = "Speed UP!";
        yield return new WaitForSeconds(5);
        speed = 8;
        powerUpUI.text = "";
    }
    
    private IEnumerator HealthPowerUp()
    {
        if (health <= 80)
        {
            health += 20;
            healthText.text = health.ToString();
        }
        else
        {
            health = 100;
            healthText.text = health.ToString();
        }
        powerUpUI.color = Color.green;
        powerUpUI.text = "Health UP!";
        yield return new WaitForSeconds(5);
        powerUpUI.text = "";
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
        health -= 20;
        healthText.text = health.ToString();
        checkAlive();
    }

    private void checkAlive()
    {
        if (health <= 0)
        {
            if (ChallengeMode.challengeStart)
            {
                _challengeMode.FinishChallenge();
                health = 100;
                healthText.text = health.ToString();
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
