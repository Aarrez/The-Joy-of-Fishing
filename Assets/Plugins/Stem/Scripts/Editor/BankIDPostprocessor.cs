using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Stem
{
	class BankIDPostprocessor : UnityEditor.AssetPostprocessor
	{
		static bool PatchBankGuids<BankType>(string[] importedAssets) where BankType : ScriptableObject, IBank
		{
			string[] guids = AssetDatabase.FindAssets("t:"+ typeof(BankType).Name);
			if (guids.Length == 0)
				return false;

			List<string> existingPaths = new List<string>();
			List<string> importedPaths = new List<string>();

			for (int i = 0; i < guids.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);

				bool wasImported = false;

				for (int j = 0; j < importedAssets.Length; j++)
				{
					if (importedAssets[j] == path)
					{
						wasImported = true;
						break;
					}
				}

				if (wasImported)
					importedPaths.Add(path);
				else
					existingPaths.Add( path);
			}

			if (importedPaths.Count == 0 || existingPaths.Count == 0)
				return false;

			Dictionary<ID, BankType> existingBanks = new Dictionary<ID, BankType>();

			for (int i = 0; i < existingPaths.Count; ++i)
			{
				string path = existingPaths[i];
				BankType bank = AssetDatabase.LoadAssetAtPath<BankType>(path);
				if (bank == null)
					continue;

				existingBanks.Add(bank.GetBankID(), bank);
			}

			bool shouldSave = false;
			for (int i = 0; i < importedPaths.Count; ++i)
			{
				string path = importedPaths[i];
				BankType importedBank = AssetDatabase.LoadAssetAtPath<BankType>(path);
				if (importedBank == null)
					continue;

				ID importedBankId = importedBank.GetBankID();

				if (!existingBanks.ContainsKey(importedBankId))
					continue;

				importedBank.RegenerateBankID();
				shouldSave = true;
			}

			return shouldSave;
		}

		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			// Make sure we won't regenerate bank IDs during build
			if (BuildPipeline.isBuildingPlayer)
				return;

			bool shouldSave = false;

			shouldSave |= PatchBankGuids<SoundBank>(importedAssets);
			shouldSave |= PatchBankGuids<MusicBank>(importedAssets);

			if (shouldSave)
				AssetDatabase.SaveAssets();
		}
	}
}
