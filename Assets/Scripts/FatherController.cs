using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;

public class FatherController : MonoBehaviour, IPausable
{
    public GameObject toolbox;
    public GameObject musicbox;
    public GameObject endingCutscene;
    public Transform rightHand;
    public Transform pickUpMusicboxTransform;
    public StudioEventEmitter walkingSFX;
    public StudioEventEmitter crashingSFX;
    public StudioEventEmitter confusedSFX;
    public StudioEventEmitter interestedSFX;
    public StudioEventEmitter shockedSFX;
    public StudioEventEmitter satisfiedSFX;
    public StudioEventEmitter grabObjectSFX;
    public StudioEventEmitter grabObjectTadaSFX;
    public StudioEventEmitter musicboxSFX;
    public StudioEventEmitter levelMusic;

    private Animator animator;
    private Trashcan trashcan;
    private Fort fort;
    private Bookshelf bookshelf;

    private const float full_playback_speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        trashcan = FindObjectOfType<Trashcan>();
        fort = FindObjectOfType<Fort>();
        bookshelf = FindObjectOfType<Bookshelf>();

        // Register this object for pause/unpause updates
        SceneDirector.GetPrimary().Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slip()
    {
        if (trashcan.canSlip)
        {
            animator.SetTrigger("Slip");
            trashcan.orangePeel.SetTrigger("Slip");
        }
    }

    public void KnockOverFort()
    {
        fort.KnockOver();
        levelMusic.SetParameter("Progress", 1);
    }

    public void NoticeToolbox()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crashed"))
        {
            animator.SetTrigger("NoticeToolbox");
        }
        interestedSFX.Play();
    }

    // this is called by the animation once the box has been grabbed
    public void GrabBox()
    {
        toolbox.transform.position = rightHand.position;
        toolbox.transform.parent = rightHand;
        bookshelf.RemoveBox();
        grabObjectTadaSFX.Play();
        levelMusic.SetParameter("Progress", 2);
    }

    public void NoticeMusicbox()
    {
        animator.SetTrigger("NoticeMusicbox");
        interestedSFX.Play();
    }

    // this is called by the animation once the musicbox has been grabbed
    public void GrabMusicbox()
    {
        musicbox.transform.position = rightHand.position;
        musicbox.transform.parent = rightHand;
        grabObjectTadaSFX.Play();
        //musicboxSFX.Play();
        levelMusic.SetParameter("Progress", 3);
        endingCutscene.SetActive(true);
    }

    public void PlayShockedSFX()
    {
        //shockedSFX.Play();
    }

    public void StartPacing()
    {
        animator.SetTrigger("StartPacing");
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
