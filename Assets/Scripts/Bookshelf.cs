using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;

public class Bookshelf : Clickable, IPausable
{
    public Musicbox musicbox;

    public StudioEventEmitter shakingSFX;
    public StudioEventEmitter musicboxRollingSFX;
    public StudioEventEmitter musicboxFallingSFX;

    private Animator animator;
    private bool hasToolbox = true;

    private const float full_playback_speed = 1.0f;

    new protected void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        // Register this object for pause/unpause updates
        SceneDirector.GetPrimary().Register(this);
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

    // Freeze all animations in-place when paused
    public void OnPause()
    {
        animator.speed = 0.0f;
    }

    // Resume all animations from where they were on resume
    public void OnResume()
    {
        animator.speed = full_playback_speed;
    }
}
