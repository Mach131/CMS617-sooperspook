using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
	public enum BroadcastType { Pause, Resume };

	// Return the callback appropriate for the input broadcast type
	private static Action GetCallback(BroadcastType bt, IPausable pausable ) {

		// fancy C# switch expression - shorthand for good ol' switch-case block
		switch(bt) {
		case BroadcastType.Pause:
			return pausable.OnPause;

		case BroadcastType.Resume:
			return pausable.OnResume;

		default:
			throw new Exception(bt.ToString());
		}
	}

	// Dictionary of all broadcast types to all items that have a callback
	// matching said type
	private Dictionary<BroadcastType, HashSet<IPausable>> pausableDict 
		= new Dictionary<BroadcastType, HashSet<IPausable>>();

	// Convenience method to get the primary scene director, since there's
	// likely only one (Non-enforced singleton)
	public static SceneDirector GetPrimary() {
		if( primaryInstance == null )
			throw new System.Exception("Primary not instantiated");
		return primaryInstance;
	}

	// Handle for the primary scene director, since there's likely only one 
	// (Non-enforced singleton)
	private static SceneDirector primaryInstance;

	// True if the game is paused; false otherwise
	public Boolean paused = false;

    // Start is called before the first frame update
    void Awake()
    {
		if( primaryInstance == null )
			primaryInstance = this;
		else
			Debug.LogWarning("Primary instance already assigned. Something probably went wrong");

		// Create a hashset for each type of broadcast
		// so fugly
		foreach( BroadcastType bt in BroadcastType.GetValues(typeof(BroadcastType)) ) {
			pausableDict.Add(bt, new HashSet<IPausable>());
		}

	}

	private void Update() {
		if( Input.GetKeyDown(KeyCode.Escape) )
			TogglePause();
	}

	// Register the input pausable object to have its OnPause/OnResume methods called
	public void Register(IPausable pausable) {
		// Register the input pausable to be executed for every pausble event
		foreach( BroadcastType bt in BroadcastType.GetValues(typeof(BroadcastType)) ) {
			pausableDict[bt].Add(pausable);
		}
	}

	// Pause if unpaused; unpause if paused
	public void TogglePause() {
		if( paused ) {
			Resume();
		} else {
			Pause();
		}
	}

	public void Pause() {
		paused = true;
		Broadcast(BroadcastType.Pause);
	}

	public void Resume() {
		paused = false;
		Broadcast(BroadcastType.Resume);
	}

	// Execute all registered callbacks for the input broadcast type
	private void Broadcast(BroadcastType bt) {
		foreach( IPausable pausable in pausableDict[bt]) {
			GetCallback(bt, pausable)();
		}
	}
}
