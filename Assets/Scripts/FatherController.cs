using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;

public class FatherController : MonoBehaviour
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

    private Animator animator;
    private Trashcan trashcan;
    private Fort fort;
    private Bookshelf bookshelf;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        trashcan = FindObjectOfType<Trashcan>();
        fort = FindObjectOfType<Fort>();
        bookshelf = FindObjectOfType<Bookshelf>();
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
        endingCutscene.SetActive(true);
    }
}
