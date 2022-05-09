using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The persistent collection of playlist tracks.
	/// </summary>
	[Serializable]
	public class Playlist : IAudioClipContainer, ISerializationCallbackReceiver
	{
		[SerializeField]
		private int id = LocalID.None;

		[SerializeField]
		private string name = null;

		[SerializeField]
		private List<PlaylistTrack> tracks = new List<PlaylistTrack>();

		[SerializeField]
		private bool unfolded = false;

		[SerializeField]
		private AudioClipManagementMode audioClipManagementMode = AudioClipManagementMode.UnloadUnused;

		[SerializeField]
		private float audioClipUnloadInterval = 60.0f;

		[SerializeField]
		private bool overrideAudioClipManagement = false;

		[NonSerialized]
		private ReadOnlyCollection<PlaylistTrack> tracksRO = null;

		[NonSerialized]
		private MusicBank bank = null;

		internal Playlist(MusicBank bank_, string name_)
		{
			bank = bank_;
			name = name_;
		}

		/// <summary>
		/// Gets the number of audio clips in the playlist.
		/// </summary>
		/// <returns>
		/// The number of audio clips.
		/// </returns>
		public int GetNumAudioClips()
		{
			return tracks.Count;
		}

		/// <summary>
		/// Gets the audio clip at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the audio clip to get.</param>
		/// <returns>
		/// A reference to an audio clip.
		/// </returns>
		public AudioClip GetAudioClip(int index)
		{
			if (index < 0 || index >= tracks.Count)
				return null;

			return tracks[index].Clip;
		}

		/// <summary>
		/// Gets the audio clip unload interval of the playlist.
		/// </summary>
		/// <remarks>
		/// <para>This value is only used if <see cref="Playlist.GetAudioClipManagementMode"/> return value is <see cref="AudioClipManagementMode.UnloadUnused"/></para>
		/// <para>If <see cref="Playlist.OverrideAudioClipManagement"/> value is true, <see cref="Playlist.AudioClipUnloadInterval"/> will be used. Otherwise, <see cref="MusicBank.PlaylistUnloadInterval"/> value of the containing bank will be used.</para>
		/// </remarks>
		/// <returns>
		/// <para>The time interval in seconds.</para>
		/// </returns>
		public float GetAudioClipUnloadInterval()
		{
			if (overrideAudioClipManagement)
				return audioClipUnloadInterval;

			return (bank != null) ? bank.PlaylistUnloadInterval : 0.0f;
		}

		/// <summary>
		/// Gets the audio clip management mode of the playlist.
		/// </summary>
		/// <remarks>
		/// <para>If <see cref="Playlist.OverrideAudioClipManagement"/> value is true, <see cref="Playlist.AudioClipManagementMode"/> will be used. Otherwise, <see cref="MusicBank.PlaylistManagementMode"/> value of the containing bank will be used.</para>
		/// </remarks>
		/// <returns>
		/// <para>An enum value.</para>
		/// </returns>
		public AudioClipManagementMode GetAudioClipManagementMode()
		{
			if (overrideAudioClipManagement)
				return audioClipManagementMode;

			return (bank != null) ? bank.PlaylistManagementMode : AudioClipManagementMode.Manual;
		}

		/// <summary>
		/// Prepares playlist for serialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during serialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnBeforeSerialize()
		{
		}

		/// <summary>
		/// Prepares playlist for runtime use after deserialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during deserialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnAfterDeserialize()
		{
			foreach (PlaylistTrack track in tracks)
				track.Playlist = this;
		}

		/// <summary>
		/// The unique identifier for fast access to the playlist.
		/// </summary>
		/// <value>A unique identifier value of the playlist.</value>
		public int ID
		{
			get { return id; }
			internal set { id = value; }
		}

#if UNITY_EDITOR
		/// <summary>
		/// The collection of playlist tracks.
		/// </summary>
		/// <value>A reference to a collection of playlist tracks.</value>
		/// <remarks>
		/// <para>This property is only available in Unity Editor.<para>
		/// </remarks>
		public List<PlaylistTrack> TracksRaw
		{
			get { return tracks; }
		}
#endif

		/// <summary>
		/// The collection of playlist tracks.
		/// </summary>
		/// <value>A reference to a read-only collection of playlist tracks.</value>
		public ReadOnlyCollection<PlaylistTrack> Tracks
		{
			get
			{
				if (tracksRO == null)
					tracksRO = tracks.AsReadOnly();

				return tracksRO;
			}
		}

		/// <summary>
		/// The music bank the playlist belongs to.
		/// </summary>
		/// <value>A reference to a music bank.</value>
		public MusicBank Bank
		{
			get { return bank; }
			set
			{
				if (bank == value)
					return;

				if (bank != null)
					bank.RemovePlaylist(this);

				bank = value;

				if (bank != null)
					bank.AddPlaylist(this);
			}
		}

		/// <summary>
		/// The name of the playlist. Used for fast search in corresponding music bank.
		/// </summary>
		/// <value>Name of the playlist.</value>
		public string Name
		{
			get { return name; }
			set
			{
				if (name == value)
					return;

				if (bank != null)
					bank.RenamePlaylist(this, name, value);

				name = value;
			}
		}

		/// <summary>
		/// The flag indicating whether the music bank inspector should show advanced settings for the playlist.
		/// </summary>
		/// <value>True, if advanced settings are shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the music bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool Unfolded
		{
			get { return unfolded; }
			set { unfolded = value; }
		}

		/// <summary>
		/// The flag indicating whether the playlist should use its own audio clip management mode and unload interval.
		/// </summary>
		/// <remarks>
		/// <para>This flag defines the behaviour of <see cref="Playlist.GetAudioClipManagementMode"/> and <see cref="Playlist.GetAudioClipUnloadInterval"/> methods.</para>
		/// <para>
		/// If true, the playlist will use its own <see cref="Playlist.AudioClipManagementMode"/> and <see cref="Playlist.AudioClipUnloadInterval"/> properties.
		/// Otherwise, <see cref="MusicBank.PlaylistManagementMode"/> and <see cref="MusicBank.PlaylistUnloadInterval"/> properties of the containing bank will be used.
		/// </para>
		/// </remarks>
		/// <value>
		/// True, if the playlist has overridden audio clip management and unload interval. False otherwise.
		/// </value>
		public bool OverrideAudioClipManagement
		{
			get { return overrideAudioClipManagement; }
			set { overrideAudioClipManagement = value; }
		}

		/// <summary>
		/// The audio clip management mode of the playlist.
		/// </summary>
		/// <remarks>
		/// <para>This value is used only if <see cref="Playlist.OverrideAudioClipManagement"/> flag is true.</para>
		/// </remarks>
		/// <value>An enum value.</value>
		public AudioClipManagementMode AudioClipManagementMode
		{
			get { return audioClipManagementMode; }
			set { audioClipManagementMode = value; }
		}

		/// <summary>
		/// The audio clip unload interval of the playlist.
		/// </summary>
		/// <remarks>
		/// <para>This value is used only if <see cref="Playlist.OverrideAudioClipManagement"/> flag is true and <see cref="Playlist.GetAudioClipManagementMode"/> return value is <see cref="AudioClipManagementMode.UnloadUnused"/>.</para>
		/// </remarks>
		/// <value>A time interval in seconds.</value>
		public float AudioClipUnloadInterval
		{
			get { return audioClipUnloadInterval; }
			set { audioClipUnloadInterval = value; }
		}

		/// <summary>
		/// Adds a single music track to the playlist.
		/// </summary>
		/// <param name="clip">A reference to the audio clip with music data.</param>
		/// <returns>
		/// A reference to a newly created playlist track.
		/// </returns>
		public PlaylistTrack AddTrack(AudioClip clip)
		{
			PlaylistTrack track = new PlaylistTrack();
			track.Clip = clip;
			track.Playlist = this;

			tracks.Add(track);
			return track;
		}

		/// <summary>
		/// Adds multiple music tracks (one per audio clip) to the playlist.
		/// </summary>
		/// <param name="clips">An array of audio clips with music data.</param>
		public void AddTracks(AudioClip[] clips)
		{
			if (clips == null)
				return;

			for (int i = 0; i < clips.Length; i++)
			{
				PlaylistTrack track = new PlaylistTrack();
				track.Clip = clips[i];
				track.Playlist = this;

				tracks.Add(track);
			}
		}
	}
}
