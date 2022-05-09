using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	internal class SyncGroupRuntime
	{
		private byte syncGroup = 0;
		private bool active = false;

		private MusicManagerRuntime musicManagerRuntime = null;
		private HashSet<MusicPlayerRuntime> syncedRuntimes = new HashSet<MusicPlayerRuntime>();
		private MusicPlayerRuntime primaryRuntime = null;

		public SyncGroupRuntime(byte group, MusicManagerRuntime manager)
		{
			syncGroup = group;
			musicManagerRuntime = manager;

			MusicManager.OnPlaybackStarted += OnPlaybackStarted;
			MusicManager.OnPlaybackStopped += OnPlaybackStopped;
			MusicManager.OnPlaybackPaused += OnPlaybackPaused;
			MusicManager.OnTrackChanged += OnTrackChanged;
		}

		internal void Shutdown()
		{
			musicManagerRuntime = null;
			syncedRuntimes.Clear();

			MusicManager.OnPlaybackStarted -= OnPlaybackStarted;
			MusicManager.OnPlaybackStopped -= OnPlaybackStopped;
			MusicManager.OnPlaybackPaused -= OnPlaybackPaused;
			MusicManager.OnTrackChanged -= OnTrackChanged;
		}

		private MusicPlayerRuntime GetMusicPlayerRuntime(MusicPlayer player)
		{
			if (player.SyncGroup != syncGroup)
				return null;

			MusicPlayerRuntime runtime = musicManagerRuntime.GetMusicPlayerRuntime(player);
			if (runtime == null)
				Debug.LogWarningFormat("SyncGroupRuntime.GetMusicPlayerRuntime(): can't get music player runtime with name \"{0}\"", player.Name);

			return runtime;
		}

		private void Sync(MusicPlayer player)
		{
			MusicPlayerRuntime runtime = GetMusicPlayerRuntime(player);
			if (runtime == null)
				return;

			active = true;

			syncedRuntimes.Add(runtime);
			if (primaryRuntime == null)
				primaryRuntime = runtime;

			int timeSamples = primaryRuntime.GetCurrentTrackTimeSamples();
			runtime.Sync(timeSamples);
		}

		private void Sync(MusicPlayer player, PlaylistTrack oldTrack)
		{
			MusicPlayerRuntime runtime = GetMusicPlayerRuntime(player);
			if (runtime == null)
				return;

			active = true;

			syncedRuntimes.Add(runtime);

			int timeSamples = 0;
			if (primaryRuntime == null)
			{
				primaryRuntime = runtime;
				timeSamples = primaryRuntime.GetTrackTimeSamples(oldTrack);
			}
			else
				timeSamples = primaryRuntime.GetCurrentTrackTimeSamples();

			runtime.Sync(timeSamples);
		}

		private void Desync(MusicPlayer player)
		{
			if (!active)
				return;

			MusicPlayerRuntime runtime = GetMusicPlayerRuntime(player);
			if (runtime == null)
				return;

			syncedRuntimes.Remove(runtime);
			if (primaryRuntime == runtime)
			{
				HashSet<MusicPlayerRuntime>.Enumerator enumerator = syncedRuntimes.GetEnumerator();
				enumerator.MoveNext();
				primaryRuntime = (syncedRuntimes.Count > 0) ? enumerator.Current : null;
			}

			if (syncedRuntimes.Count == 0)
				active = false;
		}

		private void OnPlaybackStarted(MusicPlayer player) { Sync(player); }
		private void OnPlaybackStopped(MusicPlayer player) { Desync(player); }
		private void OnPlaybackPaused(MusicPlayer player) { Desync(player); }
		private void OnTrackChanged(MusicPlayer player, PlaylistTrack oldTrack, PlaylistTrack newTrack)
		{
			if (!active)
				return;

			Desync(player);
			Sync(player, oldTrack);
		}
	}
}

#endif
