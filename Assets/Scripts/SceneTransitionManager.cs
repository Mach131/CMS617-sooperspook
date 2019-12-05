using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public string targetScene;
    public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));

    private bool isFading;
    private bool fadeIn;
    private bool fadeOut { get { return !fadeIn; } set { fadeIn = !value; } }
    private float fadeAlpha = 1;
    private float fadeTime;
    private Texture2D fadeTexture;
    private bool transitionWhenDoneFading;

    // Start is called before the first frame update
    void Start()
    {
        isFading = true;
        fadeIn = true;
        fadeAlpha = 1;
        fadeTime = 0;
        transitionWhenDoneFading = false;
    }

    public void transitionToScene()
    {
        isFading = true;
        fadeOut = true;
        fadeAlpha = 0;
        fadeTime = 1;
        transitionWhenDoneFading = true;
    }

    // adapted from https://forum.unity.com/threads/free-basic-camera-fade-in-script.509423/
    public void OnGUI()
    {
        if (!isFading)
        {
            return;
        }
        if (fadeTexture == null)
        {
            fadeTexture = new Texture2D(1, 1);
        }

        fadeTexture.SetPixel(0, 0, new Color(0, 0, 0, fadeAlpha));
        fadeTexture.Apply();

        fadeTime += fadeIn ? Time.deltaTime : -Time.deltaTime;
        fadeAlpha = fadeCurve.Evaluate(fadeTime);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);

        if ((fadeIn && fadeAlpha <= 0) || (fadeOut && fadeAlpha >= 1))
        {
            if (transitionWhenDoneFading)
            {
                fadeAlpha = 1;
                changeScenes();
            } else
            {
                isFading = false;
            }
        }
    }

    private void changeScenes()
    {
        SceneManager.LoadScene(targetScene);
    }
}
