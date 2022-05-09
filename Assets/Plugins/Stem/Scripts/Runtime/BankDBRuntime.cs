using UnityEngine;

namespace Stem
{
	public partial class BankDBRuntime : MonoBehaviour
	{
		public SoundBank[] soundBanks = null;
		public MusicBank[] musicBanks = null;

		private bool isProxy = false;

		public void Init()
		{
			if (soundBanks != null)
			{
				for (int i = 0; i < soundBanks.Length; i++)
				{
					SoundBank bank = soundBanks[i];
					if (!SoundManager.Banks.Contains(bank))
						SoundManager.RegisterBank(bank);
				}
			}

			if (musicBanks != null)
			{
				for (int i = 0; i < musicBanks.Length; i++)
				{
					MusicBank bank = musicBanks[i];
					if (!MusicManager.Banks.Contains(bank))
						MusicManager.RegisterBank(bank);
				}
			}

			SoundManager.Init();
			MusicManager.Init();

			PreloadAudioClips();
		}

		private void OnApplicationQuit()
		{
			if (isProxy)
				return;

			SoundManager.Shutdown();
			MusicManager.Shutdown();
		}

		private void Awake()
		{
			BankDBRuntime[] dbRuntimes = Object.FindObjectsOfType<BankDBRuntime>();
			isProxy = dbRuntimes.Length > 1;

			if (!isProxy)
			{
				GameObject.DontDestroyOnLoad(gameObject);
				Init();
			}
			else
				GameObject.Destroy(gameObject);
		}
	}
}
