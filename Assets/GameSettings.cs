using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;


    public static float mouseSensitivity = 2.5f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this); }
    }

    public void ChangeSensitivity(float val)
    {
        mouseSensitivity = Mathf.Clamp(val, 0.5f, 5f);

    }
}
