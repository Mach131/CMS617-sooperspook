using UnityEditor;
using UnityEngine;

public class SpookSystem : EditorWindow
{
	bool visualizeSpook = false;

	private SpookViz spookViz;

	public static float HeatGridSize {
		get => ScalarMap.cellSize;
		set => ScalarMap.cellSize = value;
	}

	private bool SpookVizAvailable {
		get => spookViz != null;
	}

	// Add menu item named "My Window" to the Window menu
	[MenuItem("sooper spook/Spook System")]
	public static void ShowWindow() {
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(SpookSystem));
	}

	void OnGUI() {
		spookViz = FindObjectOfType<SpookViz>();
		Canvas canvas = null;
		if( SpookVizAvailable )
			canvas = spookViz.GetComponent<Canvas>();

		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		//myString = EditorGUILayout.TextField("Text Field", myString);

		//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		
		GUIContent sliderContent = new GUIContent("Spook Map Grid Size", 
			"Size of the spook and interest level grid discretization");
		HeatGridSize = EditorGUILayout.Slider(sliderContent, HeatGridSize, 1, 100);
		//EditorGUILayout.EndToggleGroup();

		SpookMap.bounds = SpookMapBoundsGUI(SpookMap.bounds);
		
		using( new EditorGUI.DisabledScope(SpookVizAvailable == false) ) {

			visualizeSpook = EditorGUILayout.Toggle(
				"Visualize Spook Heatmap", spookViz.enabled);
		}

		if( SpookVizAvailable ) {
			canvas.enabled = visualizeSpook;
			spookViz.enabled = visualizeSpook;
		}
	}

	private Rect SpookMapBoundsGUI(Rect bounds) {
		GUIContent label = new GUIContent("Spook Map Bounds",
			"Region of space covered by the spook map. No effect until scene reset");

		return EditorGUILayout.RectField(label, bounds);
	}

	
}
