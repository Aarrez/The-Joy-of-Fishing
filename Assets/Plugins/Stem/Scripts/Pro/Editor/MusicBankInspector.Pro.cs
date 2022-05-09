using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	internal partial class MusicBankInspector : Editor
	{
		private const float dropAreaHeight = 50.0f;
		private Rect dropArea;

		private void OnPlaylistDropAreaGUI()
		{
			EditorGUILayout.Space();
			bank.PlaylistBatchImportMode = (AudioClipImportMode)EditorGUILayout.EnumPopup("Batch Import Mode", bank.PlaylistBatchImportMode);

			dropArea = GUILayoutUtility.GetRect(0.0f, dropAreaHeight, GUILayout.ExpandWidth(true));

			GUIStyle style = new GUIStyle("Box");
			style.alignment = TextAnchor.MiddleCenter;
			GUI.Box(dropArea, "Drag AudioClips here to add playlists", style);

			EditorGUILayout.Space();
		}

		private void OnMusicBankMemoryManagementGUI()
		{
			EditorGUILayout.Space();

			bank.PlaylistManagementMode = (AudioClipManagementMode)EditorGUILayout.EnumPopup("Memory Mode", bank.PlaylistManagementMode);

			if (bank.PlaylistManagementMode == AudioClipManagementMode.UnloadUnused)
				bank.PlaylistUnloadInterval = EditorGUILayout.FloatField("Unload Interval", Mathf.Max(0.0f, bank.PlaylistUnloadInterval));

			EditorGUILayout.Space();
		}

		private void OnPlaylistMemoryManagementGUI(Playlist playlist)
		{
			playlist.OverrideAudioClipManagement = EditorGUILayout.Toggle("Override Memory Mode", playlist.OverrideAudioClipManagement);

			bool oldEnabled = GUI.enabled;
			GUI.enabled = playlist.OverrideAudioClipManagement;

			playlist.AudioClipManagementMode = (AudioClipManagementMode)EditorGUILayout.EnumPopup("Memory Mode", playlist.GetAudioClipManagementMode());

			if (playlist.GetAudioClipManagementMode() == AudioClipManagementMode.UnloadUnused)
				playlist.AudioClipUnloadInterval = EditorGUILayout.FloatField("Unload Interval", Mathf.Max(0.0f, playlist.GetAudioClipUnloadInterval()));

			GUI.enabled = oldEnabled;

			EditorGUILayout.Space();
		}

		private void OnPlaylistDrop()
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
				switch (bank.PlaylistBatchImportMode)
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
							bank.AddPlaylist(clips[0].name, clips.ToArray());
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

							bank.AddPlaylist(clip.name, clip);
							GUI.changed = true;
						}
					}
					break;
				}
			}
			evt.Use();
		}

		private void OnMusicPlayerPlaybackModeGUI(MusicPlayer player)
		{
			player.PlaybackMode = (MusicPlayerPlaybackMode)EditorGUILayout.EnumPopup("Playback Mode", player.PlaybackMode);

			if (player.PlaybackMode == MusicPlayerPlaybackMode.Synced)
				player.SyncGroup = (byte)EditorGUILayout.IntSlider("Sync Group", player.SyncGroup, 0, 255);
		}
	}
}

#endif
