using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public int bulletCount;
    private static Text _bulletText;
    public bool isPistol;
    public bool isMachineGun;
    public bool isShotgun;
    public static bool machineGunUnlock = false;
    public static bool shotGunUnlock = false;
    public Sprite pistolIcon;
    public Sprite machineGunIcon;
    public Sprite shotgunIcon;
    private int _damage;
    private GameObject _barTiles;
    private Text[] _actionNumbers;

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _barTiles = GameObject.Find("bar_tiles");
        _actionNumbers = _barTiles.GetComponentsInChildren<Text>();
        _bulletText = GameObject.Find("bullet_text").GetComponent<Text>();
        if (isPistol)
        {
            _damage = 35;
            bulletCount = 7;
            _bulletText.text = bulletCount.ToString();
        }
        else if (isShotgun)
        {
            _damage = 25;
            bulletCount = 25;
            _bulletText.text = bulletCount.ToString();
        }
        else if (isMachineGun)
        {
            _damage = 25;
            bulletCount = 30;
            _bulletText.text = bulletCount.ToString();
        }
        StartCoroutine(FireAutomatic());
    }

    // Update is called once per frame
    void Update()
    {
        if (isPistol)
        {
            if (bulletCount > 0)
            {
                FireSingleBullet();
            }
        }
        else if (isShotgun)
        {
            if (bulletCount > 0)
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
            var position = transform.GetChild(0).GetChild(0).position;
            Instantiate(bullet, new Vector3(position.x, position.y, position.z),
                transform.parent.rotation);
            bulletCount--;
            _bulletText.text = bulletCount.ToString();
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
            var position = transform.GetChild(0).GetChild(0).position;
            Instantiate(bullet, new Vector3(position.x, position.y, position.z),
                rot_1);
            Instantiate(bullet, new Vector3(position.x, position.y, position.z),
                transform.parent.rotation);
            Instantiate(bullet, new Vector3(position.x, position.y, position.z),
                rot_2);
            Instantiate(bullet, new Vector3(position.x, position.y, position.z),
                rot_3);
            bulletCount--;
            _bulletText.text = bulletCount.ToString();
        }
    }

    private IEnumerator FireAutomatic()
    {
        while (true)
        {
            if (machineGunUnlock && isMachineGun && bulletCount > 0 && Input.GetKey(KeyCode.Mouse1))
            {
                var position = transform.GetChild(0).GetChild(0).position;
                    Instantiate(bullet, new Vector3(position.x, position.y, position.z),
                        transform.parent.rotation);
                    bulletCount--;
                    _bulletText.text = bulletCount.ToString();
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
        _damage += 20;
        yield return new WaitForSeconds(5);
        _damage -= 20;
    }

    public void ActivateDamagePowerUp()
    {
        StartCoroutine(DamagePowerUp());
    }

    private void OnEnable()
    {
        if (isMachineGun)
        {
            _bulletText.text = bulletCount.ToString();
            StartCoroutine(FireAutomatic());
        }
        else if (isPistol)
        {
            _bulletText.text = bulletCount.ToString();

        }
        else if (isShotgun)
        {
            _bulletText.text = bulletCount.ToString();
        }
    }

    private void SwitchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isMachineGun || isShotgun)
            {
                transform.parent.GetChild(0).gameObject.SetActive(true);
                foreach (var number in _actionNumbers)
                {
                    number.color = Color.white;
                }
                _actionNumbers[0].color = Color.yellow;
                gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && machineGunUnlock)
        {
            if (isPistol || isShotgun)
            {
                transform.parent.GetChild(1).gameObject.SetActive(true);
                foreach (var number in _actionNumbers)
                {
                    number.color = Color.white;
                }
                _actionNumbers[1].color = Color.yellow;
                gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && shotGunUnlock)
        {
            if (isPistol || isMachineGun)
            {
                transform.parent.GetChild(2).gameObject.SetActive(true);
                foreach (var number in _actionNumbers)
                {
                    number.color = Color.white;
                }
                _actionNumbers[2].color = Color.yellow;
                gameObject.SetActive(false);
            }
        }
    }
}