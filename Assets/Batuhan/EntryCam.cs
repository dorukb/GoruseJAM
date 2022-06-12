using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EntryCam : MonoBehaviour
{
    [SerializeField] private Camera mainCam;

    private void Start()
    {
        transform.DOMove(mainCam.transform.position, 5f);
        transform.DORotate(mainCam.transform.rotation.eulerAngles, 5f);
        StartCoroutine(ccc());
    }

    private IEnumerator ccc()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
