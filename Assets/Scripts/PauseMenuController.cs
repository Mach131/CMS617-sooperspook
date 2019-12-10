using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour, IPausable
{
    private static string gameSceneName = "MainScene";

    [SerializeField] private Canvas canvas;
    [SerializeField] private GraphicRaycaster graphicRaycaster;

    void Awake()
    {
        //canvas = GetComponent<Canvas>();
        //graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    // Start is called before the first frame update
    void Start()
    {
		// Register this object for pause/unpause updates
		SceneDirector.GetPrimary().Register(this);
	}

    // Transitions to the main game scene.
    public void OnStartButtonPress()
    {
        SceneManager.LoadScene(gameSceneName);
    }

	public void OnResumeButtonPress() {
        Debug.Log("Resume");
		SceneDirector.GetPrimary().Resume();
	}

	public void OnQuitToDeskButtonPress() {
		Application.Quit();
	}

    private void Hide()
    {
        canvas.enabled = false;
        graphicRaycaster.enabled = false;
        //canvas.gameObject.SetActive(false);
        //graphicRaycaster.gameObject.SetActive(false);
    }

    private void Reveal()
    {
        canvas.enabled = true;
        graphicRaycaster.enabled = true;
        //canvas.gameObject.SetActive(true);
        //graphicRaycaster.gameObject.SetActive(true);
    }
	
	public void OnPause() {
        Reveal();
    }

	public void OnResume() {
        Hide();
    }
}
