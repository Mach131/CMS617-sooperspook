using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : Clickable
{
    public Animator orangePeel;

    private bool isUpright = true;
    private Animator animator;

    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();
    }

    void Shake()
    {
        animator.SetTrigger("Shake");
    }

    void KnockOver()
    {
        animator.SetTrigger("KnockOver");
        orangePeel.SetTrigger("FallOut");
    }

    protected override void OnClick()
    {
        if (isUpright)
        {
            KnockOver();
            isUpright = false;
        }
        else
        {
            Shake();
        }
    }

    public bool IsUpright()
    {
        return isUpright;
    }
}
