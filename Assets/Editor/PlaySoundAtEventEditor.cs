using UnityEditor;
using UnityEngine;

// https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f6
[CustomEditor(typeof(PlaySoundAtEventChannel), editorForChildClasses: true)]
public class PlaySoundAtEventChannelSOdAtEventEditor : Editor
{
    private Vector3 position;
    public AudioClip audioClip;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        PlaySoundAtEventChannel e = target as PlaySoundAtEventChannel;

        position = Vector3.zero;
        audioClip = (AudioClip)EditorGUILayout.ObjectField("Audio", audioClip, typeof(AudioClip), true);

        if (GUILayout.Button("Raise"))
        {
            e.Raise(audioClip, position);
        }
    }
}
