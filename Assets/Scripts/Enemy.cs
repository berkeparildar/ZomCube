using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private bool hasTarget;
    private Vector3 startingPos;
    private CharacterController charController;
    private List<Vector3> randomMovement = new List<Vector3>() {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};


    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
     lookForTarget();   
    }

    private void lookForTarget()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 20)
        {
            transform.LookAt(player.transform.position);
            Chase();
            if (Vector3.Distance(startingPos, transform.position) > 30)
            {
                transform.position = Vector3.MoveTowards(transform.position, startingPos, 4 * Time.deltaTime);
            }
        }
        else
        {
            charController.SimpleMove(randomMovement[Random.Range(0, randomMovement.Count - 1)]);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            Attack();
        }
    }

    private void Chase()
    {
        transform.Translate(new Vector3(0,0,1) * Time.deltaTime);
    }

    private void Attack()
    {
        player.GetComponent<Player>().health -= 20;
    }

    private void WanderAround()
    {
        
    }
}
