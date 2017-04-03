using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        // Always updating ... cpu intensive sometimes
        MapGenerator map = target as MapGenerator;
        
        // just update when some value is changed in the inspector (much more effective)
        if (DrawDefaultInspector())
        {
            map.GenerateMap();
        }

        // or update just when we press this buttom (in case of we change something via script)
        if (GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }
    }
}
