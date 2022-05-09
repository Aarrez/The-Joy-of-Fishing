using System;
using System.Collections.Generic;

namespace Stem
{
	internal class SoundBankRuntime
	{
		[NonSerialized]
		private Dictionary<string, List<Sound> > soundByName = new Dictionary<string, List<Sound> >();

		[NonSerialized]
		private LocalIDStorageRuntime<Sound> soundByID = new LocalIDStorageRuntime<Sound>();

		[NonSerialized]
		private Dictionary<string, List<SoundBus> > busByName = new Dictionary<string, List<SoundBus> >();

		[NonSerialized]
		private LocalIDStorageRuntime<SoundBus> busByID = new LocalIDStorageRuntime<SoundBus>();

		internal int SoloedSounds { get; set; }
		internal int SoloedSoundBuses { get; set; }

		internal void Clear()
		{
			soundByID.Clear();
			soundByName.Clear();

			busByID.Clear();
			busByName.Clear();

			SoloedSounds = 0;
			SoloedSoundBuses = 0;
		}

		internal bool ContainsSound(int id)
		{
			return soundByID.Contains(id);
		}

		internal bool ContainsSound(string name)
		{
			return soundByName.ContainsKey(name);
		}

		internal Sound GetSound(int id)
		{
			return soundByID.Get(id);
		}

		internal Sound GetSound(string name)
		{
			List<Sound> soundList = null;
			if (!soundByName.TryGetValue(name, out soundList))
				return null;

			if (soundList.Count == 0)
				return null;

			return soundList[0];
		}

		internal void AddSound(Sound sound)
		{
			sound.ID = soundByID.Add(sound.ID, sound);

			List<Sound> soundList = null;
			if (!soundByName.TryGetValue(sound.Name, out soundList))
			{
				soundList = new List<Sound>();
				soundByName.Add(sound.Name, soundList);
			}

			if (!soundList.Contains(sound))
				soundList.Add(sound);

			if (sound.Soloed)
				SoloedSounds++;
		}

		internal void RemoveSound(Sound sound)
		{
			soundByID.Remove(sound.ID);

			List<Sound> soundList = null;
			if (!soundByName.TryGetValue(sound.Name, out soundList))
				return;

			int index = soundList.IndexOf(sound);
			if (index != -1)
				soundList.RemoveAt(index);

			if (soundList.Count == 0)
				soundByName.Remove(sound.Name);

			if (sound.Soloed)
				SoloedSounds--;
		}

		internal void RenameSound(Sound sound, string oldName, string newName)
		{
			List<Sound> soundList = null;
			if (!soundByName.TryGetValue(oldName, out soundList))
				return;

			int index = soundList.IndexOf(sound);
			if (index == -1)
				return;

			soundList.RemoveAt(index);
			if (!soundByName.TryGetValue(newName, out soundList))
			{
				soundList = new List<Sound>();
				soundByName.Add(newName, soundList);
			}

			soundList.Add(sound);
		}

		internal bool ContainsSoundBus(int id)
		{
			return busByID.Contains(id);
		}

		internal bool ContainsSoundBus(string name)
		{
			return busByName.ContainsKey(name);
		}

		internal SoundBus GetSoundBus(int id)
		{
			return busByID.Get(id);
		}

		internal SoundBus GetSoundBus(string name)
		{
			List<SoundBus> busList = null;
			if (!busByName.TryGetValue(name, out busList))
				return null;

			if (busList.Count == 0)
				return null;

			return busList[0];
		}

		internal void AddSoundBus(SoundBus bus)
		{
			bus.ID = busByID.Add(bus.ID, bus);

			List<SoundBus> busList = null;
			if (!busByName.TryGetValue(bus.Name, out busList))
			{
				busList = new List<SoundBus>();
				busByName.Add(bus.Name, busList);
			}

			if (!busList.Contains(bus))
				busList.Add(bus);

			if (bus.Soloed)
				SoloedSoundBuses++;
		}

		internal void RemoveSoundBus(SoundBus bus)
		{
			busByID.Remove(bus.ID);

			List<SoundBus> busList = null;
			if (!busByName.TryGetValue(bus.Name, out busList))
				return;

			int index = busList.IndexOf(bus);
			if (index != -1)
				busList.RemoveAt(index);

			if (bus.Soloed)
				SoloedSoundBuses--;
		}

		internal void RenameSoundBus(SoundBus bus, string oldName, string newName)
		{
			List<SoundBus> busList = null;
			if (!busByName.TryGetValue(oldName, out busList))
				return;

			int index = busList.IndexOf(bus);
			if (index == -1)
				return;

			busList.RemoveAt(index);
			if (!busByName.TryGetValue(newName, out busList))
			{
				busList = new List<SoundBus>();
				busByName.Add(newName, busList);
			}

			busList.Add(bus);
		}
	}
}
