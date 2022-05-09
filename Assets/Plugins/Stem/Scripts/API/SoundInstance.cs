using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The game object with audio source component. Used for manual playback and custom mixing logic.
	/// </summary>
	public class SoundInstance
	{
		private GameObject gameObject = null;
		private Transform transform = null;
		private Transform target = null;
		private AudioSource source = null;
		private Sound sound = null;
		private bool looped = false;
		private bool paused = false;
		private float volume = 0.0f;
		private float pitch = 1.0f;
		private float delay = 0.0f;

		internal SoundInstance(Sound sound_, string name, Transform root)
		{
			sound = sound_;

			gameObject = new GameObject();
			gameObject.name = name;
			gameObject.transform.parent = root;

			transform = gameObject.transform;

			source = gameObject.AddComponent<AudioSource>();
			source.playOnAwake = false;
		}

		/// <summary>
		/// The transform component to which sound instance is attached.
		/// </summary>
		/// <value>A reference a transform component.</value>
		/// <remarks>
		/// <para>Once set, it will override <see cref="SoundInstance.Position"/> property value.</para>
		/// </remarks>
		public Transform Target
		{
			get { return target; }
			set { target = value; }
		}

		/// <summary>
		/// The position of the sound instance in world space.
		/// </summary>
		/// <value>A world space coordinates of the sound instance.</value>
		/// <remarks>
		/// <para>Non-null <see cref="SoundInstance.Target"/> will override this property value.</para>
		/// </remarks>
		public Vector3 Position
		{
			get { return transform.position; }
			set { transform.position = value; }
		}

		/// <summary>
		/// The flag indicating that the sound instance is paused.
		/// </summary>
		/// <value>True, if sound instance is paused. False otherwise.</value>
		public bool Paused
		{
			get { return paused; }
		}

		/// <summary>
		/// The flag indicating that the sound instance is playing.
		/// </summary>
		/// <value>True, if sound instance is playing. False otherwise.</value>
		public bool Playing
		{
			get { return source.isPlaying; }
		}

		/// <summary>
		/// The playback position in samples.
		/// </summary>
		/// <value>An offset in samples from the start of an audio clip.</value>
		public int TimeSamples
		{
			get { return source.timeSamples; }
		}

		/// <summary>
		/// The reference to a sound which will be used for playback. Changing this value allows playing different sounds.
		/// </summary>
		/// <value>A reference to a sound.</value>
		public Sound Sound
		{
			get { return sound; }
			set { sound = value; }
		}

		/// <summary>
		/// The flag indicating that the sound instance is looping. Set whether it should replay the audio clip after it finishes.
		/// </summary>
		/// <value>True, if sound instance is looping. False otherwise.</value>
		public bool Looped
		{
			get { return looped; }
			set { looped = value; }
		}

		/// <summary>
		/// The volume property allows controlling the overall level of sound coming to the audio source.
		/// </summary>
		/// <value>Volume value in [0;1] range.</value>
		public float Volume
		{
			get { return volume; }
			set { volume = value; }
		}

		/// <summary>
		/// The pitch property allows controlling how high or low the tone of the audio source is.
		/// </summary>
		/// <value>Pitch value in [3;3] range.</value>
		public float Pitch
		{
			get { return pitch; }
			set { pitch = value; }
		}

		/// <summary>
		/// Plays sound in 3D space.
		/// </summary>
		/// <param name="position">Position of the sound.</param>
		/// <param name="variationIndex">Variation index. Must be within <see cref="Stem.Sound.Variations"/> range.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <param name="delay">Delay of the sound. Value must be greater or equal to zero.</param>
		/// <remarks>
		/// <para>Non-null variation index parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Non-null volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Non-null pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// <para>Non-null delay parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Delay"/> value.</para>
		/// </remarks>
		public void Play3D(Vector3 position, int? variationIndex = null, float? volume = null, float? pitch = null, float? delay = null)
		{
			SoundVariation variation = FetchVariation(variationIndex);
			if (variation == null)
				return;

			float desiredVolume = volume ?? variation.Volume;
			float desiredPitch = pitch ?? variation.Pitch;
			float desiredDelay = delay ?? variation.Delay;

			PlayInternal(position, variation.Clip, desiredVolume, desiredPitch, desiredDelay);
		}

		/// <summary>
		/// Plays sound.
		/// </summary>
		/// <param name="variationIndex">Variation index. Must be within <see cref="Stem.Sound.Variations"/> range.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <param name="delay">Delay of the sound. Value must be greater or equal to zero.</param>
		/// <remarks>
		/// <para>Audio source position will be set to (0, 0, 0). Make sure <see cref="Stem.Sound"/>.<see cref="Stem.Sound.SpatialBlend"/> is zero, otherwise it may not be mixed correctly.</para>
		/// <para>Non-null variation index parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Non-null volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Non-null pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// <para>Non-null delay parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Delay"/> value.</para>
		/// </remarks>
		public void Play(int? variationIndex = null, float? volume = null, float? pitch = null, float? delay = null)
		{
			Play3D(Vector3.zero, variationIndex, volume, pitch, delay);
		}

		/// <summary>
		/// Stops sound.
		/// </summary>
		public void Stop()
		{
			paused = false;
			source.Stop();
		}

		/// <summary>
		/// Pauses sound.
		/// </summary>
		public void Pause()
		{
			paused = true;
			source.Pause();
		}

		/// <summary>
		/// Resumes sound.
		/// </summary>
		public void UnPause()
		{
			paused = false;
			source.UnPause();
		}

		internal void Update()
		{
			if (sound == null)
				return;

			if (target != null)
				transform.position = target.position;

			SoundBus bus = sound.Bus;

			source.mute = !sound.Audible;
			source.volume = volume * sound.Volume * bus.Volume;
			source.pitch = pitch;
		}

		internal void Release()
		{
			paused = false;
			source.Stop();

			MemoryManager.Release(sound, source.clip);
			source.clip = null;
			sound = null;
		}

		private SoundVariation FetchVariation(int? variationIndex)
		{
			if (sound == null)
				return null;

			if (sound.Variations.Count == 0)
				return null;

			if (variationIndex != null)
			{
				if (variationIndex.Value < 0 || variationIndex.Value >= sound.Variations.Count)
				{
					Debug.LogWarningFormat("SoundInstance.FetchVariation(): can't find variation for \"{0}\" with index {1}", sound.Name, variationIndex.Value);
					return null;
				}

				return sound.Variations[variationIndex.Value];
			}

			return sound.FetchVariation();
		}

		private void PlayInternal(Vector3 position, AudioClip clip, float volume_, float pitch_, float delay_)
		{
			paused = false;

			volume = volume_;
			pitch = pitch_;
			delay = delay_;

			SoundBus bus = sound.Bus;

			MemoryManager.Release(sound, source.clip);

			source.clip = clip;
			source.volume = volume * sound.Volume * bus.Volume;
			source.pitch = pitch;
			source.loop = looped;
			source.outputAudioMixerGroup = bus.MixerGroup;

			source.spatialBlend = sound.SpatialBlend;
			source.panStereo = sound.PanStereo;
			source.dopplerLevel = sound.DopplerLevel;
			source.spread = sound.Spread;
			source.rolloffMode = (AudioRolloffMode)sound.AttenuationMode;
			source.minDistance = sound.MinDistance;
			source.maxDistance = sound.MaxDistance;

			MemoryManager.Grab(sound, clip);

			transform.localPosition = position;

			source.PlayDelayed(delay);
		}
	}
}
