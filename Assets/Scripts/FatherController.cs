using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherController : MonoBehaviour
{
    private Animator animator;
    private Trashcan trashcan;
    private Fort fort;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        trashcan = FindObjectOfType<Trashcan>();
        fort = FindObjectOfType<Fort>();
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
}
