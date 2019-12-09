using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;

public class Fort : MonoBehaviour
{
    public StudioEventEmitter breakingSFX;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void KnockOver()
    {
        animator.SetTrigger("Fall");
        breakingSFX.Play();
    }

    public void Fix()
    {
        animator.SetTrigger("Fix");
    }
}
