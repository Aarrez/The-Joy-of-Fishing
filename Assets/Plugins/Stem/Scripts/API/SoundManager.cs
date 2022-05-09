using System.Collections.ObjectModel;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The main class for sound playback and bank management.
	/// </summary>
	public static class SoundManager
	{
		private static BankManager<SoundBank, SoundManagerRuntime> bankManager = new BankManager<SoundBank, SoundManagerRuntime>();
		private static SoundInstanceManagerRuntime instanceManagerRuntime = null;
		private static GameObject instanceManagerGameObject = null;
		private static bool shutdown = false;

		/// <summary>
		/// The collection of all registered sound banks.
		/// </summary>
		/// <value>A reference to a read-only collection of sound banks.</value>
		public static ReadOnlyCollection<SoundBank> Banks
		{
			get { return bankManager.Banks; }
		}

		/// <summary>
		/// The primary sound bank that will be searched first in case of name collisions.
		/// </summary>
		/// <value>A reference to a primary sound bank.</value>
		public static SoundBank PrimaryBank
		{
			get { return bankManager.PrimaryBank; }
			set { bankManager.PrimaryBank = value; }
		}

		/// <summary>
		/// Registers new sound bank.
		/// </summary>
		/// <param name="bank">A reference to a sound bank to register.</param>
		/// <returns>
		/// True, if sound bank was succesfully registered. False otherwise.
		/// </returns>
		public static bool RegisterBank(SoundBank bank)
		{
			return bankManager.RegisterBank(bank);
		}

		/// <summary>
		/// Deregisters existing sound bank.
		/// </summary>
		/// <param name="bank">A reference to a sound bank to deregister.</param>
		/// <returns>
		/// True, if sound bank was succesfully deregistered. False otherwise.
		/// </returns>
		public static bool DeregisterBank(SoundBank bank)
		{
			return bankManager.DeregisterBank(bank);
		}

		/// <summary>
		/// Plays one-shot sound in 3D space.
		/// </summary>
		/// <param name="id">ID referring to the sound.</param>
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
		public static void Play3D(ID id, Vector3 position, int? variationIndex = null, float? volume = null, float? pitch = null, float? delay = null)
		{
			if (shutdown)
				return;

			SoundInstance instance = FetchSoundInstance(id);
			if (instance == null)
				return;

			instance.Play3D(position, variationIndex, volume, pitch, delay);
		}

		/// <summary>
		/// Plays one-shot sound in 3D space.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="position">Position of the sound.</param>
		/// <param name="variationIndex">Variation index. Must be within <see cref="Stem.Sound.Variations"/> range.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <param name="delay">Delay of the sound. Value must be greater or equal to zero.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, the primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>Non-null variation index parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Non-null volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Non-null pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// <para>Non-null delay parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Delay"/> value.</para>
		/// </remarks>
		public static void Play3D(string name, Vector3 position, int? variationIndex = null, float? volume = null, float? pitch = null, float? delay = null)
		{
			if (shutdown)
				return;

			SoundInstance instance = FetchSoundInstance(name);
			if (instance == null)
				return;

			instance.Play3D(position, variationIndex, volume, pitch, delay);
		}

		/// <summary>
		/// Plays one-shot sound.
		/// </summary>
		/// <param name="id">ID referring to the sound.</param>
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
		public static void Play(ID id, int? variationIndex = null, float? volume = null, float? pitch = null, float? delay = null)
		{
			Play3D(id, Vector3.zero, variationIndex, volume, pitch, delay);
		}

		/// <summary>
		/// Plays one-shot sound.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="variationIndex">Variation index. Must be within <see cref="Stem.Sound.Variations"/> range.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <param name="delay">Delay of the sound. Value must be greater or equal to zero.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, the primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>Audio source position will be set to (0, 0, 0). Make sure <see cref="Stem.Sound"/>.<see cref="Stem.Sound.SpatialBlend"/> is zero, otherwise it may not be mixed correctly.</para>
		/// <para>Non-null variation index parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Non-null volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Non-null pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// <para>Non-null delay parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Delay"/> value.</para>
		/// </remarks>
		public static void Play(string name, int? variationIndex = null, float? volume = null, float? pitch = null, float? delay = null)
		{
			Play3D(name, Vector3.zero, variationIndex, volume, pitch, delay);
		}

		/// <summary>
		/// Searches for the specified sound with a matching unique ID.
		/// </summary>
		/// <param name="id">ID referring to the sound.</param>
		/// <returns>
		/// A reference to a sound, if found. Null reference otherwise.
		/// </returns>
		public static Sound GetSound(ID id)
		{
			if (shutdown)
				return null;

			Sound sound = FetchSound(id);
			if (sound == null)
			{
				Debug.LogWarningFormat("SoundManager.GetSound(): can't find sound, ID: {0}", id);
				return null;
			}

			return sound;
		}

		/// <summary>
		/// Searches for the specified sound with a matching name.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <returns>
		/// A reference to a sound, if found. Null reference otherwise.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, the primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// </remarks>
		public static Sound GetSound(string name)
		{
			if (shutdown)
				return null;

			Sound sound = FetchSound(name);
			if (sound == null)
			{
				Debug.LogWarningFormat("SoundManager.GetSound(): can't find \"{0}\" sound", name);
				return null;
			}

			return sound;
		}

		/// <summary>
		/// Searches for the specified sound bus with a matching unique ID.
		/// </summary>
		/// <param name="id">ID referring to the sound bus.</param>
		/// <returns>
		/// A reference to a sound bus, if found. Null reference otherwise.
		/// </returns>
		public static SoundBus GetSoundBus(ID id)
		{
			if (shutdown)
				return null;

			SoundBus bus = FetchSoundBus(id);
			if (bus == null)
			{
				Debug.LogWarningFormat("SoundManager.GetSoundBus(): can't find sound bus, ID: {0}", id);
				return null;
			}

			return bus;
		}

		/// <summary>
		/// Searches for the specified sound bus with a matching name.
		/// </summary>
		/// <param name="name">Name of the sound bus.</param>
		/// <returns>
		/// A reference to a sound bus, if found. Null reference otherwise.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have sound buses with a matching name, the primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound bus will be used.</para>
		/// </remarks>
		public static SoundBus GetSoundBus(string name)
		{
			if (shutdown)
				return null;

			SoundBus bus = FetchSoundBus(name);
			if (bus == null)
			{
				Debug.LogWarningFormat("SoundManager.GetSoundBus(): can't find \"{0}\" sound bus", name);
				return null;
			}

			return bus;
		}

		/// <summary>
		/// Grabs an empty sound instance from the sound pool. Used for manual playback and custom mixing logic.
		/// </summary>
		/// <returns>
		/// A reference to an empty sound instance.
		/// </returns>
		/// <remarks>
		/// <para>This method may increase the size of the sound pool causing additional memory allocations.</para>
		/// <para>When a sound instance is not needed anymore, use <see cref="ReleaseSound(SoundInstance)"/> to return it back to the sound pool.</para>
		/// </remarks>
		public static SoundInstance GrabSound()
		{
			if (shutdown)
				return null;

			SoundInstanceManagerRuntime runtime = FetchSoundPool();
			if (runtime == null)
			{
				Debug.LogWarningFormat("SoundManager.GrabSound(): can't create sound pool");
				return null;
			}

			return runtime.GrabSound();
		}

		/// <summary>
		/// Grabs a sound instance from the sound pool. Used for manual playback and custom mixing logic.
		/// </summary>
		/// <param name="id">ID referring to the sound.</param>
		/// <returns>
		/// A reference to a sound instance.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, the primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>This method may increase the size of the sound pool causing additional memory allocations.</para>
		/// <para>When a sound instance is not needed anymore, use <see cref="ReleaseSound(SoundInstance)"/> to return it back to the sound pool.</para>
		/// </remarks>
		public static SoundInstance GrabSound(ID id)
		{
			if (shutdown)
				return null;

			SoundInstanceManagerRuntime runtime = FetchSoundPool();
			if (runtime == null)
			{
				Debug.LogWarningFormat("SoundManager.GrabSound(): can't create sound pool");
				return null;
			}

			Sound sound = FetchSound(id);
			if (sound == null)
			{
				Debug.LogWarningFormat("SoundManager.GrabSound(): can't find sound, ID: {0}", id);
				return null;
			}

			return runtime.GrabSound(sound);
		}

		/// <summary>
		/// Grabs a sound instance from the sound pool. Used for manual playback and custom mixing logic.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <returns>
		/// A reference to a sound instance.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, the primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>This method may increase the size of the sound pool causing additional memory allocations.</para>
		/// <para>When a sound instance is not needed anymore, use <see cref="ReleaseSound(SoundInstance)"/> to return it back to the sound pool.</para>
		/// </remarks>
		public static SoundInstance GrabSound(string name)
		{
			if (shutdown)
				return null;

			SoundInstanceManagerRuntime runtime = FetchSoundPool();
			if (runtime == null)
			{
				Debug.LogWarningFormat("SoundManager.GrabSound(): can't create sound pool");
				return null;
			}

			Sound sound = FetchSound(name);
			if (sound == null)
			{
				Debug.LogWarningFormat("SoundManager.GrabSound(): can't find \"{0}\" sound", name);
				return null;
			}

			return runtime.GrabSound(sound);
		}

		/// <summary>
		/// Releases sound instance and return it back to the sound pool.
		/// </summary>
		/// <param name="instance">A reference to a sound instance.</param>
		/// <returns>
		/// True, if the sound instance was successfully returned to sound pool. False otherwise.
		/// </returns>
		/// <remarks>
		/// <para>Once the sound instance is returned back to a sound pool, it's possible to reuse it again by calling <see cref="GrabSound()"/> or <see cref="GrabSound(string)"/>.</para>
		/// </remarks>
		public static bool ReleaseSound(SoundInstance instance)
		{
			if (shutdown)
				return false;

			SoundInstanceManagerRuntime runtime = FetchSoundPool();
			if (runtime == null)
			{
				Debug.LogWarningFormat("SoundManager.ReleaseSound(): can't create sound pool");
				return false;
			}

			return runtime.ReleaseSound(instance);
		}

		/// <summary>
		/// Stops all playing sounds.
		/// </summary>
		/// <remarks>
		/// <para>This method will also stop all sounds instances returned from <see cref="GrabSound()"/> or <see cref="GrabSound(string)"/>.</para>
		/// </remarks>
		public static void Stop()
		{
			if (shutdown)
				return;

			ReadOnlyCollection<SoundManagerRuntime> runtimes = bankManager.Runtimes;
			for (int i = 0; i < runtimes.Count; i++)
				runtimes[i].Stop();

			if (instanceManagerRuntime != null)
				instanceManagerRuntime.Stop();
		}

		/// <summary>
		/// Pauses all playing sounds.
		/// </summary>
		/// <remarks>
		/// <para>This method will also stop all sounds instances returned from <see cref="Stem.SoundManager.GrabSound()"/> or <see cref="Stem.SoundManager.GrabSound(string)"/>.</para>
		/// </remarks>
		public static void Pause()
		{
			if (shutdown)
				return;

			ReadOnlyCollection<SoundManagerRuntime> runtimes = bankManager.Runtimes;
			for (int i = 0; i < runtimes.Count; i++)
				runtimes[i].Pause();

			if (instanceManagerRuntime != null)
				instanceManagerRuntime.Pause();
		}

		/// <summary>
		/// Resumes all paused sounds.
		/// </summary>
		/// <remarks>
		/// <para>This method will also resume all sounds instances returned from <see cref="Stem.SoundManager.GrabSound()"/> or <see cref="Stem.SoundManager.GrabSound(string)"/>.</para>
		/// </remarks>
		public static void UnPause()
		{
			if (shutdown)
				return;

			ReadOnlyCollection<SoundManagerRuntime> runtimes = bankManager.Runtimes;
			for (int i = 0; i < runtimes.Count; i++)
				runtimes[i].UnPause();

			if (instanceManagerRuntime != null)
				instanceManagerRuntime.UnPause();
		}

		internal static void Init()
		{
			for (int i = 0; i < bankManager.Banks.Count; i++)
				bankManager.FetchRuntime(i);

			shutdown = false;
		}

		internal static void Shutdown()
		{
			shutdown = true;
		}

		internal static SoundManagerRuntime FetchSoundManagerRuntime(ID id)
		{
			// Check all banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				SoundBank bank = bankManager.Banks[i];
				Sound sound = bank.GetSound(id);

				if (sound != null)
					return bankManager.FetchRuntime(i);
			}

			return null;
		}

		internal static SoundManagerRuntime FetchSoundManagerRuntime(string name)
		{
			// Check primary bank first
			int primaryIndex = bankManager.PrimaryBankIndex;
			if (primaryIndex != -1)
			{
				SoundBank bank = bankManager.Banks[primaryIndex];

				if (bank.Runtime.ContainsSound(name))
					return bankManager.FetchRuntime(primaryIndex);
			}

			// Check other banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				// Skip primary bank
				if (i == primaryIndex)
					continue;

				SoundBank bank = bankManager.Banks[i];

				if (bank.Runtime.ContainsSound(name))
					return bankManager.FetchRuntime(i);
			}

			return null;
		}

		internal static SoundInstance FetchSoundInstance(ID id)
		{
			SoundManagerRuntime runtime = FetchSoundManagerRuntime(id);
			if (runtime == null)
			{
				Debug.LogWarningFormat("SoundManager.FetchSoundInstance(): can't find sound, ID: {0}", id);
				return null;
			}

			Sound sound = runtime.Bank.GetSound(id);
			return runtime.FetchSoundInstance(sound);
		}

		internal static SoundInstance FetchSoundInstance(string name)
		{
			SoundManagerRuntime runtime = FetchSoundManagerRuntime(name);
			if (runtime == null)
			{
				Debug.LogWarningFormat("SoundManager.FetchSoundInstance(): can't find \"{0}\" sound", name);
				return null;
			}

			Sound sound = runtime.Bank.GetSound(name);
			return runtime.FetchSoundInstance(sound);
		}

		internal static Sound FetchSound(ID id)
		{
			// Check all banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				SoundBank bank = bankManager.Banks[i];
				Sound sound = bank.GetSound(id);

				if (sound != null)
					return sound;
			}

			return null;
		}

		internal static Sound FetchSound(string name)
		{
			// Check primary bank first
			int primaryIndex = bankManager.PrimaryBankIndex;
			if (primaryIndex != -1)
			{
				SoundBank bank = bankManager.Banks[primaryIndex];

				if (bank.Runtime.ContainsSound(name))
					return bank.Runtime.GetSound(name);
			}

			// Check other banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				// Skip primary bank
				if (i == primaryIndex)
					continue;

				SoundBank bank = bankManager.Banks[i];

				if (bank.Runtime.ContainsSound(name))
					return bank.Runtime.GetSound(name);
			}

			return null;
		}

		internal static SoundBus FetchSoundBus(ID id)
		{
			// Check all banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				SoundBank bank = bankManager.Banks[i];
				SoundBus bus = bank.GetSoundBus(id);

				if (bus != null)
					return bus;
			}

			return null;
		}

		internal static SoundBus FetchSoundBus(string name)
		{
			// Check primary bank first
			int primaryIndex = bankManager.PrimaryBankIndex;
			if (primaryIndex != -1)
			{
				SoundBank bank = bankManager.Banks[primaryIndex];

				if (bank.Runtime.ContainsSoundBus(name))
					return bank.Runtime.GetSoundBus(name);
			}

			// Check other banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				// Skip primary bank
				if (i == primaryIndex)
					continue;

				SoundBank bank = bankManager.Banks[i];

				if (bank.Runtime.ContainsSoundBus(name))
					return bank.Runtime.GetSoundBus(name);
			}

			return null;
		}

		private static SoundInstanceManagerRuntime FetchSoundPool()
		{
			if (instanceManagerRuntime != null)
				return instanceManagerRuntime;

			instanceManagerGameObject = new GameObject();
			instanceManagerGameObject.name = "Sound Instance Pool";
			GameObject.DontDestroyOnLoad(instanceManagerGameObject);

			instanceManagerRuntime = instanceManagerGameObject.AddComponent<SoundInstanceManagerRuntime>();
			return instanceManagerRuntime;
		}
	}
}
