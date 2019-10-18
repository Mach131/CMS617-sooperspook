using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FearSource : MonoBehaviour
{
    public float fearWeight = 1;
    public bool isJumpScare = false;
    public abstract void TriggerEffect(Interactable source);
}
