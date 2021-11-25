using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Kibum Park
/// @version 2.0
/// Script that allows the user to view the boxes and the area with the mouse and keyboard.
/// WASD is used to move the camera on the XYZ axis.
/// The mouse allows for camera rotataion.
/// </summary>
    public class CameraScript : MonoBehaviour {
    public float turnSpeed = 4.0f;
    public float moveSpeed = 6.0f;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;
    private float rotX;
    //Update ensures that the camera script runs at all times.
    void Update ()
    {
        MouseAiming();
        KeyboardMovement();
    }
    //Allows the user to rotate the camera with mouse movement.
    void MouseAiming ()
    {
    // get the mouse inputs
        float y = Input.GetAxis("Mouse X") * turnSpeed;
        rotX += Input.GetAxis("Mouse Y") * turnSpeed;
        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
        // rotate the camera
        transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
    }
    //Allows the user to move the camera along the XYZ axis
    void KeyboardMovement ()
    {
        Vector3 dir = new Vector3(0, 0, 0);
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        transform.Translate(dir * moveSpeed * Time.deltaTime);
}
}

