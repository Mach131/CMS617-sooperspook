using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;

public class Bookshelf : Clickable
{
    public Musicbox musicbox;

    public StudioEventEmitter shakingSFX;
    public StudioEventEmitter musicboxRollingSFX;
    public StudioEventEmitter musicboxFallingSFX;

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
        shakingSFX.Play();
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

    public void OnMusicboxFall()
    {
        musicbox.hasFallen = true;
    }

    public void PlayRollingSFX()
    {
        musicboxRollingSFX.Play();
    }

    public void PlayFallingSFX()
    {
        musicboxFallingSFX.Play();
    }
}
