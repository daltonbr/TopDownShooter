using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakePoint {
	[MenuItem("Assets/Create/My Point")]
	public static void CreatePoint()
	{
		Point pointAsset = ScriptableObject.CreateInstance<Point>();

		AssetDatabase.CreateAsset(pointAsset, "Assets/Point.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = pointAsset;
	}
}