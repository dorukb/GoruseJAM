using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForTexture : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float planeY;
    [SerializeField] private GameObject ReflectCam;
    [SerializeField] private GameObject RefractCam;
    [SerializeField] private GameObject DepthCam;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        RefractCam.transform.position = cam.transform.position;
        RefractCam.transform.rotation = cam.transform.rotation;
        
        DepthCam.transform.position = cam.transform.position;
        DepthCam.transform.rotation = cam.transform.rotation;

        float dist = Math.Abs(cam.transform.position.y - planeY);
        ReflectCam.transform.position = cam.transform.position - Vector3.up * (2 * dist);
        
        var angles = cam.transform.rotation.eulerAngles;
        angles.x *= -1;
        ReflectCam.transform.rotation = Quaternion.Euler(angles);
    }
}
