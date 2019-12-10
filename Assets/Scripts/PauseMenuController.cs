﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenuController : MonoBehaviour, IPausable
{
    private static string gameSceneName = "MainScene";
    private bool creditsPanelOpen;

    public Button[] mainMenuButtons;
    public GameObject creditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        creditsPanelOpen = false;

		// Register this object for pause/unpause updates
		SceneDirector.GetPrimary().Register(this);
	}

    // Transitions to the main game scene.
    public void OnStartButtonPress()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Opens the credits panel, if it is not already open.
    public void OnCreditsButtonPress()
    {
        if (!creditsPanelOpen)
        {
            // Disable main menu buttons so they can't be pressed anymore
            foreach (Button button in mainMenuButtons)
            {
                button.interactable = false;
            }
            // Bring up the panel
            creditsPanel.SetActive(true);

            creditsPanelOpen = true;
        }
    }

    public void OnCreditsReturnButtonPress()
    {
        if (creditsPanelOpen)
        {
            // Remove the panel
            creditsPanel.SetActive(false);
            // Re-enable main menu buttons
            foreach (Button button in mainMenuButtons)
            {
                button.interactable = true;
            }

            creditsPanelOpen = false;
        }
    }

	public void OnResumeButtonPress() {
		SceneDirector.GetPrimary().Resume();
	}

	public void OnQuitToDeskButtonPress() {
		Application.Quit();
	}

	
	public void OnPause() {
		
	}

	public void OnResume() {
		
	}
}
