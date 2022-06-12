using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DepthGetter : MonoBehaviour
{
    private Camera cam;
    private void Start()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    
    }

    private void Update()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }
    
    
}
