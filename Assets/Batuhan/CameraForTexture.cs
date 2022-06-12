using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForTexture : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private GameObject plane;
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
        
    }
}
