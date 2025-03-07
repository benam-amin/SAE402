using UnityEditor;
using UnityEngine;

// https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f6
[CustomEditor(typeof(CameraShakeEventChannel), editorForChildClasses: true)]
public class CameraShakeEventEditor : Editor
{
    float duration;
    float magnitude;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
 
        GUI.enabled = Application.isPlaying;

        CameraShakeEventChannel e = target as CameraShakeEventChannel;

        duration = EditorGUILayout.FloatField("duration: ", duration);
        magnitude = EditorGUILayout.FloatField("magnitude: ", magnitude);

        ShakeTypeVariable shakeType = new ShakeTypeVariable();
        shakeType.Duration = duration;
        shakeType.Magnitude = magnitude;
    
        if (GUILayout.Button("Raise")) {
            e.Raise(shakeType);
        }
    }
}
