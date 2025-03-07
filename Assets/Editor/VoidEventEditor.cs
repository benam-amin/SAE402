using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoidEventChannel), editorForChildClasses: true)]
public class VoidEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        VoidEventChannel e = target as VoidEventChannel;
        if (GUILayout.Button("Raise"))
            e.Raise();
    }
}
