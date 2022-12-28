using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private float rotY;
    private float rotX;
    private float angleCap = 80;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseMove();
    }

    void MouseMove()
    {
        var rotation = transform.localRotation.eulerAngles;
        var mouseY = -Input.GetAxis("Mouse Y");
        var mouseX = Input.GetAxis("Mouse X");
        rotY = rotation.y;
        rotX = rotation.x;
        rotY += mouseX * Time.deltaTime * 100;
        rotX += mouseY * Time.deltaTime * 100;
        var localRotation = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = localRotation;
    }
}
