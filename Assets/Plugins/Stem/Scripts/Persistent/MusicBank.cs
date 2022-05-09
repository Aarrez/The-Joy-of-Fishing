using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// A music bank callback function, called after adding playlist to the bank.
	/// </summary>
	/// <param name="playlist">A reference to a newly added playlist.</param>
	public delegate void PlaylistAddedDelegate(Playlist playlist);

	/// <summary>
	/// A music bank callback function, called before removing the playlist from the bank.
	/// </summary>
	/// <param name="playlist">A reference to a playlist to be removed.</param>
	/// <param name="index">An index in corresponding <see cref="MusicBank.Playlists"/> collection.</param>
	public delegate void PlaylistRemovedDelegate(Playlist playlist, int index);

	/// <summary>
	/// A music bank callback function, called after changing playlist name.
	/// </summary>
	/// <param name="playlist">A reference to a playlist.</param>
	/// <param name="index">An index in corresponding <see cref="MusicBank.Playlists"/> collection.</param>
	/// <param name="oldName">An old name of the playlist.</param>
	/// <param name="newName">A new name of the playlist.</param>
	public delegate void PlaylistRenamedDelegate(Playlist playlist, int index, string oldName, string newName);

	/// <summary>
	/// A music bank callback function, called after adding music player to the bank.
	/// </summary>
	/// <param name="player">A reference to a newly added music player.</param>
	public delegate void MusicPlayerAddedDelegate(MusicPlayer player);

	/// <summary>
	/// A music bank callback function, called before removing the music player from the bank.
	/// </summary>
	/// <param name="player">A reference to a music player to be removed.</param>
	/// <param name="index">An index in corresponding <see cref="MusicBank.Players"/> collection.</param>
	public delegate void MusicPlayerRemovedDelegate(MusicPlayer player, int index);

	/// <summary>
	/// A music bank callback function, called after changing music player name.
	/// </summary>
	/// <param name="player">A reference to a music player.</param>
	/// <param name="index">An index in corresponding <see cref="MusicBank.Players"/> collection.</param>
	/// <param name="oldName">An old name of the music player.</param>
	/// <param name="newName">A new name of the music player.</param>
	public delegate void MusicPlayerRenamedDelegate(MusicPlayer player, int index, string oldName, string newName);

	/// <summary>
	/// The attribute class used to make an int variable in a script be restricted to a playlist id.
	/// </summary>
	/// <remarks>
	/// When this attribute is used, the variable will be shown as two dropdown fields in the inspector instead of the default number field.
	/// </remarks>
	public class PlaylistIDAttribute : PropertyAttribute { }

	/// <summary>
	/// The attribute class used to make an int variable in a script be restricted to a music player id.
	/// </summary>
	/// <remarks>
	/// When this attribute is used, the variable will be shown as two dropdown fields in the inspector instead of the default number field.
	/// </remarks>
	public class MusicPlayerIDAttribute : PropertyAttribute { }

	/// <summary>
	/// The persistent storage for playlists and music players.
	/// </summary>
	[CreateAssetMenu(fileName = "New Music Bank", menuName = "Stem/Music Bank", order = 1001)]
	public class MusicBank : ScriptableObject, IBank, ISerializationCallbackReceiver
	{
		[SerializeField]
		private int guidA = 0;

		[SerializeField]
		private int guidB = 0;

		[SerializeField]
		private int guidC = 0;

		[SerializeField]
		private int guidD = 0;

		[SerializeField]
		private AudioClipImportMode playlistBatchImportMode = AudioClipImportMode.SingleItemWithAllClips;

		[SerializeField]
		private bool showPlaylists = true;

		[SerializeField]
		private bool showPlayers = true;

		[SerializeField]
		private List<Playlist> playlists = new List<Playlist>();
		private ReadOnlyCollection<Playlist> playlistsRO = null;

		[SerializeField]
		private List<MusicPlayer> players = new List<MusicPlayer>();
		private ReadOnlyCollection<MusicPlayer> playersRO = null;

		[SerializeField]
		private int[] playlistIndices = null;

		[SerializeField]
		private AudioClipManagementMode playlistManagementMode = AudioClipManagementMode.UnloadUnused;

		[SerializeField]
		private float playlistUnloadInterval = 60.0f;

		[NonSerialized]
		private MusicBankRuntime runtime = new MusicBankRuntime();

		/// <summary>
		/// The delegate informing about adding playlists.
		/// </summary>
		public event PlaylistAddedDelegate OnPlaylistAdded;

		/// <summary>
		/// The delegate informing about removing playlists.
		/// </summary>
		public event PlaylistRemovedDelegate OnPlaylistRemoved;

		/// <summary>
		/// The delegate informing about the change of playlist names.
		/// </summary>
		public event PlaylistRenamedDelegate OnPlaylistRenamed;

		/// <summary>
		/// The delegate informing about adding music players.
		/// </summary>
		public event MusicPlayerAddedDelegate OnMusicPlayerAdded;

		/// <summary>
		/// The delegate informing about removing music players.
		/// </summary>
		public event MusicPlayerRemovedDelegate OnMusicPlayerRemoved;

		/// <summary>
		/// The delegate informing about the change of music player names.
		/// </summary>
		public event MusicPlayerRenamedDelegate OnMusicPlayerRenamed;

		/// <summary>
		/// Prepares music bank for serialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during serialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnBeforeSerialize()
		{
			if (players.Count > 0)
			{
				playlistIndices = new int[players.Count];
				for (int i = 0; i < players.Count; i++)
				{
					Playlist playlist = players[i].Playlist;
					playlistIndices[i] = playlists.IndexOf(playlist);
				}
			}

			if (guidA == 0 && guidB == 0 && guidC == 0 && guidD == 0)
				RegenerateBankID();
		}

		/// <summary>
		/// Prepares music bank for runtime use after deserialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during deserialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnAfterDeserialize()
		{
			if (guidA == 0 && guidB == 0 && guidC == 0 && guidD == 0)
				RegenerateBankID();

			runtime = new MusicBankRuntime();
			for (int i = 0; i < players.Count; i++)
			{
				MusicPlayer player = players[i];
				Playlist playlist = null;
				if (playlistIndices != null)
				{
					int index = playlistIndices[i];
					playlist = (index != -1) ? playlists[index] : null;
				}
				player.Playlist = playlist;
				player.Bank = this;

				runtime.AddMusicPlayer(player);
			}

			foreach (Playlist playlist in playlists)
			{
				playlist.Bank = this;
				runtime.AddPlaylist(playlist);
			}
		}

		/// <summary>
		/// Returns music bank <see cref="Stem.ID"/>.
		/// </summary>
		/// <returns>
		/// <para>An ID value.</para>
		/// </returns>
		public ID GetBankID()
		{
			return new ID(guidA, guidB, guidC, guidD, 0);
		}

		/// <summary>
		/// Generates a new unique <see cref="Stem.ID"/> for the music bank.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Stem during serialization process. Don't call it manually as it may break existing ID references.</para>
		/// </remarks>
		public void RegenerateBankID()
		{
			System.Guid guid = System.Guid.NewGuid();
			byte[] guidBytes = guid.ToByteArray();

			guidA = BitConverter.ToInt32(guidBytes, 0);
			guidB = BitConverter.ToInt32(guidBytes, 4);
			guidC = BitConverter.ToInt32(guidBytes, 8);
			guidD = BitConverter.ToInt32(guidBytes, 12);
		}

		/// <summary>
		/// The collection of playlists.
		/// </summary>
		/// <value>A reference to a read-only collection of playlists.</value>
		public ReadOnlyCollection<Playlist> Playlists
		{
			get
			{
				if (playlistsRO == null)
					playlistsRO = playlists.AsReadOnly();

				return playlistsRO;
			}
		}

		/// <summary>
		/// The collection of music players.
		/// </summary>
		/// <value>A reference to a read-only collection of music players.</value>
		public ReadOnlyCollection<MusicPlayer> Players
		{
			get
			{
				if (playersRO == null)
					playersRO = players.AsReadOnly();

				return playersRO;
			}
		}

		/// <summary>
		/// The flag indicating whether the music bank inspector should show the 'Playlists' group.
		/// </summary>
		/// <value>True, if the 'Playlists' group is shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the music bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool ShowPlaylists
		{
			get { return showPlaylists; }
			set { showPlaylists = value; }
		}

		/// <summary>
		/// The flag indicating whether the music bank inspector should show the 'Players' group.
		/// </summary>
		/// <value>True, if the 'Players' group is shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the music bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool ShowPlayers
		{
			get { return showPlayers; }
			set { showPlayers = value; }
		}

		/// <summary>
		/// The batch import mode defining how new playlists will be created after the drag-drop event.
		/// </summary>
		/// <value>An enum value.</value>
		public AudioClipImportMode PlaylistBatchImportMode
		{
			get { return playlistBatchImportMode; }
			set { playlistBatchImportMode = value; }
		}

		/// <summary>
		/// The default audio clip management mode for all playlists of the music bank.
		/// </summary>
		/// <remarks>
		/// <para>By default, all playlists will use this value for audio clip management, however, it can be overridden by the <see cref="Playlist.OverrideAudioClipManagement"/> flag.</para>
		/// </remarks>
		/// <value>An enum value.</value>
		public AudioClipManagementMode PlaylistManagementMode
		{
			get { return playlistManagementMode; }
			set { playlistManagementMode = value; }
		}

		/// <summary>
		/// The default audio clip unload interval for all playlists of the the music bank.
		/// </summary>
		/// <remarks>
		/// <para>By default, all playlists will use this value for audio clip unload interval, however, it can be overridden by the <see cref="Playlist.OverrideAudioClipManagement"/> flag.</para>
		/// </remarks>
		/// <value>A time interval in seconds.</value>
		public float PlaylistUnloadInterval
		{
			get { return playlistUnloadInterval; }
			set { playlistUnloadInterval = value; }
		}

		internal MusicBankRuntime Runtime
		{
			get { return runtime; }
		}

		/// <summary>
		/// Searches for the specified playlist by <see cref="Stem.ID"/>.
		/// </summary>
		/// <param name="id">ID that refers to a playlist.</param>
		/// <returns>
		/// A reference to a playlist, if found. Null reference otherwise.
		/// </returns>
		public Playlist GetPlaylist(ID id)
		{
			if (id.BankGuidA != guidA)
				return null;

			if (id.BankGuidB != guidB)
				return null;

			if (id.BankGuidC != guidC)
				return null;

			if (id.BankGuidD != guidD)
				return null;

			return runtime.GetPlaylist(id.ItemId);
		}

		/// <summary>
		/// Searches for the specified playlist with a matching name.
		/// </summary>
		/// <param name="name">Name of the playlist.</param>
		/// <returns>
		/// A reference to a playlist, if found. Null reference otherwise.
		/// </returns>
		public Playlist GetPlaylist(string name)
		{
			return runtime.GetPlaylist(name);
		}

		/// <summary>
		/// Gets an <see cref="Stem.ID"/> to the specific playlist.
		/// </summary>
		/// <param name="index">Zero-based index of the playlist in the current music bank.</param>
		/// <returns>
		/// An <see cref="Stem.ID"/> to the specific playlist.
		/// </returns>
		public ID GetPlaylistID(int index)
		{
			if (index >= playlists.Count)
				return ID.None;

			return new ID(guidA, guidB, guidC, guidD, playlists[index].ID);
		}

		/// <summary>
		/// Adds an empty playlist to the music bank.
		/// </summary>
		/// <param name="name">Name of the playlist.</param>
		/// <returns>
		/// A reference to a newly created playlist.
		/// </returns>
		public Playlist AddPlaylist(string name)
		{
			Playlist playlist = new Playlist(this, name);

			playlists.Add(playlist);
			runtime.AddPlaylist(playlist);

			if (OnPlaylistAdded != null)
				OnPlaylistAdded(playlist);

			return playlist;
		}

		/// <summary>
		/// Adds a new playlist with a single track to the music bank.
		/// </summary>
		/// <param name="name">Name of the playlist.</param>
		/// <param name="clip">A reference to the audio clip with music data.</param>
		/// <returns>
		/// A reference to a newly created playlist.
		/// </returns>
		public Playlist AddPlaylist(string name, AudioClip clip)
		{
			Playlist playlist = new Playlist(this, name);
			playlist.AddTrack(clip);

			playlists.Add(playlist);
			runtime.AddPlaylist(playlist);

			if (OnPlaylistAdded != null)
				OnPlaylistAdded(playlist);

			return playlist;
		}

		/// <summary>
		/// Adds a new playlist with multiple tracks to the music bank.
		/// </summary>
		/// <param name="name">Name of the playlist.</param>
		/// <param name="clips">An array of audio clips with music data.</param>
		/// <returns>
		/// A reference to a newly created playlist.
		/// </returns>
		public Playlist AddPlaylist(string name, AudioClip[] clips)
		{
			Playlist playlist = new Playlist(this, name);
			playlist.AddTracks(clips);

			playlists.Add(playlist);
			runtime.AddPlaylist(playlist);

			if (OnPlaylistAdded != null)
				OnPlaylistAdded(playlist);

			return playlist;
		}

		/// <summary>
		/// Removes existing playlist from the music bank.
		/// </summary>
		/// <param name="playlist">A reference to a playlist.</param>
		/// <remarks>
		/// <para>This method does nothing if the playlist was not found in the music bank.</para>
		/// <para>All existing music players containing removed playlist will set their playlist reference to null.</para>
		/// </remarks>
		public void RemovePlaylist(Playlist playlist)
		{
			int index = playlists.IndexOf(playlist);
			if (index == -1)
				return;

			if (OnPlaylistRemoved != null)
				OnPlaylistRemoved(playlist, index);

			foreach (MusicPlayer player in players)
				if (player.Playlist == playlist)
					player.Playlist = null;

			playlists.RemoveAt(index);
			runtime.RemovePlaylist(playlist);
		}

		/// <summary>
		/// Searches for the specified music player by <see cref="Stem.ID"/>.
		/// </summary>
		/// <param name="id">ID that refers to a music player.</param>
		/// <returns>
		/// A reference to a music player, if found. Null reference otherwise.
		/// </returns>
		public MusicPlayer GetMusicPlayer(ID id)
		{
			if (id.BankGuidA != guidA)
				return null;

			if (id.BankGuidB != guidB)
				return null;

			if (id.BankGuidC != guidC)
				return null;

			if (id.BankGuidD != guidD)
				return null;

			return runtime.GetMusicPlayer(id.ItemId);
		}

		/// <summary>
		/// Searches for the specified music player with a matching name.
		/// </summary>
		/// <param name="name">Name of the music player.</param>
		/// <returns>
		/// A reference to a music player, if found. Null reference otherwise.
		/// </returns>
		public MusicPlayer GetMusicPlayer(string name)
		{
			return runtime.GetMusicPlayer(name);
		}

		/// <summary>
		/// Gets an <see cref="Stem.ID"/> to the specific music player.
		/// </summary>
		/// <param name="index">Zero-based index of the music player in the current music bank.</param>
		/// <returns>
		/// An <see cref="Stem.ID"/> to the specific music player.
		/// </returns>
		public ID GetMusicPlayerID(int index)
		{
			if (index >= players.Count)
				return ID.None;

			return new ID(guidA, guidB, guidC, guidD, players[index].ID);
		}

		/// <summary>
		/// Adds a new music player to the music bank.
		/// </summary>
		/// <param name="name">Name of the music player.</param>
		/// <returns>
		/// A reference to a newly created music player.
		/// </returns>
		public MusicPlayer AddMusicPlayer(string name)
		{
			MusicPlayer player = new MusicPlayer(this, name);

			players.Add(player);
			runtime.AddMusicPlayer(player);

			if (OnMusicPlayerAdded != null)
				OnMusicPlayerAdded(player);

			return player;
		}

		/// <summary>
		/// Removes existing music player from the music bank.
		/// </summary>
		/// <param name="player">A reference to a music player.</param>
		/// <remarks>
		/// <para>This method does nothing if the music player was not found in the music bank.</para>
		/// </remarks>
		public void RemoveMusicPlayer(MusicPlayer player)
		{
			int index = players.IndexOf(player);
			if (index == -1)
				return;

			if (OnMusicPlayerRemoved != null)
				OnMusicPlayerRemoved(player, index);

			players.RemoveAt(index);
			runtime.RemoveMusicPlayer(player);
		}

		internal void AddPlaylist(Playlist playlist)
		{
			if (playlists.Contains(playlist))
				return;

			playlists.Add(playlist);
			runtime.AddPlaylist(playlist);

			if (OnPlaylistAdded != null)
				OnPlaylistAdded(playlist);
		}

		internal void RenamePlaylist(Playlist playlist, string oldName, string newName)
		{
			int index = playlists.IndexOf(playlist);
			if (index == -1)
				return;

			runtime.RenamePlaylist(playlist, oldName, newName);

			if (OnPlaylistRenamed != null)
				OnPlaylistRenamed(playlist, index, oldName, newName);
		}

		internal void AddMusicPlayer(MusicPlayer player)
		{
			if (players.Contains(player))
				return;

			players.Add(player);
			runtime.AddMusicPlayer(player);

			if (OnMusicPlayerAdded != null)
				OnMusicPlayerAdded(player);
		}

		internal void RenameMusicPlayer(MusicPlayer player, string oldName, string newName)
		{
			int index = players.IndexOf(player);
			if (index == -1)
				return;

			runtime.RenameMusicPlayer(player, oldName, newName);

			if (OnMusicPlayerRenamed != null)
				OnMusicPlayerRenamed(player, index, oldName, newName);
		}

		internal void OnDestroy()
		{
			if (MusicManager.Banks.Contains(this))
				MusicManager.DeregisterBank(this);
		}
	}
}
