using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditorInternal;

namespace Stem
{
	internal class MusicBankInspectorViewModel
	{
		private MusicBank musicBank = null;

		private Dictionary<Playlist, ReorderableList> reorderableLists = new Dictionary<Playlist, ReorderableList>();

		private List<string> playlistNames = new List<string>();
		private Dictionary<Playlist, int> playlistIndices = new Dictionary<Playlist, int>();
		private string[] playlistNamesWrapper = null;
		private NameDuplicatesManager playlistDuplicateManager = null;

		internal MusicBankInspectorViewModel(MusicBank bank)
		{
			playlistDuplicateManager = new NameDuplicatesManager(DecoratePlaylistName);

			musicBank = bank;

			playlistNames.Add("None");
			for (int i = 0; i < bank.Playlists.Count; i++)
			{
				Playlist playlist = bank.Playlists[i];
				string playlistName = playlistDuplicateManager.GrabName(playlist.Name);

				playlistNames.Add(playlistName);
				playlistIndices.Add(playlist, i + 1);
			}

			playlistNamesWrapper = playlistNames.ToArray();

			bank.OnPlaylistAdded += OnPlaylistAdded;
			bank.OnPlaylistRemoved += OnPlaylistRemoved;
			bank.OnPlaylistRenamed += OnPlaylistRenamed;
		}

		internal void Shutdown()
		{
			reorderableLists.Clear();

			playlistNames.Clear();
			playlistIndices.Clear();
			playlistNamesWrapper = null;

			musicBank.OnPlaylistAdded -= OnPlaylistAdded;
			musicBank.OnPlaylistRemoved -= OnPlaylistRemoved;
			musicBank.OnPlaylistRenamed -= OnPlaylistRenamed;
			musicBank = null;
		}

		internal string[] GrabPlaylistNames()
		{
			return playlistNamesWrapper;
		}

		internal int FetchPlaylistIndex(Playlist playlist)
		{
			if (playlist == null)
				return 0;

			int index = 0;
			playlistIndices.TryGetValue(playlist, out index);

			return index;
		}

		internal ReorderableList FetchReorderableList(Playlist playlist)
		{
			ReorderableList list = null;
			if (!reorderableLists.TryGetValue(playlist, out list))
			{
				list = new ReorderableList(playlist.TracksRaw, typeof(PlaylistTrack), true, true, true, true);
				list.elementHeightCallback = PlaylistTrackHeight;
				list.drawElementCallback = (_1, _2, _3, _4) => { PlaylistTrack(list, _1, _2, _3, _4); };
				list.drawHeaderCallback = PlaylistTrackHeader;

				reorderableLists.Add(playlist, list);
			}

			return list;
		}

		private string DecoratePlaylistName(string name)
		{
			return string.Format("[{0}]", name);
		}

		private void OnPlaylistAdded(Playlist playlist)
		{
			string playlistName = playlistDuplicateManager.GrabName(playlist.Name);

			playlistNames.Add(playlistName);
			playlistNamesWrapper = playlistNames.ToArray();

			playlistIndices.Add(playlist, musicBank.Playlists.Count);
		}

		private void OnPlaylistRemoved(Playlist playlist, int index)
		{
			playlistDuplicateManager.ReleaseName(playlist.Name);

			playlistNames.RemoveAt(index + 1);
			playlistNamesWrapper = playlistNames.ToArray();

			playlistIndices.Clear();
			for (int i = 0; i < musicBank.Playlists.Count; i++)
				playlistIndices.Add(musicBank.Playlists[i], i + 1);
		}

		private void OnPlaylistRenamed(Playlist playlist, int index, string oldName, string newName)
		{
			playlistDuplicateManager.ReleaseName(oldName);
			string playlistName = playlistDuplicateManager.GrabName(newName);

			playlistNames[index + 1] = playlistName;
			playlistNamesWrapper[index + 1] = playlistName;
		}

		private static float PlaylistTrackHeight(int index)
		{
			int numLines = 1;

			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * numLines;
		}

		private static void PlaylistTrackHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Tracks");
		}

		private static void PlaylistTrack(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
		{
			PlaylistTrack track = (PlaylistTrack)list.list[index];

			rect.height = EditorGUIUtility.singleLineHeight;
			rect.width /= 2;
			track.Clip = (AudioClip)EditorGUI.ObjectField(rect, track.Clip, typeof(AudioClip), false);

			rect.x += rect.width + 4;
			rect.width -= 4;
			track.Volume = EditorGUI.Slider(rect, track.Volume, 0.0f, 1.0f);
		}
	}

