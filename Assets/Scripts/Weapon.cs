using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        Instantiate(_bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z),
            transform.parent.rotation);
    }
}