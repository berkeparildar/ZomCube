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
    public bool isHealthKit;
    public static bool machineGunUnlock = false;
    public static bool shotGunUnlock = false;
    public static bool hasOneHealthKit = false;
    public static bool hasTwoHealthKit = false;
    public Sprite pistolIcon;
    public Sprite machineGunIcon;
    public Sprite shotgunIcon;
    private int _damage;
    private GameObject _barTiles;
    private Text[] _actionNumbers;
    private Text powerUpUI;
    private Player _player;
    private GameObject _healthKitIcon;
    private GameObject _healthKitIconTwo;

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        powerUpUI = GameObject.Find("power_up_UI").GetComponent<Text>();
        _barTiles = GameObject.Find("bar_tiles");
        _healthKitIcon = GameObject.Find("bar_tile_4").transform.GetChild(1).gameObject;
        _healthKitIconTwo = GameObject.Find("bar_tile_5").transform.GetChild(1).gameObject;
        _actionNumbers = _barTiles.GetComponentsInChildren<Text>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _bulletText = GameObject.Find("bullet_text").GetComponent<Text>();
        if (isPistol)
        {
            _damage = 35;
            bulletCount = 7;
            _bulletText.text = bulletCount.ToString();
        }
        else if (isShotgun)
        {
            _damage = 35;
            bulletCount = 5;
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
        else if (isHealthKit)
        {
            UseHealthKit();
        }
        SwitchWeapons();
    }

    private void FireSingleBullet()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && isPistol)
        {
            var position = transform.GetChild(0).GetChild(0).position;
            Instantiate(bullet, new Vector3(position.x, position.y, position.z),
                transform.parent.rotation);
            bulletCount--;
            _bulletText.text = bulletCount.ToString();
        }
    }
    
    private void UseHealthKit()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && isHealthKit)
        {
            _player.ActivateHealthPowerUp();
            if (hasTwoHealthKit)
            {
                _healthKitIconTwo.SetActive(false);
                hasTwoHealthKit = false;
                transform.parent.GetChild(0).gameObject.SetActive(true);
                foreach (var number in _actionNumbers)
                {
                    number.color = Color.white;
                }
                _actionNumbers[0].color = Color.yellow;
                gameObject.SetActive(false);
            }
            else if (hasOneHealthKit)
            {
                _healthKitIcon.SetActive(false);
                hasOneHealthKit = false;
                transform.parent.GetChild(0).gameObject.SetActive(true);
                foreach (var number in _actionNumbers)
                {
                    number.color = Color.white;
                }
                _actionNumbers[0].color = Color.yellow;
                gameObject.SetActive(false);
            }
        }
    }

    private void FireShotgunShell()
    {
        if (shotGunUnlock && Input.GetKeyDown(KeyCode.Mouse1))
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
        powerUpUI.color = new Color(148, 0, 255);
        powerUpUI.text = "Damage UP!";
        yield return new WaitForSeconds(5);
        _damage -= 20;
        powerUpUI.text = "";
    }

    public void ActivateDamagePowerUp()
    {
        StartCoroutine(DamagePowerUp());
    }

    private void OnEnable()
    {
        _bulletText = GameObject.Find("bullet_text").GetComponent<Text>();
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
            if (isMachineGun || isShotgun || isHealthKit)
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
            if (isPistol || isShotgun ||isHealthKit)
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
            if (isPistol || isMachineGun || isHealthKit)
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
        else if (Input.GetKeyDown(KeyCode.Alpha4) && hasOneHealthKit)
        {
            if (isPistol || isMachineGun || isShotgun)
            {
                transform.parent.GetChild(3).gameObject.SetActive(true);
                foreach (var number in _actionNumbers)
                {
                    number.color = Color.white;
                }
                _actionNumbers[3].color = Color.yellow;
                gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && hasTwoHealthKit)
        {
            if (isPistol || isMachineGun || isShotgun)
            {
                transform.parent.GetChild(4).gameObject.SetActive(true);
                foreach (var number in _actionNumbers)
                {
                    number.color = Color.white;
                }
                _actionNumbers[4].color = Color.yellow;
                gameObject.SetActive(false);
            }
        }
    }
}