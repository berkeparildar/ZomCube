using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private bool hasTarget;
    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log("Player nearby");
            Chase();

            if (Vector3.Distance(startingPos, transform.position) > 30)
            {
                transform.position = startingPos;
            }
        }
        else
        {
            transform.position = startingPos;
        }
    }

    private void Chase()
    {
        transform.Translate(new Vector3(0,0,1) * Time.deltaTime);
    }
    
    
}
