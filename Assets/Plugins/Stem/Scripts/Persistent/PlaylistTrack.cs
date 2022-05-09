using System;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The persistent storage for music audio data.
	/// </summary>
	[Serializable]
	public class PlaylistTrack
	{
		[SerializeField]
		private AudioClip clip = null;

		[SerializeField]
		private float volume = 1.0f;

		[NonSerialized]
		private Playlist playlist = null;

		/// <summary>
		/// The name of the track.
		/// </summary>
		/// <value>Name of the current audio clip being used. Null reference otherwise.</value>
		public string Name
		{
			get { return (clip) ? clip.name : null; }
		}

		/// <summary>
		/// The audio clip with audio data. Please refer to Unity Scripting Reference for details.
		/// </summary>
		/// <value>A reference to an audio clip.</value>
		public AudioClip Clip
		{
			get { return clip; }
			set { clip = value; }
		}

		/// <summary>
		/// The volume of the track.
		/// </summary>
		/// <value>Volume of the track. Value must be in [0;1] range.</value>
		public float Volume
		{
			get { return volume; }
			set { volume = value; }
		}

		/// <summary>
		/// The playlist the track belongs to.
		/// </summary>
		/// <value>A reference to a playlist.</value>
		public Playlist Playlist
		{
			get { return playlist; }
			set { playlist = value; }
		}
	}
}
