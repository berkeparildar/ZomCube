using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Powerup : MonoBehaviour
{

    public Material healthMat;
    public Material damageMat;
    public Material speedMat;
    private int random;
    private Weapon _weapon;
    private List<Material> matList;
    // Start is called before the first frame update
    void Start()
    {
        _weapon = GameObject.Find("Weapon").GetComponent<Weapon>();
        matList = new List<Material>();
        matList.Add(healthMat); // 0
        matList.Add(speedMat); // 1
        matList.Add(damageMat); // 2
        random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                gameObject.GetComponent<MeshRenderer>().material = healthMat;
                break;
            case 1:
                gameObject.GetComponent<MeshRenderer>().material = speedMat;
                break;
            case 2:
                gameObject.GetComponent<MeshRenderer>().material = damageMat;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            switch (random)
            {
                case 0:
                    other.gameObject.GetComponent<Player>().ActivateHealthPowerUp();
                    SpawnManager.powerUpCount--;
                    Destroy(gameObject);
                    break;
                case 1:
                    other.gameObject.GetComponent<Player>().ActivateSpeedPowerUp();
                    SpawnManager.powerUpCount--;
                    Destroy(gameObject);
                    break;
                case 2:
                    Debug.Log("case 2");
                   _weapon.ActivateDamagePowerUp();
                    SpawnManager.powerUpCount--;
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
