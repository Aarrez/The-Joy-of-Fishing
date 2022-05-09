using UnityEditor;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Stem
{
	internal class SoundManagerViewModel
	{
		public SoundBank[] banks = null;
		public GUIContent[] bankNames = null;
		public GUIContent[] soundNames = null;
		public GUIContent[] busNames = null;
		public int bankIndex = 0;
		public int soundIndex = 0;
		public int busIndex = 0;

		public SoundManagerViewModel(ID desiredId)
		{
			banks = BankDBPostprocessor.LoadBankAssets<SoundBank>();
			int numBanks = (banks != null) ? banks.Length : 0;

			bankNames = new GUIContent[numBanks + 1];
			bankNames[0] = new GUIContent("None");
			for (int i = 0; i < numBanks; i++)
			{
				SoundBank bank = banks[i];
				if (bank == null)
					continue;

				bankNames[i + 1] = new GUIContent(string.Format("[{0}]", bank.name));
			}

			for (int i = 0; i < numBanks; i++)
			{
				SoundBank possibleBank = banks[i];
				if (possibleBank == null)
					continue;

				Sound possibleSound = possibleBank.GetSound(desiredId);
				SoundBus possibleBus = possibleBank.GetSoundBus(desiredId);

				if (possibleSound != null)
					soundIndex = possibleBank.Sounds.IndexOf(possibleSound);

				if (possibleBus != null)
					busIndex = possibleBank.Buses.IndexOf(possibleBus);

				ID bankId = possibleBank.GetBankID();
				if (bankId.BankEquals(desiredId))
				{
					bankIndex = i + 1;
					break;
				}
			}
		}

		internal void FetchSoundNames(int index)
		{
			SoundBank bank = banks[index];
			if (bank == null || bank.Sounds.Count == 0)
			{
				soundNames = null;
				return;
			}

			if (soundNames == null || (soundNames.Length != bank.Sounds.Count))
				soundNames = new GUIContent[bank.Sounds.Count];

			NameDuplicatesManager duplicates = new NameDuplicatesManager(null);
			for (int i = 0; i < bank.Sounds.Count; i++)
			{
				if (bank.Sounds[i] == null)
					continue;

				string soundName = duplicates.GrabName(bank.Sounds[i].Name);
				soundNames[i] = new GUIContent(soundName);
			}
		}

		internal void FetchSoundBusNames(int index)
		{
			SoundBank bank = banks[index];
			if (bank == null || bank.Buses.Count == 0)
			{
				busNames = null;
				return;
			}

			if (busNames == null || (busNames.Length != bank.Buses.Count))
				busNames = new GUIContent[bank.Buses.Count];

			NameDuplicatesManager duplicates = new NameDuplicatesManager(null);
			for (int i = 0; i < bank.Buses.Count; i++)
			{
				if (bank.Buses[i] == null)
					continue;

				string busName = duplicates.GrabName(bank.Buses[i].Name);
				busNames[i] = new GUIContent(busName);
			}
		}
	}

	internal class SoundManagerView
	{
		private SoundManagerViewModel viewModel = null;

		public SoundManagerView(SoundManagerViewModel model)
		{
			viewModel = model;
		}

		internal static void Label(ref Rect position, GUIContent label)
		{
			EditorGUI.LabelField(position, label);

			position.x += EditorGUIUtility.labelWidth;
			position.width -= EditorGUIUtility.labelWidth;
		}

		internal static bool WrongTypeMessage(Rect position, SerializedProperty prop, string name)
		{
			SerializedProperty itemId = prop.FindPropertyRelative("itemId");
			SerializedProperty guidA = prop.FindPropertyRelative("bankGuidA");
			SerializedProperty guidB = prop.FindPropertyRelative("bankGuidB");
			SerializedProperty guidC = prop.FindPropertyRelative("bankGuidC");
			SerializedProperty guidD = prop.FindPropertyRelative("bankGuidD");

			bool itemIdExists = (itemId != null && itemId.propertyType == SerializedPropertyType.Integer);
			bool guidAExists = (guidA != null && guidA.propertyType == SerializedPropertyType.Integer);
			bool guidBExists = (guidB != null && guidB.propertyType == SerializedPropertyType.Integer);
			bool guidCExists = (guidC != null && guidC.propertyType == SerializedPropertyType.Integer);
			bool guidDExists = (guidD != null && guidD.propertyType == SerializedPropertyType.Integer);

			if (itemIdExists && guidAExists && guidBExists && guidCExists && guidDExists)
				return false;

			EditorGUI.LabelField(position, string.Format("Use {0} attribute with int type.", name));
			return true;
		}

		internal bool NoBanksMessage(Rect position)
		{
			if (viewModel.banks != null && viewModel.banks.Length > 0)
				return false;

			EditorGUI.LabelField(position, "No sound banks found, consider adding one.");
			return true;
		}

		internal void BanksPopup(ref Rect position)
		{
			position.width *= 0.5f;

			viewModel.bankIndex = EditorGUI.Popup(position, viewModel.bankIndex, viewModel.bankNames);

			position.x += position.width;

			int bankIndex = viewModel.bankIndex - 1;
			if (bankIndex < 0 || bankIndex >= viewModel.banks.Length)
				return;

			viewModel.FetchSoundNames(bankIndex);
			viewModel.FetchSoundBusNames(bankIndex);
		}

		internal Stem.ID SoundsPopup(Rect position)
		{
			if (viewModel.bankIndex == 0)
			{
				EditorGUI.LabelField(position, "select a sound bank");
				return Stem.ID.None;
			}

			if (viewModel.soundNames == null || viewModel.soundNames.Length == 0)
			{
				EditorGUI.LabelField(position, "bank has no sounds.");
				return Stem.ID.None;
			}

			viewModel.soundIndex = EditorGUI.Popup(position, viewModel.soundIndex, viewModel.soundNames);
			if (viewModel.soundIndex < 0 || viewModel.soundIndex >= viewModel.soundNames.Length)
				return Stem.ID.None;

			SoundBank bank = viewModel.banks[viewModel.bankIndex - 1];

			return bank.GetSoundID(viewModel.soundIndex);
		}

		internal Stem.ID BusesPopup(Rect position)
		{
			if (viewModel.bankIndex == 0)
			{
				EditorGUI.LabelField(position, "select a sound bank");
				return Stem.ID.None;
			}

			if (viewModel.busNames == null || viewModel.busNames.Length == 0)
			{
				EditorGUI.LabelField(position, "bank has no sound buses.");
				return Stem.ID.None;
			}

			viewModel.busIndex = EditorGUI.Popup(position, viewModel.busIndex, viewModel.busNames);
			if (viewModel.busIndex < 0 || viewModel.busIndex >= viewModel.busNames.Length)
				return Stem.ID.None;

			SoundBank bank = viewModel.banks[viewModel.bankIndex - 1];

			return bank.GetSoundBusID(viewModel.busIndex);
		}
	}

	internal abstract class SoundBankDrawerBase : PropertyDrawer
	{
		internal SoundManagerViewModel viewModel = null;
		internal SoundManagerView view = null;
		internal string attributeName = null;

		internal abstract Stem.ID GetID(Rect position);

		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, prop);

			SoundManagerView.Label(ref position, label);
			if (SoundManagerView.WrongTypeMessage(position, prop, attributeName))
				return;

			SerializedProperty itemId = prop.FindPropertyRelative("itemId");
			SerializedProperty guidA = prop.FindPropertyRelative("bankGuidA");
			SerializedProperty guidB = prop.FindPropertyRelative("bankGuidB");
			SerializedProperty guidC = prop.FindPropertyRelative("bankGuidC");
			SerializedProperty guidD = prop.FindPropertyRelative("bankGuidD");

			ID currentId = new ID(guidA.intValue, guidB.intValue, guidC.intValue, guidD.intValue, itemId.intValue);

			if (viewModel == null || view == null)
			{
				viewModel = new SoundManagerViewModel(currentId);
				view = new SoundManagerView(viewModel);
			}

			if (view.NoBanksMessage(position))
				return;

			EditorGUI.BeginChangeCheck();

			view.BanksPopup(ref position);
			Stem.ID newId = GetID(position);

			if (EditorGUI.EndChangeCheck())
			{
				itemId.intValue = newId.ItemId;
				guidA.intValue = newId.BankGuidA;
				guidB.intValue = newId.BankGuidB;
				guidC.intValue = newId.BankGuidC;
				guidD.intValue = newId.BankGuidD;
			}

			EditorGUI.EndProperty();
		}
	}

	[CustomPropertyDrawer(typeof(SoundIDAttribute))]
	internal class SoundIDDrawer : SoundBankDrawerBase
	{
		public SoundIDDrawer()
		{
			attributeName = "SoundID";
		}

		internal override Stem.ID GetID(Rect position)
		{
			return view.SoundsPopup(position);
		}
	}

	[CustomPropertyDrawer(typeof(SoundBusIDAttribute))]
	internal class SoundBusIDDrawer : SoundBankDrawerBase
	{
		public SoundBusIDDrawer()
		{
			attributeName = "SoundBusID";
		}

		internal override Stem.ID GetID(Rect position)
		{
			return view.BusesPopup(position);
		}
	}
}
