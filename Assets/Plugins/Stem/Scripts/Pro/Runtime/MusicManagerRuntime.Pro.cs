using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	internal partial class MusicManagerRuntime : MonoBehaviour, IManagerRuntime<MusicBank>
	{
		private Dictionary<byte, SyncGroupRuntime> syncGroupRuntimes = new Dictionary<byte, SyncGroupRuntime>();

		internal void InitSyncGroups(MusicBank bank)
		{
			Dictionary<MusicPlayer, MusicPlayerRuntime>.Enumerator enumerator = playerRuntimes.GetEnumerator();
			while(enumerator.MoveNext())
			{
				MusicPlayer player = enumerator.Current.Key;

				if (player.PlaybackMode != MusicPlayerPlaybackMode.Synced)
					continue;

				if (syncGroupRuntimes.ContainsKey(player.SyncGroup))
					continue;

				syncGroupRuntimes.Add(player.SyncGroup, new SyncGroupRuntime(player.SyncGroup, this));
			}
		}

		private void OnDestroy()
		{
			Dictionary<byte, SyncGroupRuntime>.Enumerator enumerator = syncGroupRuntimes.GetEnumerator();
			while(enumerator.MoveNext())
			{
				SyncGroupRuntime runtime = enumerator.Current.Value;
				runtime.Shutdown();
			}
		}
	}
}

#endif
