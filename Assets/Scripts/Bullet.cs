using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 25;
    
    private float _distance;
    private Vector3 _currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        _currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector3.Distance(transform.position, _currentPosition);
        if (_distance > 50)
        {
            Destroy(this.gameObject);
        }
        Movement();
    }

    private void Movement()
    {
        transform.Translate(new Vector3(0, 0, 1) * (bulletSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
