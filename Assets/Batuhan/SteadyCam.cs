using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteadyCam : MonoBehaviour
{
    public float amount = 10.0f;
    public float speed = 1.0f;
    private Vector3 lastPos;
    private float dist = 0.0f;
    private Vector3 rotation = Vector3.zero;

    void Start () {
        lastPos = transform.position;
    }
 
    void Update () {
        dist += (transform.position - lastPos).magnitude;
        lastPos = transform.position;
        rotation.z = Mathf.Sin (dist * speed) * amount;
        transform.localEulerAngles = rotation;
    }
}
