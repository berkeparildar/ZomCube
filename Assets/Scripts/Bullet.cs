using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 35;
    private float _distance;
    private Vector3 _currentPosition;
    private Weapon[] _weaponList;
    private Weapon _weapon;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.Find("Player");
        _weaponList = _player.transform.GetChild(0).GetComponentsInChildren<Weapon>(includeInactive: true);
        for (int i = 0; i < _weaponList.Length; i++)
        {
            if (_weaponList[i].gameObject.activeSelf)
            {
                _weapon = _weaponList[i];
            }
        }
        _currentPosition = transform.position;
    }

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
        transform.Translate(new Vector3(0, 0, 1) * (bulletSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag($"Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(_weapon.Damage);
            Destroy(gameObject);
        }
    }
}
