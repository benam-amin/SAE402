using UnityEditor;
using UnityEngine;

// https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f6
[CustomEditor(typeof(BoolEventChannel), editorForChildClasses: true)]
public class BoolEventEditor : Editor
{
    bool value = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
 
        GUI.enabled = Application.isPlaying;

        BoolEventChannel e = target as BoolEventChannel;

        value = EditorGUILayout.Toggle("Is Active", value);
    
        if (GUILayout.Button("Raise")) {
            e.Raise(value);
        }
    }
}
