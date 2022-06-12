using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LakeTriggerForCamera : MonoBehaviour
{
    [SerializeField] private Transform endCamPoint;
    [SerializeField] private Vector3 endCamRot;
    
    private Vector3 camPos;
    private Transform camera;
    private void OnTriggerEnter(Collider other)
    {
        var p = other.GetComponent<Movement>();
        if (p)
        {
            p.DisablePlayerMovement();
            camera = p.playerCamera;
            camPos = camera.position;
            StartCoroutine(CamRoutine());
        }
    }

    private IEnumerator CamRoutine()
    {
        camera.DOMove(endCamRot, .2f);
        yield break;
    }
}
