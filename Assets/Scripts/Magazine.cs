using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : MonoBehaviour
{
    private GameObject player;
    private Weapon[] weaponList;
    public Text bulletText;
    private Weapon _weapon;
    
    
    // Start is called before the first frame update
    void Start()
    {
        bulletText = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Text>();
        player = GameObject.Find("Player");
        weaponList = player.transform.GetChild(0).GetComponentsInChildren<Weapon>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerRange();
    }

    private void CheckPlayerRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 3)
        {
            for (int i = 0; i < weaponList.Length; i++)
            {
                if (weaponList[i].gameObject.activeSelf)
                {
                    _weapon = weaponList[i];
                }
            }

            if (_weapon.isPistol)
            {
                _weapon._bulletCount = 7;
                bulletText.text = 7.ToString();
                Destroy(gameObject);
            }
            else if (_weapon.isMachineGun)
            {
                _weapon._bulletCount = 30;
                bulletText.text = 30.ToString();
                Destroy(gameObject);
            }
            else if (_weapon.isShotgun)
            {
                _weapon._bulletCount = 5;
                bulletText.text = 5.ToString();
                Destroy(gameObject);
            }
        }
    }
}
