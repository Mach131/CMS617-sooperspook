using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public static List<CameraTarget> targets = new List<CameraTarget>();

    public float weight = 1f;

    void OnEnable()
    {
        targets.Add(this);
    }

    void OnDisable()
    {
        targets.Remove(this);
    }
}
