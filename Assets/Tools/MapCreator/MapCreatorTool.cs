using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(MapCreator))]
public class MapCreatorTool : Editor
{
    /* Comment these function to build project */
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapCreator creator = (MapCreator)target;
        if (GUILayout.Button("Create Map"))
        {
            creator.Create();
        }
    }
}

#endif