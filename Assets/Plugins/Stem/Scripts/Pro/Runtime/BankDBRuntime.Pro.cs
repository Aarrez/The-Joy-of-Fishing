using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	public partial class BankDBRuntime : MonoBehaviour
	{
		private void PreloadAudioClips()
		{
			for (int i = 0; i < SoundManager.Banks.Count; i++)
			{
				SoundBank bank = SoundManager.Banks[i];
				for (int j = 0; j < bank.Sounds.Count; j++)
				{
					Sound sound = bank.Sounds[j];

					if (sound.GetAudioClipManagementMode() == AudioClipManagementMode.PreloadAndKeepInMemory)
						MemoryManager.Touch(sound);
				}
			}

			for (int i = 0; i < MusicManager.Banks.Count; i++)
			{
				MusicBank bank = MusicManager.Banks[i];
				for (int j = 0; j < bank.Playlists.Count; j++)
				{
					Playlist playlist = bank.Playlists[j];

					if (playlist.GetAudioClipManagementMode() == AudioClipManagementMode.PreloadAndKeepInMemory)
						MemoryManager.Touch(playlist);
				}
			}
		}
	}
}

#endif
