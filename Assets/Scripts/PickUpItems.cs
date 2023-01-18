using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItems : MonoBehaviour
{
    private GameObject _player;
    private Weapon[] _weaponList;
    private Text bulletText;
    private Weapon _weapon;
    private GameObject _rifleIcon;
    private GameObject _shotgunIcon;
    public bool isMagazine;
    public bool isWeapon;


    // Start is called before the first frame update
    void Start()
    {
        _rifleIcon = GameObject.Find("bar_tile_2").transform.GetChild(1).gameObject;
        _shotgunIcon = GameObject.Find("bar_tile_2").transform.GetChild(1).gameObject;
        bulletText = GameObject.Find("bullet_text").GetComponent<Text>();
        _player = GameObject.Find("Player");
        _weaponList = _player.transform.GetChild(0).GetComponentsInChildren<Weapon>(includeInactive: true);
        Debug.Log(_weaponList.Length);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerRange();
    }

    private void CheckPlayerRange()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < 3)
        {
            if (isMagazine)
            {
                for (int i = 0; i < _weaponList.Length; i++)
                {
                    if (_weaponList[i].gameObject.activeInHierarchy)
                    {
                        _weapon = _weaponList[i];
                    }
                }

                if (_weapon.isPistol)
                {
                    _weapon.bulletCount = 7;
                    bulletText.text = 7.ToString();
                    Destroy(gameObject);
                }
                else if (_weapon.isMachineGun)
                {
                    _weapon.bulletCount = 30;
                    bulletText.text = 30.ToString();
                    Destroy(gameObject);
                }
                else if (_weapon.isShotgun)
                {
                    _weapon.bulletCount = 5;
                    bulletText.text = 5.ToString();
                    Destroy(gameObject);
                }
            }
            else if (isWeapon)
            {
                if (gameObject.name.Equals("machine_gun_model"))
                {
                    _rifleIcon.SetActive(true);
                    Weapon.machineGunUnlock = true;
                    Destroy(gameObject);
                }
            }
        }
    }
}