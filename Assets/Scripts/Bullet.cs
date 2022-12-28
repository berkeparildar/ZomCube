using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 25;
    private float _distance;
    private Vector3 _currentPosition;
    private GameObject weapon;
    
    void Start()
    {
        _currentPosition = transform.position;
        weapon = GameObject.Find("Weapon");
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
            other.gameObject.GetComponent<Enemy>().TakeDamage(weapon.GetComponent<Weapon>().Damage);
            Destroy(gameObject);
        }
    }
}
