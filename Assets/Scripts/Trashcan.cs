using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;

public class Trashcan : Clickable, IPausable
{
    public Animator orangePeel;
    public bool canSlip = false;
    public StudioEventEmitter fallingSFX;
    public StudioEventEmitter shakingSFX;

    private bool isUpright = true;
    private Animator animator;

    private const float full_playback_speed = 1.0f;

    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        // Register this object for pause/unpause updates
        SceneDirector.GetPrimary().Register(this);
    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();
    }

    void Shake()
    {
        animator.SetTrigger("Shake");
        shakingSFX.Play();
    }

    void KnockOver()
    {
        animator.SetTrigger("KnockOver");
        orangePeel.SetTrigger("FallOut");
    }

    public void PlayFallSFX()
    {
        fallingSFX.Play();
    }

    void EnableSlip()
    {
        canSlip = true;
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
