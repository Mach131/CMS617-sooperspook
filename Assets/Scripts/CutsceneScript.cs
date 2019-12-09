using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CutsceneScript : MonoBehaviour, IPausable
{
    public bool endingCutscene = false;
	private PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
		director = GetComponent<PlayableDirector>();
		SceneDirector.GetPrimary().Register(this);
        if (endingCutscene)
        {
            FindObjectOfType<PlayerMovement>().DisableMovement();
        }
    }

	public void StartFresh() {
		director.time = 0;
		director.Play();
	}

	// Freeze all animations in-place when paused
	public void OnPause() {
		try {
			director.playableGraph.GetRootPlayable(0).SetSpeed(0);
		} catch( System.NullReferenceException ) {
			// do nothing - null expected when cutscene not in progress
		}
	}

	// Resume all animations from where they were on resume
	public void OnResume() {
		try { 
			director.playableGraph.GetRootPlayable(0).SetSpeed(1);
		} catch(System.NullReferenceException ) {
			// do nothing - null expected when cutscene not in progress
		}
	}
}
