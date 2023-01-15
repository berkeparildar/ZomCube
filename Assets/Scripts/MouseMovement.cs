using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private float rotY;
    private float rotX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MouseMove();
    }

    void MouseMove()
    {
        var rotation = transform.rotation.eulerAngles;
        var mouseY = -Input.GetAxis("Mouse Y");
        var mouseX = Input.GetAxis("Mouse X");
        rotY = rotation.y;
        rotX = rotation.x;
        rotY += mouseX * Time.deltaTime * 200;
        rotX += mouseY * Time.deltaTime * 200;
        var localRotation = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = localRotation;
    }
}
