using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Stem
{
	internal class BankDBPostprocessor
	{
		[PostProcessSceneAttribute(0)]
		internal static void OnPostprocessScene()
		{
			if (!BuildPipeline.isBuildingPlayer)
				AssetDatabase.SaveAssets();

			SoundBank[] soundBanks = LoadBankAssets<SoundBank>();
			MusicBank[] musicBanks = LoadBankAssets<MusicBank>();

			GameObject db = new GameObject();
			db.name = "BankDBRuntime";

			BankDBRuntime dbRuntime = db.AddComponent<BankDBRuntime>();
			dbRuntime.soundBanks = soundBanks;
			dbRuntime.musicBanks = musicBanks;

			// Initialize banks in the editor manually, because BankDBRuntime already called Awake method
			if (!BuildPipeline.isBuildingPlayer)
				dbRuntime.Init();

#if UNITY_5_5
			EditorApplication.playmodeStateChanged += PlayModeChanged;
#else
			EditorApplication.playModeStateChanged += PlayModeChanged;
#endif
		}

#if UNITY_5_5
		private static void PlayModeChanged()
#else
		private static void PlayModeChanged(PlayModeStateChange state)
#endif
		{
			if (EditorApplication.isPlaying || EditorApplication.isPaused || EditorApplication.isPlayingOrWillChangePlaymode)
				return;

			// Revert any changes made during runtime
			UnloadBankAssets();
		}

		internal static void UnloadBankAssets()
		{
			for (int i = SoundManager.Banks.Count - 1; i >= 0; i--)
			{
				SoundBank bank = SoundManager.Banks[i];
				SoundManager.DeregisterBank(bank);
				Resources.UnloadAsset(bank);
			}

			for (int i = MusicManager.Banks.Count - 1; i >= 0; i--)
			{
				MusicBank bank = MusicManager.Banks[i];
				MusicManager.DeregisterBank(bank);
				Resources.UnloadAsset(bank);
			}
		}

		internal static BankType[] LoadBankAssets<BankType>() where BankType : ScriptableObject, IBank
		{
			string[] guids = AssetDatabase.FindAssets("t:"+ typeof(BankType).Name);
			if (guids.Length == 0)
				return null;

			BankType[] banks = new BankType[guids.Length];
			for (int i = 0; i < guids.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				BankType bank = AssetDatabase.LoadAssetAtPath<BankType>(path);

				banks[i] = bank;
			}

			return banks;
		}
	}
}
