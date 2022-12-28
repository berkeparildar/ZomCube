using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _charController;
    private bool _isJumping;
    private Vector3 _currentJumpVelocity;
    private Camera _Camera;


    // Start is called before the first frame update
    void Start()
    {
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var walkInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var currentSpeed = walkInput * 4;
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
            _charController.Move((moveVelocity + _currentJumpVelocity) * Time.deltaTime);
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
}
