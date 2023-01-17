using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private CharacterController _charController;
    private bool _isJumping;
    private Vector3 _currentJumpVelocity;
    public float health = 100;
    public float speed = 4;
    private Text healthText;
    

    // Start is called before the first frame update
    void Start()
    {
        healthText = GameObject.Find("health_Text").GetComponent<Text>();
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Enemy")
        {
            Debug.Log(health);
        }
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
        speed = 8;
        yield return new WaitForSeconds(5);
        speed = 4;
    }

    public void ActivateSpeedPowerUp()
    {
        StartCoroutine(SpeedPowerUp());
    }

    public void ActivateHealthPowerUp()
    {
        if (health <= 80)
        {
            health += 20;
        }
        else
        {
            health = 100;
        }
    }

    public void TakeDamage()
    {
        health -= 20;
        healthText.text = health.ToString();
    }
    
}
