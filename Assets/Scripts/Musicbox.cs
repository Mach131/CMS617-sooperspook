using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musicbox : Clickable
{
    public bool hasFallen = false;

    new protected void Start()
    {
        base.Start();
    }

    new protected void Update()
    {
        base.Update();
    }

    protected override void OnClick()
    {
        if (hasFallen)
        {
            FindObjectOfType<FatherController>().NoticeMusicbox();
        }
    }
}
