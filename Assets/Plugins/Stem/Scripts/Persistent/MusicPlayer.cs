using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Stem
{
	/// <summary>
	/// Defines how music player plays its tracks.
	/// </summary>
	public enum MusicPlayerPlaybackMode
	{
		/// <summary>
		/// The music player will play a single track from the current playlist.
		/// </summary>
		Default,

		/// <summary>
		/// The music player will advance to the next track from the current playlist after the current track is finished.
		/// For non-zero <see cref="MusicPlayer.Fade"/> property values the music player will automatically crossfade between current and next tracks.
		/// </summary>
		AutoAdvance,

		/// <summary>
		/// The music player will synchronize current track time according to other music players in a sync group.
		/// All music players with the same <see cref="MusicPlayer.SyncGroup"/> value will be automatically synchronized.
		///</summary>
		Synced,
	}

	/// <summary>
	/// The persistent storage for playback rules of a particular playlist.
	/// </summary>
	[Serializable]
	public class MusicPlayer : ISerializationCallbackReceiver
	{
		[SerializeField]
		private int id = LocalID.None;

		[NonSerialized]
		private Playlist playlist = null;

		[SerializeField]
		private string name = null;

		[SerializeField]
		private AudioMixerGroup mixerGroup = null;

		[SerializeField]
		private byte syncGroup = 0;

		[SerializeField]
		private MusicPlayerPlaybackMode playbackMode = MusicPlayerPlaybackMode.AutoAdvance;

		[SerializeField]
		private bool shuffle = false;

		[SerializeField]
		private bool loop = false;

		[SerializeField]
		private bool playOnStart = false;

		[SerializeField]
		private float fade = 0.5f;

		[SerializeField]
		private float volume = 1.0f;

		[SerializeField]
		private bool muted = false;

		[SerializeField]
		private bool soloed = false;

		[SerializeField]
		private bool unfolded = false;

		[NonSerialized]
		private MusicBank bank = null;

		/// <summary>
		/// The delegate informing about playback start in the music player.
		/// </summary>
		public event PlaybackChangedDelegate OnPlaybackStarted;

		/// <summary>
		/// The delegate informing about playback stop in the music player.
		/// </summary>
		/// <remarks>
		/// <para>This delegate will only be called after the music player fades out.</para>
		/// </remarks>
		public event PlaybackChangedDelegate OnPlaybackStopped;

		/// <summary>
		/// The delegate informing about playback pause in the music player.
		/// </summary>
		/// <remarks>
		/// <para>This delegate will only be called after the music player fades out.</para>
		/// </remarks>
		public event PlaybackChangedDelegate OnPlaybackPaused;

		/// <summary>
		/// The delegate informing about tracks changes in the music player.
		/// </summary>
		public event TrackChangedDelegate OnTrackChanged;

		internal MusicPlayer(MusicBank bank_, string name_)
		{
			bank = bank_;
			name = name_;
		}

		/// <summary>
		/// Prepares sound for serialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during serialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnBeforeSerialize()
		{
		}

		/// <summary>
		/// Prepares sound for runtime use after deserialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during deserialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnAfterDeserialize()
		{
		}

		/// <summary>
		/// The unique identifier for fast access to the music player.
		/// </summary>
		/// <value>A unique identifier value of the music player.</value>
		public int ID
		{
			get { return id; }
			internal set { id = value; }
		}

		/// <summary>
		/// The flag indicating if the music player can be heard.
		/// </summary>
		/// <value>True, if the music player can be heard. False otherwise.</value>
		public bool Audible
		{
			get
			{
				if (bank != null && bank.Runtime.SoloedMusicPlayers > 0)
					return soloed;

				return !muted;
			}
		}

		/// <summary>
		/// The music bank the music player belongs to.
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
					bank.RemoveMusicPlayer(this);

				bank = value;

				if (bank != null)
					bank.AddMusicPlayer(this);
			}
		}

		/// <summary>
		/// The name of the music player. Used for fast search in corresponding music bank.
		/// </summary>
		/// <value>Name of the music player.</value>
		public string Name
		{
			get { return name; }
			set
			{
				if (name == value)
					return;

				if (bank != null)
					bank.RenameMusicPlayer(this, name, value);

				name = value;
			}
		}

		/// <summary>
		/// The reference to a playlist which will be played.
		/// </summary>
		/// <value>A reference to a playlist.</value>
		public Playlist Playlist
		{
			get { return playlist; }
			set { playlist = value; }
		}

		/// <summary>
		/// The reference to an audio mixer group. Please refer to Unity Scripting Reference for details.
		/// </summary>
		/// <value>A reference to a mixer group.</value>
		public AudioMixerGroup MixerGroup
		{
			get { return mixerGroup; }
			set { mixerGroup = value; }
		}

		/// <summary>
		/// The playback mode defining how music player should play its tracks.
		/// </summary>
		/// <value>An enum value.</value>
		public MusicPlayerPlaybackMode PlaybackMode
		{
			get { return playbackMode; }
			set { playbackMode = value; }
		}

		/// <summary>
		/// The sync group of the music player. Music players with the same sync group will share playback time.
		/// </summary>
		/// <value>Sync group of the music player.</value>
		/// <remarks>
		/// <para>Sync groups work only if <see cref="MusicPlayer.PlaybackMode"/> value is <see cref="MusicPlayerPlaybackMode.Synced"/>.
		/// </para>
		/// </remarks>
		public byte SyncGroup
		{
			get { return syncGroup; }
			set { syncGroup = value; }
		}

		/// <summary>
		/// The flag indicating whether the music player should play tracks in random order.
		/// </summary>
		/// <value>True, if the music player is playing playlist tracks in random order. False if it's playing sequentially.</value>
		/// <remarks>
		/// <para>Note that tracks will be reshuffled again after the player will finish playing all the tracks.</para>
		/// </remarks>
		public bool Shuffle
		{
			get { return shuffle; }
			set { shuffle = value; }
		}

		/// <summary>
		/// The flag indicating whether the music player should repeat playlist tracks after they finish.
		/// </summary>
		/// <value>True, if the music player is looping playlist tracks. False otherwise.</value>
		/// <remarks>
		/// <para>
		/// If <see cref="MusicPlayer.PlaybackMode"/> value is <see cref="MusicPlayerPlaybackMode.AutoAdvance"/> then this
		/// flag is indicating whether the music player should repeat the whole playlist instead of individual playlist track.
		/// </para>
		/// </remarks>
		public bool Loop
		{
			get { return loop; }
			set { loop = value; }
		}

		/// <summary>
		/// The flag indicating whether the music player should start playing once the game started.
		/// </summary>
		/// <value>True, if the music player should play on start. False otherwise.</value>
		public bool PlayOnStart
		{
			get { return playOnStart; }
			set { playOnStart = value; }
		}

		/// <summary>
		/// The crossfade parameter that is used when the music player transitions between tracks or playback states.
		/// </summary>
		/// <value>Crossfade duration in seconds.</value>
		public float Fade
		{
			get { return fade; }
			set { fade = value; }
		}

		/// <summary>
		/// The volume of the music player.
		/// </summary>
		/// <value>Volume of the music player. Value must be in [0;1] range.</value>
		public float Volume
		{
			get { return volume; }
			set { volume = value; }
		}

		/// <summary>
		/// The flag indicating if the music player is muted and can't be heard.
		/// </summary>
		/// <value>True, if the music player is muted. False otherwise.</value>
		/// <remarks>
		/// <para>This flag may be overridden by the <see cref="MusicPlayer.Soloed"/> flag, i.e. if the music player is simultaneously muted and soloed it'll be audible.</para>
		/// </remarks>
		public bool Muted
		{
			get { return muted; }
			set { muted = value; }
		}

		/// <summary>
		/// The flag indicating if the music player is soloed. If set to true, all other non-solo music players won't be audible.
		/// </summary>
		/// <value>True, if the music player is soloed. False otherwise.</value>
		/// <remarks>
		/// <para>This flag may override the <see cref="MusicPlayer.Muted"/> flag, i.e. if the music player is simultaneously muted and soloed it'll be audible.</para>
		/// </remarks>
		public bool Soloed
		{
			get { return soloed; }
			set
			{
				if (soloed == value)
					return;

				soloed = value;
				if (bank != null)
					bank.Runtime.SoloedMusicPlayers += (soloed) ? 1 : -1;
			}
		}

		/// <summary>
		/// The flag indicating whether the music bank inspector should show advanced settings for the music player.
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

		internal void InvokePlaybackStart()
		{
			if (OnPlaybackStarted != null)
				OnPlaybackStarted(this);
		}

		internal void InvokePlaybackStop()
		{
			if (OnPlaybackStopped != null)
				OnPlaybackStopped(this);
		}

		internal void InvokePlaybackPause()
		{
			if (OnPlaybackPaused != null)
				OnPlaybackPaused(this);
		}

		internal void InvokeTrackChange(PlaylistTrack oldTrack, PlaylistTrack newTrack)
		{
			if (OnTrackChanged != null)
				OnTrackChanged(this, oldTrack, newTrack);
		}
	}
}
