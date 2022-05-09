using System;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The persistent storage for sound effect audio data.
	/// </summary>
	[Serializable]
	public class SoundVariation
	{
		[SerializeField]
		private AudioClip clip = null;

		[SerializeField]
		private float fixedVolume = 1.0f;

		[SerializeField]
		private float fixedPitch = 1.0f;

		[SerializeField]
		private float fixedDelay = 0.0f;

		[SerializeField]
		private bool randomizeVolume = false;

		[SerializeField]
		private bool randomizePitch = false;

		[SerializeField]
		private bool randomizeDelay = false;

		[SerializeField]
		private Vector2 randomVolumeRange = Vector2.one;

		[SerializeField]
		private Vector2 randomPitchRange = Vector2.one;

		[SerializeField]
		private Vector2 randomDelayRange = Vector2.zero;

		/// <summary>
		/// The name of the sound variation.
		/// </summary>
		/// <value>Name of the current audio clip being used. Null reference otherwise.</value>
		public string Name
		{
			get { return (clip != null) ? clip.name : null; }
		}

		/// <summary>
		/// The volume of the sound variation.
		/// </summary>
		/// <value><see cref="SoundVariation.FixedVolume"/> value if <see cref="SoundVariation.RandomizeVolume"/> is false.
		/// Otherwise random volume value from <see cref="SoundVariation.RandomVolume"/> range.
		///</value>
		public float Volume
		{
			get { return GetValue(fixedVolume, randomizeVolume, randomVolumeRange); }
		}

		/// <summary>
		/// The pitch of the sound variation.
		/// </summary>
		/// <value><see cref="SoundVariation.FixedPitch"/> value if <see cref="SoundVariation.RandomizePitch"/> is false.
		/// Otherwise random pitch value from <see cref="SoundVariation.RandomPitch"/> range.
		///</value>
		public float Pitch
		{
			get { return GetValue(fixedPitch, randomizePitch, randomPitchRange); }
		}

		/// <summary>
		/// The delay of the sound variation.
		/// </summary>
		/// <value><see cref="SoundVariation.FixedDelay"/> value if <see cref="SoundVariation.RandomizeDelay"/> is false.
		/// Otherwise random delay value from <see cref="SoundVariation.RandomDelay"/> range.
		///</value>
		public float Delay
		{
			get { return GetValue(fixedDelay, randomizeDelay, randomDelayRange); }
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
		/// The fixed volume of the sound variation.
		/// </summary>
		/// <value>Fixed volume of the sound variation. Value must be in [0;1] range.</value>
		public float FixedVolume
		{
			get { return fixedVolume; }
			set { fixedVolume = value; }
		}

		/// <summary>
		/// The fixed pitch of the sound variation.
		/// </summary>
		/// <value>Fixed pitch of the souhnd variation. Value must be in [-3;3] range.</value>
		public float FixedPitch
		{
			get { return fixedPitch; }
			set { fixedPitch = value; }
		}

		/// <summary>
		/// The fixed delay of the sound variation.
		/// </summary>
		/// <value>Fixed delay in seconds of the sound variation. Value must be greater or equal to zero.</value>
		public float FixedDelay
		{
			get { return fixedDelay; }
			set { fixedDelay = value; }
		}

		/// <summary>
		/// The random volume range of the sound variation.
		/// </summary>
		/// <value>Random volume range of the sound variation. Vector components store range boundaries (x - min, y - max).</value>
		public Vector2 RandomVolume
		{
			get { return randomVolumeRange; }
			set { randomVolumeRange = value; }
		}

		/// <summary>
		/// The random pitch range of the sound variation.
		/// </summary>
		/// <value>Random pitch range of the sound variation. Vector components store range boundaries (x - min, y - max).</value>
		public Vector2 RandomPitch
		{
			get { return randomPitchRange; }
			set { randomPitchRange = value; }
		}

		/// <summary>
		/// The random delay range of the sound variation.
		/// </summary>
		/// <value>Random delay range of the sound variation. Vector components store range boundaries (x - min, y - max).</value>
		public Vector2 RandomDelay
		{
			get { return randomDelayRange; }
			set { randomDelayRange = value; }
		}

		/// <summary>
		/// The flag indicating which property is used for <see cref="SoundVariation.Volume"/>.
		/// </summary>
		/// <value>True, if <see cref="SoundVariation.RandomVolume"/> is used. Otherwise <see cref="SoundVariation.FixedVolume"/> is used.</value>
		public bool RandomizeVolume
		{
			get { return randomizeVolume; }
			set { randomizeVolume = value; }
		}

		/// <summary>
		/// The flag indicating which property is used for <see cref="SoundVariation.Pitch"/>.
		/// </summary>
		/// <value>True, if <see cref="SoundVariation.RandomPitch"/> is used. Otherwise <see cref="SoundVariation.FixedPitch"/> is used.</value>
		public bool RandomizePitch
		{
			get { return randomizePitch; }
			set { randomizePitch = value; }
		}

		/// <summary>
		/// The flag indicating which property is used for <see cref="SoundVariation.Delay"/>.
		/// </summary>
		/// <value>True, if <see cref="SoundVariation.RandomDelay"/> is used. Otherwise <see cref="SoundVariation.FixedDelay"/> is used.</value>
		public bool RandomizeDelay
		{
			get { return randomizeDelay; }
			set { randomizeDelay = value; }
		}

		private float GetValue(float value, bool randomize, Vector2 valueRange)
		{
			if (randomize)
				return UnityEngine.Random.Range(valueRange.x, valueRange.y);

			return value;
		}
	}
}
