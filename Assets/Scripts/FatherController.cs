using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherController : MonoBehaviour
{
    public GameObject toolbox;
    public Transform rightHand;

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
        if (!trashcan.IsUpright())
        {
            animator.SetBool("Slip", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slip"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.87)
            {
                fort.KnockOver();
            }
            trashcan.orangePeel.SetTrigger("Slip");
        }
    }

    public void NoticeToolbox()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crashed"))
        {
            animator.SetTrigger("NoticeToolbox");
        }
    }

    // this is called by the animation once the box has been grabbed
    public void GrabBox()
    {
        toolbox.transform.parent = rightHand;
        bookshelf.RemoveBox();
    }
}
