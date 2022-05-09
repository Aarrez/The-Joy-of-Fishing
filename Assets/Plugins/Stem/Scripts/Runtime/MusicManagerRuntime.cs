using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal partial class MusicManagerRuntime : MonoBehaviour, IManagerRuntime<MusicBank>
	{
		private MusicBank bank = null;
		private Dictionary<MusicPlayer, MusicPlayerRuntime> playerRuntimes = new Dictionary<MusicPlayer, MusicPlayerRuntime>();

		public void Init(MusicBank bank_)
		{
			bank = bank_;

			playerRuntimes.Clear();
			foreach (MusicPlayer player in bank.Players)
			{
				MusicPlayerRuntime runtime = new MusicPlayerRuntime(transform, player);
				playerRuntimes.Add(player, runtime);
			}

			InitSyncGroups(bank);

			Dictionary<MusicPlayer, MusicPlayerRuntime>.Enumerator enumerator = playerRuntimes.GetEnumerator();
			while(enumerator.MoveNext())
			{
				MusicPlayerRuntime runtime = enumerator.Current.Value;
				MusicPlayer player = enumerator.Current.Key;

				if (player.Playlist != null)
					runtime.SetPlaylist(player.Playlist);

				if (player.PlayOnStart)
					runtime.Play();
			}
		}

		internal MusicPlayerRuntime GetMusicPlayerRuntime(MusicPlayer player)
		{
			MusicPlayerRuntime runtime = null;
			playerRuntimes.TryGetValue(player, out runtime);

			return runtime;
		}

		private void Update()
		{
			float dt = Time.unscaledDeltaTime;

			Dictionary<MusicPlayer, MusicPlayerRuntime>.Enumerator enumerator = playerRuntimes.GetEnumerator();
			while(enumerator.MoveNext())
			{
				MusicPlayerRuntime runtime = enumerator.Current.Value;
				runtime.Update(dt);
			}
		}
	}
}
