using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookOMeter : MonoBehaviour
{

	public float Value { get; set; }
	public float NormingValue { get; set; }

	public Func<SpookOMeter, Color> ColorFunc { get; set; }

	private SpriteRenderer spriteRend;

    void Awake()
    {
		spriteRend = GetComponent<SpriteRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
		spriteRend.color = ColorFunc(this);
    }
}
