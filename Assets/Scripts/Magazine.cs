using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : MonoBehaviour
{
    private GameObject _player;
    private Weapon[] _weaponList;
    public Text bulletText;
    private Weapon _weapon;
    
    
    // Start is called before the first frame update
    void Start()
    {
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
    }
}
