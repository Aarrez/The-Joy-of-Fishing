using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	[ExecuteInEditMode]
	public class SoundBankInspectorPlayerRuntime : MonoBehaviour
	{
		private SoundBank bank = null;
		private Dictionary<SoundBus, SoundBusRuntime> busRuntimes = new Dictionary<SoundBus, SoundBusRuntime>();

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

		public void Play(Sound sound)
		{
			if (sound == null)
				return;

			SoundBus bus = sound.Bus;
			if (bus == null)
				bus = bank.DefaultBus;

			if (bus.Bank != bank)
				return;

			SoundBusRuntime runtime = null;
			if (!busRuntimes.TryGetValue(bus, out runtime))
			{
				runtime = new SoundBusRuntime(transform, bus);
				busRuntimes.Add(bus, runtime);
			}

			SoundInstance instance = runtime.GrabSound(sound);
			if (instance == null)
				return;

			instance.Play();
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

#endif