	[CustomEditor(typeof(MusicBank))]
	internal partial class MusicBankInspector : Editor
	{
		private GUILayoutOption defaultWidth = GUILayout.Width(22);

		private MusicBank bank = null;
		private MusicBankInspectorViewModel viewModel = null;

		public void OnEnable()
		{
			bank = target as MusicBank;
		}

		public void OnDisable()
		{
			if (viewModel != null)
			{
				viewModel.Shutdown();
				viewModel = null;
			}
		}

		public override void OnInspectorGUI()
		{
			if (viewModel == null)
				viewModel = new MusicBankInspectorViewModel(bank);

			Undo.RecordObject(bank, "Music Bank Change");
			EditorGUI.BeginChangeCheck();

			OnPlaylistDropAreaGUI();
			OnMusicBankMemoryManagementGUI();

			bank.ShowPlaylists = EditorGUILayout.Foldout(bank.ShowPlaylists, "Playlists");
			if (bank.ShowPlaylists)
			{
				List<Playlist> removedPlaylists = new List<Playlist>();
				foreach (Playlist playlist in bank.Playlists)
					OnPlaylistGUI(playlist, removedPlaylists);

				foreach (Playlist playlist in removedPlaylists)
					bank.RemovePlaylist(playlist);

				if (GUILayout.Button("Add Playlist"))
					bank.AddPlaylist("New Playlist");
			}

			bank.ShowPlayers = EditorGUILayout.Foldout(bank.ShowPlayers, "Music Players");
			if (bank.ShowPlayers)
			{
				List<MusicPlayer> removedPlayers = new List<MusicPlayer>();
				foreach (MusicPlayer player in bank.Players)
					OnMusicPlayerGUI(player, removedPlayers);

				foreach (MusicPlayer player in removedPlayers)
					bank.RemoveMusicPlayer(player);

				if (GUILayout.Button("Add Music Player"))
					bank.AddMusicPlayer("New Music Player");
			}

			OnPlaylistDrop();

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(bank);
		}

		private void OnPlaylistGUI(Playlist playlist, List<Playlist> removedPlaylists)
		{
			EditorGUILayout.BeginHorizontal();

			playlist.Unfolded = GUILayout.Toggle(playlist.Unfolded, "", "foldout", GUILayout.ExpandWidth(false));
			playlist.Name = EditorGUILayout.TextField(playlist.Name, GUILayout.ExpandWidth(true));

			if (GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
				removedPlaylists.Add(playlist);

			EditorGUILayout.EndHorizontal();

			if (playlist.Unfolded)
			{
				EditorGUILayout.BeginVertical("groupbox");

				OnPlaylistMemoryManagementGUI(playlist);

				ReorderableList list = viewModel.FetchReorderableList(playlist);
				list.DoLayoutList();

				EditorGUILayout.EndVertical();
			}
		}

		private void OnMusicPlayerGUI(MusicPlayer player, List<MusicPlayer> removedPlayers)
		{
			EditorGUILayout.BeginHorizontal();

			player.Unfolded = GUILayout.Toggle(player.Unfolded, "", "foldout", GUILayout.ExpandWidth(false));
			player.Name = EditorGUILayout.TextField(player.Name, GUILayout.ExpandWidth(true));
			player.Volume = EditorGUILayout.Slider(player.Volume, 0.0f, 1.0f);

			player.Muted = GUILayout.Toggle(player.Muted, "M", "button", defaultWidth, GUILayout.ExpandWidth(false));
			player.Soloed = GUILayout.Toggle(player.Soloed, "S", "button", defaultWidth, GUILayout.ExpandWidth(false));

			if (GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
				removedPlayers.Add(player);

			EditorGUILayout.EndHorizontal();

			if (player.Unfolded)
			{
				EditorGUILayout.BeginVertical("groupbox");

				int index = viewModel.FetchPlaylistIndex(player.Playlist);
				index = EditorGUILayout.Popup("Playlist", index, viewModel.GrabPlaylistNames(), GUILayout.ExpandWidth(true));

				player.Playlist = (index != 0) ? bank.Playlists[index - 1] : null;
				player.MixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField("Output", player.MixerGroup, typeof(AudioMixerGroup), false);
				player.Fade = EditorGUILayout.FloatField("Fade", Mathf.Max(0.0f, player.Fade));
				player.Shuffle = EditorGUILayout.Toggle("Shuffle", player.Shuffle);
				player.Loop = EditorGUILayout.Toggle("Loop", player.Loop);
				player.PlayOnStart = EditorGUILayout.Toggle("Play On Start", player.PlayOnStart);

				OnMusicPlayerPlaybackModeGUI(player);

				EditorGUILayout.EndVertical();
			}
		}
	}
}
