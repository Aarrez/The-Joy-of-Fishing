using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal class SoundManagerRuntime : MonoBehaviour, IManagerRuntime<SoundBank>
	{
		private SoundBank bank = null;
		private Dictionary<SoundBus, SoundBusRuntime> busRuntimes = new Dictionary<SoundBus, SoundBusRuntime>();

		public SoundBank Bank
		{
			get { return bank; }
		}

		public void Init(SoundBank bank_)
		{
			busRuntimes.Clear();
			bank = bank_;

			foreach (SoundBus bus in bank.Buses)
			{
				SoundBusRuntime busRuntime = new SoundBusRuntime(transform, bus);
				busRuntimes.Add(bus, busRuntime);
			}
		}

		internal SoundInstance FetchSoundInstance(Sound sound)
		{
			if (sound == null)
				return null;

			SoundBus bus = sound.Bus;
			if (bus == null)
				bus = bank.DefaultBus;

			SoundBusRuntime runtime = null;
			if (!busRuntimes.TryGetValue(bus, out runtime))
				return null;

			SoundInstance instance = runtime.GrabSound(sound);
			if (instance == null)
				return null;

			return instance;
		}

		internal void Stop()
		{
			Dictionary<SoundBus, SoundBusRuntime>.Enumerator enumerator = busRuntimes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoundBusRuntime runtime = enumerator.Current.Value;
				runtime.Stop();
			}
		}

		internal void Pause()
		{
			Dictionary<SoundBus, SoundBusRuntime>.Enumerator enumerator = busRuntimes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoundBusRuntime runtime = enumerator.Current.Value;
				runtime.Pause();
			}
		}

		internal void UnPause()
		{
			Dictionary<SoundBus, SoundBusRuntime>.Enumerator enumerator = busRuntimes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoundBusRuntime runtime = enumerator.Current.Value;
				runtime.UnPause();
			}
		}

		private void Update()
		{
			Dictionary<SoundBus, SoundBusRuntime>.Enumerator enumerator = busRuntimes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoundBusRuntime runtime = enumerator.Current.Value;
				runtime.Update(bank);
			}
		}
	}
}
