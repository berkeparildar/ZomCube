using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousBall : MonoBehaviour
{
    public float ballSpeed = 50 * NewGamePlus.level;
    private float _distance;
    private Vector3 _currentPosition;

    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector3.Distance(transform.position, _currentPosition);
        if (_distance > 50)
        {
            Destroy(gameObject);
        }
        Movement();
    }
    
    private void Movement()
    {
        transform.Translate(new Vector3(0, 0, 1) * (ballSpeed * Time.deltaTime));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            _player.TakePoisonDamage();
        }
    }
}
