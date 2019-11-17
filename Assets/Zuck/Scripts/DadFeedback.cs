using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadFeedback : MonoBehaviour
{
    
    public GameObject exclamation;
    public GameObject question;
    public GameObject dotdotdot;

    void Start()
    {
        // To start, turn off all feedback
        exclamation.SetActive(false);
        question.SetActive(false);
        dotdotdot.SetActive(false);
    }

    // Flashes exclamation momentary
    public IEnumerator exclame() 
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(3);
        exclamation.SetActive(false);
        yield return null;
    }

    public void curious() 
    {
        question.SetActive(true);        
    }

    public void endCurious()
    {
        question.SetActive(false);
    }

    public IEnumerator ohno() 
    {
        dotdotdot.SetActive(true);
        yield return new WaitForSeconds(3);
        dotdotdot.SetActive(false);
        yield return null;
    }
}