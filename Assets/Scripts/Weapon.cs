using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject _bullet;

    public bool isPistol;

    public bool isMachineGun;

    public bool isShotgun;

    private int damage;

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isPistol)
        {
            damage = 15;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPistol)
        {
            FireSingleBullet();
        }
    }

    private void FireSingleBullet()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var position = transform.position;
            Instantiate(_bullet, new Vector3(position.x, position.y, position.z),
                transform.parent.rotation);
        }
    }
    
    private IEnumerator DamagePowerUp()
    {
        damage += 20;
        yield return new WaitForSeconds(5);
        damage -= 20;
    }

    public void ActivateDamagePowerUp()
    {
        StartCoroutine(DamagePowerUp());
    }
}