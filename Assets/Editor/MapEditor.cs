using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Always updating ... cpu intensive sometimes
        //MapGenerator map = target as MapGenerator;
        //map.GenerateMap();

        // just update when some value is changed in the inspector (much more effective)
        if (GUI.changed)
        {
            //Debug.Log("GUI changed");
            MapGenerator map = target as MapGenerator;
            map.GenerateMap();
        }

        // or update just when we press this buttom
        //if (GUILayout.Button("Generate Map"))
        //{
        //    Debug.Log("Generating map");
        //    MapGenerator map = target as MapGenerator;
        //    map.GenerateMap();
        //}
    }
}
