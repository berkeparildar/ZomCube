using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 35;
    private float _distance;
    private Vector3 _currentPosition;
    private Weapon[] weaponList;
    private Weapon _weapon;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.Find("Player");
        weaponList = _player.transform.GetChild(0).GetComponentsInChildren<Weapon>();
        for (int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i].gameObject.activeSelf)
            {
                _weapon = weaponList[i];
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
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(_weapon.Damage);
            Destroy(gameObject);
        }
    }
}
