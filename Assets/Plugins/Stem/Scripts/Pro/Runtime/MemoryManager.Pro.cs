using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	internal static class MemoryManager
	{
		private static GameObject gameObject = null;
		private static MemoryManagerRuntime memoryManagerRuntime = null;

		internal static void Touch(IAudioClipContainer container)
		{
			if (container == null)
				return;

			if (container.GetAudioClipManagementMode() == AudioClipManagementMode.Manual)
				return;

			MemoryManagerRuntime runtime = FetchMemoryManagerRuntime();
			if (runtime == null)
				return;

			for (int i = 0; i < container.GetNumAudioClips(); i++)
			{
				AudioClip clip = container.GetAudioClip(i);
				if (clip == null)
					continue;

				runtime.Touch(container, clip);
			}
		}

		internal static void Touch(IAudioClipContainer container, AudioClip clip)
		{
			if (container == null)
				return;

			if (container.GetAudioClipManagementMode() == AudioClipManagementMode.Manual)
				return;

			if (clip == null)
				return;

			MemoryManagerRuntime runtime = FetchMemoryManagerRuntime();
			if (runtime == null)
				return;

			runtime.Touch(container, clip);
		}

		internal static void Grab(IAudioClipContainer container)
		{
			if (container == null)
				return;

			if (container.GetAudioClipManagementMode() == AudioClipManagementMode.Manual)
				return;

			MemoryManagerRuntime runtime = FetchMemoryManagerRuntime();
			if (runtime == null)
				return;

			for (int i = 0; i < container.GetNumAudioClips(); i++)
			{
				AudioClip clip = container.GetAudioClip(i);
				if (clip == null)
					continue;

				runtime.Grab(container, clip);
			}
		}

		internal static void Grab(IAudioClipContainer container, AudioClip clip)
		{
			if (container == null)
				return;

			if (container.GetAudioClipManagementMode() == AudioClipManagementMode.Manual)
				return;

			if (clip == null)
				return;

			MemoryManagerRuntime runtime = FetchMemoryManagerRuntime();
			if (runtime == null)
				return;

			runtime.Grab(container, clip);
		}

		internal static void Release(IAudioClipContainer container, AudioClip clip)
		{
			if (container == null)
				return;

			if (container.GetAudioClipManagementMode() == AudioClipManagementMode.Manual)
				return;

			if (clip == null)
				return;

			MemoryManagerRuntime runtime = FetchMemoryManagerRuntime();
			if (runtime == null)
				return;

			runtime.Release(container, clip);
		}

		private static MemoryManagerRuntime FetchMemoryManagerRuntime()
		{
			if (!Application.isPlaying)
				return null;

			if (memoryManagerRuntime != null)
				return memoryManagerRuntime;

			gameObject = new GameObject();
			gameObject.name = "Memory Manager";

			memoryManagerRuntime = gameObject.AddComponent<MemoryManagerRuntime>();
			GameObject.DontDestroyOnLoad(gameObject);

			return memoryManagerRuntime;
		}
	}
}

#endif
