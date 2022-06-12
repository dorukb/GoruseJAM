using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;
    public float maxDown = 90f;
    public float maxUp = -45f;


    float xRotation = 0.0f;
    float yRotation = 0.0f;
    private Camera cam;
    public PlayerMovement Player;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = Camera.main;
    }

    private void Update()
    {
        if (!Player.CanMove()) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * horizontalSpeed;
        float mouseY = Input.GetAxisRaw("Mouse Y") * verticalSpeed;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxDown, maxUp);

        cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
        Player.transform.eulerAngles = new Vector3(0, yRotation, 0);
        //transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //orientiation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
