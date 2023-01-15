using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public GameObject _bullet;
    public int _bulletCount;
    public Text bulletText;
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
            _bulletCount = 7;
            bulletText.text = _bulletCount.ToString();
        }
        else if (isShotgun)
        {
            damage = 25;
            _bulletCount = 5;
        }
        else if (isMachineGun)
        {
            damage = 30;
            _bulletCount = 30;
        }

        StartCoroutine(FireAutomatic());
    }

    // Update is called once per frame
    void Update()
    {
        if (isPistol)
        {
            if (_bulletCount > 0)
            {
                FireSingleBullet();
            }
        }
        else if (isShotgun)
        {
            if (_bulletCount > 0)
            {
                FireShotgunShell();
            }
        }
        SwitchWeapons();
    }

    private void FireSingleBullet()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var position = transform.position;
            Instantiate(_bullet, new Vector3(position.x, position.y, position.z),
                transform.parent.rotation);
            _bulletCount--;
            bulletText.text = _bulletCount.ToString();
        }
    }

    private void FireShotgunShell()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 r = transform.parent.rotation.eulerAngles;
            Quaternion rot_1 = Quaternion.Euler(r.x, r.y + 5, r.z);
            Quaternion rot_2 = Quaternion.Euler(r.x, r.y - 5, r.z);
            Quaternion rot_3 = Quaternion.Euler(r.x + 3, r.y, r.z);
            var position = transform.position;
            Instantiate(_bullet, new Vector3(position.x, position.y, position.z),
                rot_1);
            Instantiate(_bullet, new Vector3(position.x, position.y, position.z),
                transform.parent.rotation);
            Instantiate(_bullet, new Vector3(position.x, position.y, position.z),
                rot_2);
            Instantiate(_bullet, new Vector3(position.x, position.y, position.z),
                rot_3);
            _bulletCount--;
            bulletText.text = _bulletCount.ToString();
        }
    }

    private IEnumerator FireAutomatic()
    {
        while (true)
        {
            if (isMachineGun && _bulletCount > 0 && Input.GetKey(KeyCode.Mouse1))
            {
                var position = transform.position;
                    Instantiate(_bullet, new Vector3(position.x, position.y, position.z),
                        transform.parent.rotation);
                    _bulletCount--;
                    bulletText.text = _bulletCount.ToString();
                    yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return null;
            }
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
    
    private void SwitchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (isMachineGun || isShotgun)
            {
                if (isPistol)
                {
                    gameObject.SetActive(true);
                }
                gameObject.SetActive(false);
            }
        }
    }
}