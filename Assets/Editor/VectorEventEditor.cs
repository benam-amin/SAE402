using UnityEditor;
using UnityEngine;

// https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f6
[CustomEditor(typeof(VectorEventChannel), editorForChildClasses: true)]
public class VectorEventEditor : Editor
{
    Vector3 value = Vector3.zero;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
 
        GUI.enabled = Application.isPlaying;

        VectorEventChannel e = target as VectorEventChannel;

        value = EditorGUILayout.Vector3Field("Value:", value);
        
        if (GUILayout.Button("Raise"))
            e.Raise(value);
    }
}
