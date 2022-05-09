using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Stem
{
	/// <summary>
	/// The persistent storage for playback rules of a group of sounds.
	/// </summary>
	[Serializable]
	public class SoundBus : ISerializationCallbackReceiver
	{
		[SerializeField]
		private int id = LocalID.None;

		[SerializeField]
		private string name = null;

		[SerializeField]
		private bool muted = false;

		[SerializeField]
		private bool soloed = false;

		[SerializeField]
		private AudioMixerGroup mixerGroup = null;

		[SerializeField]
		private float volume = 1.0f;

		[SerializeField]
		private byte polyphony = 1;

		[SerializeField]
		private bool unfolded = false;

		[SerializeField]
		private bool allowVoiceStealing = true;

		[SerializeField]
		private float playLimitInterval = 0.0f;

		[NonSerialized]
		private SoundBank bank = null;

		internal SoundBus(SoundBank bank_, string name_)
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
		/// The unique identifier for fast access to the sound bus.
		/// </summary>
		/// <value>A unique identifier value of the sound bus.</value>
		public int ID
		{
			get { return id; }
			internal set { id = value; }
		}

		/// <summary>
		/// The flag indicating if the sound bus can be heard.
		/// </summary>
		/// <value>True, if the sound bus can be heard. False otherwise.</value>
		public bool Audible
		{
			get
			{
				if (bank != null && bank.Runtime.SoloedSoundBuses > 0)
					return soloed;

				return !muted;
			}
		}

		/// <summary>
		/// The sound bank the sound bus belongs to.
		/// </summary>
		/// <value>A reference to a sound bank.</value>
		public SoundBank Bank
		{
			get { return bank; }
			set
			{
				if (bank == value)
					return;

				if (bank != null)
					bank.RemoveSoundBus(this);

				bank = value;

				if (bank != null)
					bank.AddSoundBus(this);
			}
		}

		/// <summary>
		/// The name of the sound bus. Used for fast search in corresponding sound bank.
		/// </summary>
		/// <value>Name of the sound bus.</value>
		public string Name
		{
			get { return name; }
			set
			{
				if (name == value)
					return;

				if (bank != null)
					bank.RenameSoundBus(this, name, value);

				name = value;
			}
		}

		/// <summary>
		/// The flag indicating if the sound bus is muted and can't be heard.
		/// </summary>
		/// <value>True, if the sound bus is muted. False otherwise.</value>
		/// <remarks>
		/// <para>This flag may be overridden by the <see cref="SoundBus.Soloed"/> flag, i.e. if the sound bus is simultaneously muted and soloed it'll be audible.</para>
		/// </remarks>
		public bool Muted
		{
			get { return muted; }
			set { muted = value; }
		}

		/// <summary>
		/// The flag indicating if the sound bus is soloed. If set to true, all other non-solo sound buses won't be audible.
		/// </summary>
		/// <value>True, if the sound bus is soloed. False otherwise.</value>
		/// <remarks>
		/// <para>This flag may override the <see cref="SoundBus.Muted"/> flag, i.e. if the sound bus is simultaneously muted and soloed it'll be audible.</para>
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
					bank.Runtime.SoloedSoundBuses += (soloed) ? 1 : -1;
			}
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
		/// The volume of the sound bus.
		/// </summary>
		/// <value>Volume of the sound bus. Value must be in [0;1] range.</value>
		public float Volume
		{
			get { return volume; }
			set { volume = value; }
		}

		/// <summary>
		/// The number of maximum allowed simultaneously playing sounds in the sound bus.
		/// </summary>
		/// <value>Number of allowed sounds.</value>
		public byte Polyphony
		{
			get { return polyphony; }
			set { polyphony = Math.Max((byte)1, value); }
		}

		/// <summary>
		/// The flag indicating whether the sound bus can stop the oldest playing sound and play the new one if <see cref="SoundBus.Polyphony"/> limit is exceeded.
		/// </summary>
		/// <value>True, if sound bus will be stopping the oldest playing sounds. False if the sound bus won't allow new sounds to play.</value>
		public bool AllowVoiceStealing
		{
			get { return allowVoiceStealing; }
			set { allowVoiceStealing = value; }
		}

		/// <summary>
		/// The time interval in seconds limiting consecutive sound bus plays.
		/// </summary>
		/// <value>Time interval in seconds. Zero means no limit.</value>
		public float PlayLimitInterval
		{
			get { return playLimitInterval; }
			set { playLimitInterval = value; }
		}

		/// <summary>
		/// The flag indicating whether the sound bank inspector should show advanced settings for the sound bus.
		/// </summary>
		/// <value>True, if advanced settings are shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the sound bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool Unfolded
		{
			get { return unfolded; }
			set { unfolded = value; }
		}
	}
}
