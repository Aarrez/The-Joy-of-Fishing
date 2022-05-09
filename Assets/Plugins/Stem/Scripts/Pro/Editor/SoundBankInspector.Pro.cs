using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditorInternal;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	internal class SoundBankInspectorPlayer
	{
		private GameObject playerGameObject = null;
		private SoundBankInspectorPlayerRuntime player = null;

		internal SoundBankInspectorPlayer(SoundBank bank)
		{
			playerGameObject = new GameObject();
			playerGameObject.hideFlags = HideFlags.HideInHierarchy;

			if (!EditorApplication.isPlaying)
				playerGameObject.hideFlags |= HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

			player = playerGameObject.AddComponent<SoundBankInspectorPlayerRuntime>();
			player.Init(bank);
		}

		internal void Shutdown()
		{
			GameObject.DestroyImmediate(playerGameObject);

			playerGameObject = null;
			player = null;
		}

		internal void Play(Sound sound)
		{
			player.Play(sound);
		}
	}

	internal partial class SoundBankInspector : Editor
	{
		private const float dropAreaHeight = 50.0f;
		private Rect dropArea;

		private SoundBankInspectorPlayer player = null;

		private void OnPlayerInit(SoundBank bank)
		{
			if (player != null)
				return;

			player = new SoundBankInspectorPlayer(bank);
		}

		private void OnPlayerShutdown()
		{
			if (player == null)
				return;

			player.Shutdown();
			player = null;
		}

		private void OnPlayerPlay(Sound sound)
		{
			if (GUILayout.Button("Play", GUILayout.ExpandWidth(false)))
				player.Play(sound);
		}

		private void OnSoundDropAreaGUI()
		{
			EditorGUILayout.Space();
			bank.SoundBatchImportMode = (AudioClipImportMode)EditorGUILayout.EnumPopup("Batch Import Mode", bank.SoundBatchImportMode);

			dropArea = GUILayoutUtility.GetRect(0.0f, dropAreaHeight, GUILayout.ExpandWidth(true));

			GUIStyle style = new GUIStyle("Box");
			style.alignment = TextAnchor.MiddleCenter;
			GUI.Box(dropArea, "Drag AudioClips here to add sounds", style);

			EditorGUILayout.Space();
		}

		private void OnSoundBankMemoryManagementGUI()
		{
			EditorGUILayout.Space();

			bank.SoundManagementMode = (AudioClipManagementMode)EditorGUILayout.EnumPopup("Memory Mode", bank.SoundManagementMode);

			if (bank.SoundManagementMode == AudioClipManagementMode.UnloadUnused)
				bank.SoundUnloadInterval = EditorGUILayout.FloatField("Unload Interval", Mathf.Max(0.0f, bank.SoundUnloadInterval));

			EditorGUILayout.Space();
		}

		private void OnSoundMemoryManagementGUI(Sound sound)
		{
			EditorGUILayout.Space();

			sound.OverrideAudioClipManagement = EditorGUILayout.Toggle("Override Memory Mode", sound.OverrideAudioClipManagement);

			bool oldEnabled = GUI.enabled;
			GUI.enabled = sound.OverrideAudioClipManagement;

			sound.AudioClipManagementMode = (AudioClipManagementMode)EditorGUILayout.EnumPopup("Memory Mode", sound.GetAudioClipManagementMode());

			if (sound.GetAudioClipManagementMode() == AudioClipManagementMode.UnloadUnused)
				sound.AudioClipUnloadInterval = EditorGUILayout.FloatField("Unload Interval", Mathf.Max(0.0f, sound.GetAudioClipUnloadInterval()));

			GUI.enabled = oldEnabled;

			EditorGUILayout.Space();
		}

		private void OnSoundBusAdditionalGUI(SoundBus bus)
		{
			bus.PlayLimitInterval = EditorGUILayout.FloatField("Play Limit Interval", Mathf.Max(0.0f, bus.PlayLimitInterval));
		}

		private void OnSoundDrop()
		{
			Event evt = Event.current;
			if (evt.type != EventType.DragUpdated && evt.type != EventType.DragPerform)
				return;

			if (!dropArea.Contains(evt.mousePosition))
				return;

			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

			if (evt.type == EventType.DragPerform)
			{
				DragAndDrop.AcceptDrag();
				switch (bank.SoundBatchImportMode)
				{
					case AudioClipImportMode.SingleItemWithAllClips:
					{
						List<AudioClip> clips = new List<AudioClip>();
						foreach (Object obj in DragAndDrop.objectReferences)
						{
							AudioClip clip = obj as AudioClip;
							if (clip == null)
								continue;

							clips.Add(clip);
						}

						if (clips.Count > 0)
						{
							bank.AddSound(clips[0].name, clips.ToArray());
							GUI.changed = true;
						}
					}
					break;
					case AudioClipImportMode.MultipleItemsWithSingleClip:
					{
						foreach (Object obj in DragAndDrop.objectReferences)
						{
							AudioClip clip = obj as AudioClip;
							if (clip == null)
								continue;

							bank.AddSound(clip.name, clip);
							GUI.changed = true;
						}
					}
					break;
				}
			}
			evt.Use();
		}
	}
}

#endif
