using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SpookViz : MonoBehaviour
{
	public static Sprite tileSprite;

	private static float CellSize {
		get => SpookMap.cellSize;
	}

	public VisitorScript subject = null;
	private GameObject[,] objectGrid;
	private VisitorScript oldSubject = null;
	private SpookMap spookMap;

	private bool HasSubject { get; set; }

	private bool SubjectChanged {
		get => subject != oldSubject;
	}

	private Vector2Int GridSize {
		get => spookMap.GridSize;
	}

	public bool RequestViz(SpookMap newMap) {
		// TODO
		// this should notify the old spookmap that it is no longer being
		// observed, so that map can de-select its observe boolean.
		return false;
	}

	private void FixedUpdate() {
		if( SubjectChanged )
			UpdateSubject(subject);

		if( !HasSubject )
			return;

		// Grid size here must always match the heatmap's size
		Assert.AreEqual(GridSize.x, objectGrid.GetLength(0));
		Assert.AreEqual(GridSize.y, objectGrid.GetLength(1));

		float max = spookMap.Max();

		foreach (Vector2Int cell in spookMap.CellEnumerable()) {
			float normed = Mathf.Clamp01(spookMap.ValueAt(cell) / max);
			Color c = Color.Lerp(Color.blue, Color.red, normed);
			objectGrid[cell.x, cell.y].GetComponent<Image>().color = c;
		}
	}

	private static GameObject[,] RegenObjectGrid( 
		SpookMap spookMap, Transform parent ) {

		GameObject[,] objectGrid = new GameObject[
			spookMap.GridSize.x, spookMap.GridSize.y];

		foreach( Vector2Int cell in spookMap.CellEnumerable() ) {
			Vector2 p = spookMap.CellCenter2(cell);
			objectGrid[cell.x, cell.y] = CreateCircle(p, parent);
		}

		return objectGrid;
	}

    private static GameObject CreateCircle(
		Vector2 anchoredPosition, Transform parent) {

		Vector2 center = new Vector2(0.5f, 0.5f);

		GameObject gameObject = new GameObject("Heatmap Cell");

		Image newImage = gameObject.AddComponent<Image>();
		newImage.sprite = tileSprite;
		newImage.color = Color.green;
		//gameObject.SetActive(true); //Activate the GameObject

		gameObject.transform.SetParent(parent, false);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(CellSize, CellSize);
        rectTransform.anchorMin = center;
        rectTransform.anchorMax = center;

		return gameObject;
    }

	private void UpdateSubject( VisitorScript newSubject ) {
		subject = newSubject;
		oldSubject = newSubject;

		if( subject != null ) {
			HasSubject = true;
			spookMap = subject.transform.GetComponentInParent<SpookMap>();
			objectGrid = RegenObjectGrid(spookMap, transform);

		} else {
			HasSubject = false;
			spookMap = null;
		}
	}
}
