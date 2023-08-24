using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(MapCreator))]
public class MapCreatorTool : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapCreator creator = (MapCreator)target;
        if (GUILayout.Button("Create Map"))
        {
            creator.Create();
        }

        if (GUILayout.Button("Create Template"))
        {
            creator.Template();
        }
    }
}

#endif