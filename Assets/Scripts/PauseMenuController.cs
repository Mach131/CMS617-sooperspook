using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour, IPausable
{
    private static string mainMenuSceneName = "MainMenu";

    public Canvas canvas;
    public GraphicRaycaster graphicRaycaster;

    // Start is called before the first frame update
    void Start()
    {
		// Register this object for pause/unpause updates
		SceneDirector.GetPrimary().Register(this);
	}

	public void OnResumeButtonPress() {
		SceneDirector.GetPrimary().Resume();
	}

    // Transitions to the main menu scene.
    public void OnQuitToMainButtonPress()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OnQuitToDeskButtonPress() {
        Debug.Log("Quitting to Desktop");
		Application.Quit();
	}

    private void Hide()
    {
        canvas.enabled = false;
        graphicRaycaster.enabled = false;
    }

    private void Reveal()
    {
        canvas.enabled = true;
        graphicRaycaster.enabled = true;
    }
	
	public void OnPause() {
        Reveal();
    }

	public void OnResume() {
        Hide();
    }
}
