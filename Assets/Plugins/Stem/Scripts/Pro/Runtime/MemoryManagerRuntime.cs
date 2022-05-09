using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	internal class MemoryManagerRuntime: MonoBehaviour
	{
		internal class Data
		{
			public int usageCounter;
			public float lastUsedTime;
			public float unloadInterval;
		}

		private Dictionary<AudioClip, Data> managedClips = new Dictionary<AudioClip, Data>();
		private List<AudioClip> unloadedClips = new List<AudioClip>();

		internal void Grab(IAudioClipContainer container, AudioClip clip)
		{
			Touch(container, clip);

			Data data = null;
			if (!managedClips.TryGetValue(clip, out data))
				return;

			data.usageCounter++;
		}

		internal void Release(IAudioClipContainer container, AudioClip clip)
		{
			Touch(container, clip);

			Data data = null;
			if (!managedClips.TryGetValue(clip, out data))
				return;

			data.usageCounter = Mathf.Max(0, data.usageCounter - 1);
		}

		internal void Touch(IAudioClipContainer container, AudioClip clip)
		{
			AudioClipManagementMode mode = container.GetAudioClipManagementMode();

			if (mode == AudioClipManagementMode.Manual)
				return;

			if (clip.loadState != AudioDataLoadState.Loaded)
				clip.LoadAudioData();

			if (mode != AudioClipManagementMode.UnloadUnused)
				return;

			Data data = null;
			if (!managedClips.TryGetValue(clip, out data))
			{
				data = new Data();
				data.unloadInterval = container.GetAudioClipUnloadInterval();
				data.usageCounter = 0;

				managedClips.Add(clip, data);
			}

			data.lastUsedTime = Time.realtimeSinceStartup;
		}

		private void Update()
		{
			float currentTime = Time.realtimeSinceStartup;
			unloadedClips.Clear();

			Dictionary<AudioClip, Data>.Enumerator enumerator = managedClips.GetEnumerator();
			while (enumerator.MoveNext())
			{
				AudioClip clip = enumerator.Current.Key;
				Data data = enumerator.Current.Value;

				if (data.usageCounter > 0)
					continue;

				if (currentTime - data.lastUsedTime < data.unloadInterval)
					continue;

				if (clip != null)
					clip.UnloadAudioData();

				unloadedClips.Add(clip);
			}

			for (int i = 0; i < unloadedClips.Count; i++)
				managedClips.Remove(unloadedClips[i]);
		}
	}
}

#endif
