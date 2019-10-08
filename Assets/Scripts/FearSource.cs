using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FearSource : MonoBehaviour
{
    public float fearWeight = 1;
    public abstract void TriggerEffect();
}
