using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal class SoundInstanceManagerRuntime : MonoBehaviour
	{
		private const int defaultPoolSize = 32;

		private List<SoundInstance> usedSounds = null;
		private List<SoundInstance> freeSounds = null;

		internal void Stop()
		{
			for (int i = 0; i < usedSounds.Count; i++)
				usedSounds[i].Stop();
		}

		internal void Pause()
		{
			for (int i = 0; i < usedSounds.Count; i++)
				usedSounds[i].Pause();
		}

		internal void UnPause()
		{
			for (int i = 0; i < usedSounds.Count; i++)
				usedSounds[i].UnPause();
		}

		internal void Update()
		{
			for (int i = 0; i < usedSounds.Count; i++)
				usedSounds[i].Update();
		}

		internal SoundInstance GrabSound()
		{
			return GrabSound(null);
		}

		internal SoundInstance GrabSound(Sound sound)
		{
			// get free sound
			if (freeSounds.Count > 0)
			{
				int last = freeSounds.Count - 1;

				SoundInstance instance = freeSounds[last];
				instance.Sound = sound;

				freeSounds.RemoveAt(last);
				usedSounds.Add(instance);

				return instance;
			}

			// create new sound instance
			SoundInstance newInstance = new SoundInstance(sound, string.Format("Sound {0}", usedSounds.Count + 1), gameObject.transform);
			usedSounds.Add(newInstance);

			return newInstance;
		}

		internal bool ReleaseSound(SoundInstance instance)
		{
			int index = usedSounds.IndexOf(instance);
			if (index == -1)
				return false;

			instance.Release();

			int last = usedSounds.Count - 1;
			usedSounds[index] = usedSounds[last];
			usedSounds.RemoveAt(last);

			freeSounds.Add(instance);

			return true;
		}

		private void Awake()
		{
			gameObject.name = "Sound Instance Pool";

			freeSounds = new List<SoundInstance>(defaultPoolSize);
			usedSounds = new List<SoundInstance>(defaultPoolSize);

			for (int i = 0; i < defaultPoolSize; i++)
			{
				SoundInstance manualInstance = new SoundInstance(null, string.Format("Sound {0}", i + 1), gameObject.transform);
				freeSounds.Add(manualInstance);
			}
		}
	}
}
