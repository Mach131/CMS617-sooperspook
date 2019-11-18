using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : Clickable
{
    private Animator animator;
    private bool hasToolbox = true;

    new protected void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    new protected void Update()
    {
        base.Update();
    }

    void Shake()
    {
        if (hasToolbox)
        {
            FindObjectOfType<FatherController>().NoticeToolbox();
        }
        animator.SetTrigger("Shake");
    }

    public void RemoveBox()
    {
        animator.SetTrigger("RemoveBox");
        hasToolbox = false;
    }

    protected override void OnClick()
    {
        Shake();
    }
}
