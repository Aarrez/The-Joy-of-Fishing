using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// A sound bank callback function, called after adding sound to the bank.
	/// </summary>
	/// <param name="sound">A reference to a newly added sound.</param>
	public delegate void SoundAddedDelegate(Sound sound);

	/// <summary>
	/// A sound bank callback function, called before removing the sound from the bank.
	/// </summary>
	/// <param name="sound">A reference to a sound to be removed.</param>
	/// <param name="index">An index in corresponding <see cref="SoundBank.Sounds"/> collection.</param>
	public delegate void SoundRemovedDelegate(Sound sound, int index);

	/// <summary>
	/// A sound bank callback function, called after changing sound name.
	/// </summary>
	/// <param name="sound">A reference to a sound.</param>
	/// <param name="index">An index in corresponding <see cref="SoundBank.Sounds"/> collection.</param>
	/// <param name="oldName">An old name of the sound.</param>
	/// <param name="newName">A new name of the sound.</param>
	public delegate void SoundRenamedDelegate(Sound sound, int index, string oldName, string newName);

	/// <summary>
	/// A sound bank callback function, called after adding sound bus to the bank.
	/// </summary>
	/// <param name="bus">A reference to a newly added sound bus.</param>
	public delegate void SoundBusAddedDelegate(SoundBus bus);

	/// <summary>
	/// A sound bank callback function, called before removing the sound bus from the bank.
	/// </summary>
	/// <param name="bus">A reference to a sound bus to be removed.</param>
	/// <param name="index">An index in corresponding <see cref="SoundBank.Buses"/> collection.</param>
	public delegate void SoundBusRemovedDelegate(SoundBus bus, int index);

	/// <summary>
	/// A sound bank callback function, called after changing sound bus name.
	/// </summary>
	/// <param name="bus">A reference to a sound bus.</param>
	/// <param name="index">An index in corresponding <see cref="SoundBank.Buses"/> collection.</param>
	/// <param name="oldName">An old name of the sound bus.</param>
	/// <param name="newName">A new name of the sound bus.</param>
	public delegate void SoundBusRenamedDelegate(SoundBus bus, int index, string oldName, string newName);

	/// <summary>
	/// The attribute class used to make an int variable in a script be restricted to a sound id.
	/// </summary>
	/// <remarks>
	/// When this attribute is used, the variable will be shown as two dropdown fields in the inspector instead of the default number field.
	/// </remarks>
	public class SoundIDAttribute : PropertyAttribute { }

	/// <summary>
	/// The attribute class used to make an int variable in a script be restricted to a sound bus id.
	/// </summary>
	/// <remarks>
	/// When this attribute is used, the variable will be shown as two dropdown fields in the inspector instead of the default number field.
	/// </remarks>
	public class SoundBusIDAttribute : PropertyAttribute { }

	/// <summary>
	/// The persistent storage for sounds and sound buses.
	/// </summary>
	[CreateAssetMenu(fileName = "New Sound Bank", menuName = "Stem/Sound Bank", order = 1002)]
	public class SoundBank : ScriptableObject, IBank, ISerializationCallbackReceiver
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
		private AudioClipImportMode soundBatchImportMode = AudioClipImportMode.SingleItemWithAllClips;

		[SerializeField]
		private bool showSounds = true;

		[SerializeField]
		private bool showSoundBuses = true;

		[SerializeField]
		private List<Sound> sounds = new List<Sound>();
		private ReadOnlyCollection<Sound> soundsRO = null;

		[SerializeField]
		private List<SoundBus> buses = new List<SoundBus>();
		private ReadOnlyCollection<SoundBus> busesRO = null;

		[SerializeField]
		private int[] busIndices = null;

		[SerializeField]
		private AudioClipManagementMode soundManagementMode = AudioClipManagementMode.UnloadUnused;

		[SerializeField]
		private float soundUnloadInterval = 60.0f;

		[NonSerialized]
		private SoundBankRuntime runtime = new SoundBankRuntime();

		/// <summary>
		/// The delegate informing about adding sounds.
		/// </summary>
		public event SoundAddedDelegate OnSoundAdded;

		/// <summary>
		/// The delegate informing about removing sounds.
		/// </summary>
		public event SoundRemovedDelegate OnSoundRemoved;

		/// <summary>
		/// The delegate informing about the change of sound names.
		/// </summary>
		public event SoundRenamedDelegate OnSoundRenamed;

		/// <summary>
		/// The delegate informing about adding sound buses.
		/// </summary>
		public event SoundBusAddedDelegate OnSoundBusAdded;

		/// <summary>
		/// The delegate informing about removing sound buses.
		/// </summary>
		public event SoundBusRemovedDelegate OnSoundBusRemoved;

		/// <summary>
		/// The delegate informing about the change of sound bus names.
		/// </summary>
		public event SoundBusRenamedDelegate OnSoundBusRenamed;

		/// <summary>
		/// Prepares sound bank for serialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during serialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnBeforeSerialize()
		{
			if (buses.Count == 0)
				AddSoundBus("Master");

			if (sounds.Count > 0)
			{
				busIndices = new int[sounds.Count];
				for (int i = 0; i < sounds.Count; i++)
				{
					SoundBus bus = sounds[i].Bus;
					busIndices[i] = buses.IndexOf(bus);
				}
			}

			if (guidA == 0 && guidB == 0 && guidC == 0 && guidD == 0)
				RegenerateBankID();
		}

		/// <summary>
		/// Prepares sound bank for runtime use after deserialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during deserialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnAfterDeserialize()
		{
			if (guidA == 0 && guidB == 0 && guidC == 0 && guidD == 0)
				RegenerateBankID();

			runtime = new SoundBankRuntime();
			for (int i = 0; i < sounds.Count; i++)
			{
				Sound sound = sounds[i];
				SoundBus bus = null;
				if (busIndices != null)
				{
					int index = busIndices[i];
					bus = (index != -1) ? buses[index] : null;
				}
				sound.Bus = bus;
				sound.Bank = this;

				runtime.AddSound(sound);
			}

			foreach (SoundBus bus in buses)
			{
				bus.Bank = this;
				runtime.AddSoundBus(bus);
			}
		}

		/// <summary>
		/// Returns sound bank <see cref="Stem.ID"/>.
		/// </summary>
		/// <returns>
		/// <para>An ID value.</para>
		/// </returns>
		public ID GetBankID()
		{
			return new ID(guidA, guidB, guidC, guidD, 0);
		}

		/// <summary>
		/// Generates a new unique <see cref="Stem.ID"/> for the sound bank.
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
		/// The collection of sounds.
		/// </summary>
		/// <value>A reference to a read-only collection of sounds.</value>
		public ReadOnlyCollection<Sound> Sounds
		{
			get
			{
				if (soundsRO == null)
					soundsRO = sounds.AsReadOnly();

				return soundsRO;
			}
		}

		/// <summary>
		/// The collection of sound buses.
		/// </summary>
		/// <value>A reference to a read-only collection of sound buses.</value>
		public ReadOnlyCollection<SoundBus> Buses
		{
			get
			{
				if (busesRO == null)
					busesRO = buses.AsReadOnly();

				return busesRO;
			}
		}

		/// <summary>
		/// The sound bus which will be used by default on newly created sounds.
		/// </summary>
		/// <value>A reference to a sound bus.</value>
		public SoundBus DefaultBus
		{
			get { return buses[0]; }
		}

		/// <summary>
		/// The flag indicating whether the sound bank inspector should show the 'Sounds' group.
		/// </summary>
		/// <value>True, if the 'Sounds' group is shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the sound bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool ShowSounds
		{
			get { return showSounds; }
			set { showSounds = value; }
		}

		/// <summary>
		/// The flag indicating whether the sound bank inspector should show the 'Buses' group.
		/// </summary>
		/// <value>True, if the 'Buses' group is shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the sound bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool ShowSoundBuses
		{
			get { return showSoundBuses; }
			set { showSoundBuses = value; }
		}

		/// <summary>
		/// The batch import mode defining how new sounds will be created after the drag-drop event.
		/// </summary>
		/// <value>An enum value.</value>
		public AudioClipImportMode SoundBatchImportMode
		{
			get { return soundBatchImportMode; }
			set { soundBatchImportMode = value; }
		}

		/// <summary>
		/// The default audio clip management mode for all sounds of the sound bank.
		/// </summary>
		/// <remarks>
		/// <para>By default, all sounds will use this value for audio clip management, however, it can be overridden by the <see cref="Sound.OverrideAudioClipManagement"/> flag.</para>
		/// </remarks>
		/// <value>An enum value.</value>
		public AudioClipManagementMode SoundManagementMode
		{
			get { return soundManagementMode; }
			set { soundManagementMode = value; }
		}

		/// <summary>
		/// The default audio clip unload interval for all sounds of the the sound bank.
		/// </summary>
		/// <remarks>
		/// <para>By default, all sounds will use this value for audio clip unload interval, however, it can be overridden by the <see cref="Sound.OverrideAudioClipManagement"/> flag.</para>
		/// </remarks>
		/// <value>A time interval in seconds.</value>
		public float SoundUnloadInterval
		{
			get { return soundUnloadInterval; }
			set { soundUnloadInterval = value; }
		}

		internal SoundBankRuntime Runtime
		{
			get { return runtime; }
		}

		/// <summary>
		/// Searches for the specified sound by <see cref="Stem.ID"/>.
		/// </summary>
		/// <param name="id">ID that refers to a sound.</param>
		/// <returns>
		/// A reference to a sound, if found. Null reference otherwise.
		/// </returns>
		public Sound GetSound(ID id)
		{
			if (id.BankGuidA != guidA)
				return null;

			if (id.BankGuidB != guidB)
				return null;

			if (id.BankGuidC != guidC)
				return null;

			if (id.BankGuidD != guidD)
				return null;

			return runtime.GetSound(id.ItemId);
		}

		/// <summary>
		/// Searches for the specified sound with a matching name.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <returns>
		/// A reference to a sound, if found. Null reference otherwise.
		/// </returns>
		public Sound GetSound(string name)
		{
			return runtime.GetSound(name);
		}

		/// <summary>
		/// Gets an <see cref="Stem.ID"/> to the specific sound.
		/// </summary>
		/// <param name="index">Zero-based index of the sound in the current sound bank.</param>
		/// <returns>
		/// An <see cref="Stem.ID"/> to the specific sound.
		/// </returns>
		public ID GetSoundID(int index)
		{
			if (index >= sounds.Count)
				return ID.None;

			return new ID(guidA, guidB, guidC, guidD, sounds[index].ID);
		}

		/// <summary>
		/// Adds an empty sound to the sound bank.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <returns>
		/// A reference to a newly created sound.
		/// </returns>
		public Sound AddSound(string name)
		{
			Sound sound = new Sound(this, name);
			sound.Bus = DefaultBus;

			sounds.Add(sound);
			runtime.AddSound(sound);

			if (OnSoundAdded != null)
				OnSoundAdded(sound);

			return sound;
		}

		/// <summary>
		/// Adds a sound with a single variation to the sound bank.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="clip">A reference to the audio clip with audio data.</param>
		/// <returns>
		/// A reference to a newly created sound.
		/// </returns>
		public Sound AddSound(string name, AudioClip clip)
		{
			Sound sound = new Sound(this, name);
			sound.Bus = DefaultBus;
			sound.AddVariation(clip);

			sounds.Add(sound);
			runtime.AddSound(sound);

			if (OnSoundAdded != null)
				OnSoundAdded(sound);

			return sound;
		}

		/// <summary>
		/// Adds a sound with multiple variations to the sound bank.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="clips">An array of audio clips with audio data.</param>
		/// <returns>
		/// A reference to a newly created sound.
		/// </returns>
		public Sound AddSound(string name, AudioClip[] clips)
		{
			Sound sound = new Sound(this, name);
			sound.Bus = DefaultBus;
			sound.AddVariations(clips);

			sounds.Add(sound);
			runtime.AddSound(sound);

			if (OnSoundAdded != null)
				OnSoundAdded(sound);

			return sound;
		}

		/// <summary>
		/// Removes existing sound from the sound bank.
		/// </summary>
		/// <param name="sound">A reference to a sound.</param>
		/// <remarks>
		/// <para>This method does nothing if the sound was not found in the sound bank.</para>
		/// </remarks>
		public void RemoveSound(Sound sound)
		{
			int index = sounds.IndexOf(sound);
			if (index == -1)
				return;

			if (OnSoundRemoved != null)
				OnSoundRemoved(sound, index);

			sounds.RemoveAt(index);
			runtime.RemoveSound(sound);
		}

		/// <summary>
		/// Searches for the specified sound bus by <see cref="Stem.ID"/>.
		/// </summary>
		/// <param name="id">ID that refers to a sound bus.</param>
		/// <returns>
		/// A reference to a sound bus, if found. Null reference otherwise.
		/// </returns>
		public SoundBus GetSoundBus(ID id)
		{
			if (id.BankGuidA != guidA)
				return null;

			if (id.BankGuidB != guidB)
				return null;

			if (id.BankGuidC != guidC)
				return null;

			if (id.BankGuidD != guidD)
				return null;

			return runtime.GetSoundBus(id.ItemId);
		}

		/// <summary>
		/// Searches for the specified sound bus with a matching name.
		/// </summary>
		/// <param name="name">Name of the sound bus.</param>
		/// <returns>
		/// A reference to a sound bus, if found. Null reference otherwise.
		/// </returns>
		public SoundBus GetSoundBus(string name)
		{
			return runtime.GetSoundBus(name);
		}

		/// <summary>
		/// Gets an <see cref="Stem.ID"/> to the specific sound bus.
		/// </summary>
		/// <param name="index">Zero-based index of the sound bus in the current sound bank.</param>
		/// <returns>
		/// An <see cref="Stem.ID"/> to the specific sound bus.
		/// </returns>
		public ID GetSoundBusID(int index)
		{
			if (index >= buses.Count)
				return ID.None;

			return new ID(guidA, guidB, guidC, guidD, buses[index].ID);
		}

		/// <summary>
		/// Adds a new sound bus to the sound bank.
		/// </summary>
		/// <param name="name">Name of the sound bus.</param>
		/// <returns>
		/// A reference to a newly created sound bus.
		/// </returns>
		public SoundBus AddSoundBus(string name)
		{
			SoundBus bus = new SoundBus(this, name);

			buses.Add(bus);
			runtime.AddSoundBus(bus);

			if (OnSoundBusAdded != null)
				OnSoundBusAdded(bus);

			return bus;
		}

		/// <summary>
		/// Removes existing sound bus from the sound bank.
		/// </summary>
		/// <param name="bus">A reference to a sound bus.</param>
		/// <remarks>
		/// <para>This method does nothing if the sound bus was not found in the sound bank.</para>
		/// </remarks>
		public void RemoveSoundBus(SoundBus bus)
		{
			if (buses.Count == 1)
				return;

			int index = buses.IndexOf(bus);
			if (index == -1)
				return;

			if (OnSoundBusRemoved != null)
				OnSoundBusRemoved(bus, index);

			foreach (Sound sound in sounds)
				if (sound.Bus == bus)
					sound.Bus = DefaultBus;

			buses.RemoveAt(index);
			runtime.RemoveSoundBus(bus);
		}

		internal void AddSound(Sound sound)
		{
			if (sounds.Contains(sound))
				return;

			sounds.Add(sound);
			runtime.AddSound(sound);

			if (OnSoundAdded != null)
				OnSoundAdded(sound);
		}

		internal void RenameSound(Sound sound, string oldName, string newName)
		{
			int index = sounds.IndexOf(sound);
			if (index == -1)
				return;

			runtime.RenameSound(sound, oldName, newName);

			if (OnSoundRenamed != null)
				OnSoundRenamed(sound, index, oldName, newName);
		}

		internal void AddSoundBus(SoundBus bus)
		{
			if (buses.Contains(bus))
				return;

			buses.Add(bus);
			runtime.AddSoundBus(bus);

			if (OnSoundBusAdded != null)
				OnSoundBusAdded(bus);
		}

		internal void RenameSoundBus(SoundBus bus, string oldName, string newName)
		{
			int index = buses.IndexOf(bus);
			if (index == -1)
				return;

			runtime.RenameSoundBus(bus, oldName, newName);

			if (OnSoundBusRenamed != null)
				OnSoundBusRenamed(bus, index, oldName, newName);
		}

		internal void OnDestroy()
		{
			if (SoundManager.Banks.Contains(this))
				SoundManager.DeregisterBank(this);
		}
	}
}
