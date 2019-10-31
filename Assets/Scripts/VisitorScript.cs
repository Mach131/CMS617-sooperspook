using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorScript : MonoBehaviour
{
	public enum Motif {
		Modern
	}

	/// <summary>
	/// Motif the visitor is interested in.
	/// </summary>
	public Motif interestMotif = Motif.Modern;

	/// <summary>
	/// Motif the visitor is most spooked by.
	/// </summary>
	public Motif spookyMotif = Motif.Modern;

	/// <summary>
	/// Spatial mapping of spookiness.
	/// </summary>
	public SpookMap SpookMap { get; private set; }

	/// <summary>
	/// Floating indicator of how spooky the current location is.
	/// </summary>
	public SpookOMeter SpookOMeter { get; private set; }

	private void Awake() {
		SpookMap = GetComponent<SpookMap>();
		SpookOMeter = GetComponentInChildren<SpookOMeter>();

		SpookOMeter.ColorFunc = self =>
		{
			float value = self.Value;
			float normed = Mathf.Clamp01(self.Value / self.NormingValue);
			return Color.Lerp(Color.blue, Color.red, normed);
		};
	}

	private void FixedUpdate() {
		// push updates to the spook-o-meter
		// Allows the meter itself to be completely dumb.
		//SpookOMeter.NormingValue = SpookMap.Max();
		SpookOMeter.NormingValue = 20.0f;
		SpookOMeter.Value = LocalSpook();
	}

	/// <summary>
	/// Spookiness spatial gradient at the visitor's position.
	/// </summary>
	/// <remarks>
	/// This is the full 3D gradient of spookiness in space. Thus, the
	/// direction spookiness increases the most is aligned with the resulting
	/// vector.
	/// </remarks>
	/// <returns>
	/// The vector resulting from applying the gradient operator to the
	/// spookiness spatial map at the visitor's current position.
	/// </returns>
	public Vector3 LocalGradient() {
		return SpookMap.Gradient3(transform.position);
	}

	/// <summary>
	/// Spookiness value at the visitor's position.
	/// </summary>
	/// <returns>
	/// Spookiness value at the visitor's position, where higher values are
	/// more spooky.
	/// </returns>
	public float LocalSpook() {
		return SpookMap.ValueAt(transform.position);
	}

}
