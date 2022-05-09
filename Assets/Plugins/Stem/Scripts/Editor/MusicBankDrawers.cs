using UnityEditor;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Stem
{
	internal class MusicManagerViewModel
	{
		public MusicBank[] banks = null;
		public GUIContent[] bankNames = null;
		public GUIContent[] playlistNames = null;
		public GUIContent[] playerNames = null;
		public int bankIndex = 0;
		public int playlistIndex = 0;
		public int playerIndex = 0;

		public MusicManagerViewModel(ID desiredId)
		{
			banks = BankDBPostprocessor.LoadBankAssets<MusicBank>();
			int numBanks = (banks != null) ? banks.Length : 0;

			bankNames = new GUIContent[numBanks + 1];
			bankNames[0] = new GUIContent("None");
			for (int i = 0; i < numBanks; i++)
			{
				MusicBank bank = banks[i];
				if (bank == null)
					continue;

				bankNames[i + 1] = new GUIContent(string.Format("[{0}]", bank.name));
			}

			for (int i = 0; i < numBanks; i++)
			{
				MusicBank possibleBank = banks[i];
				if (possibleBank == null)
					continue;

				Playlist possiblePlaylist = possibleBank.GetPlaylist(desiredId);
				MusicPlayer possiblePlayer = possibleBank.GetMusicPlayer(desiredId);

				if (possiblePlaylist != null)
					playlistIndex = possibleBank.Playlists.IndexOf(possiblePlaylist);

				if (possiblePlayer != null)
					playerIndex = possibleBank.Players.IndexOf(possiblePlayer);

				ID bankId = possibleBank.GetBankID();
				if (bankId.BankEquals(desiredId))
				{
					bankIndex = i + 1;
					break;
				}
			}
		}

		internal void FetchPlaylistNames(int index)
		{
			MusicBank bank = banks[index];
			if (bank == null || bank.Playlists.Count == 0)
			{
				playlistNames = null;
				return;
			}

			if (playlistNames == null || (playlistNames.Length != bank.Playlists.Count))
				playlistNames = new GUIContent[bank.Playlists.Count];

			NameDuplicatesManager duplicates = new NameDuplicatesManager(null);
			for (int i = 0; i < bank.Playlists.Count; i++)
			{
				if (bank.Playlists[i] == null)
					continue;

				string playlistName = duplicates.GrabName(bank.Playlists[i].Name);
				playlistNames[i] = new GUIContent(playlistName);
			}
		}

		internal void FetchMusicPlayerNames(int index)
		{
			MusicBank bank = banks[index];
			if (bank == null || bank.Players.Count == 0)
			{
				playerNames = null;
				return;
			}

			if (playerNames == null || (playerNames.Length != bank.Players.Count))
				playerNames = new GUIContent[bank.Players.Count];

			NameDuplicatesManager duplicates = new NameDuplicatesManager(null);
			for (int i = 0; i < bank.Players.Count; i++)
			{
				if (bank.Players[i] == null)
					continue;

				string playerName = duplicates.GrabName(bank.Players[i].Name);
				playerNames[i] = new GUIContent(playerName);
			}
		}
	}

	internal class MusicManagerView
	{
		private MusicManagerViewModel viewModel = null;

		public MusicManagerView(MusicManagerViewModel model)
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

			EditorGUI.LabelField(position, string.Format("Use {0} attribute with ID type.", name));
			return true;
		}

		internal bool NoBanksMessage(Rect position)
		{
			if (viewModel.banks != null && viewModel.banks.Length > 0)
				return false;

			EditorGUI.LabelField(position, "No music banks found, consider adding one.");
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

			viewModel.FetchPlaylistNames(bankIndex);
			viewModel.FetchMusicPlayerNames(bankIndex);
		}

		internal ID PlaylistsPopup(Rect position)
		{
			if (viewModel.bankIndex == 0)
			{
				EditorGUI.LabelField(position, "select a music bank");
				return ID.None;
			}

			if (viewModel.playlistNames == null || viewModel.playlistNames.Length == 0)
			{
				EditorGUI.LabelField(position, "bank has no playlists.");
				return ID.None;
			}

			viewModel.playlistIndex = EditorGUI.Popup(position, viewModel.playlistIndex, viewModel.playlistNames);
			if (viewModel.playlistIndex >= viewModel.playlistNames.Length)
				return ID.None;

			MusicBank bank = viewModel.banks[viewModel.bankIndex - 1];

			return bank.GetPlaylistID(viewModel.playlistIndex);
		}

		internal ID PlayersPopup(Rect position)
		{
			if (viewModel.bankIndex == 0)
			{
				EditorGUI.LabelField(position, "select a music bank");
				return ID.None;
			}

			if (viewModel.playerNames == null || viewModel.playerNames.Length == 0)
			{
				EditorGUI.LabelField(position, "bank has no music players.");
				return ID.None;
			}

			viewModel.playerIndex = EditorGUI.Popup(position, viewModel.playerIndex, viewModel.playerNames);
			if (viewModel.playerIndex >= viewModel.playerNames.Length)
				return ID.None;

			MusicBank bank = viewModel.banks[viewModel.bankIndex - 1];

			return bank.GetMusicPlayerID(viewModel.playerIndex);
		}
	}

	internal abstract class MusicBankDrawerBase : PropertyDrawer
	{
		internal MusicManagerViewModel viewModel = null;
		internal MusicManagerView view = null;
		internal string attributeName = null;

		internal abstract ID GetID(Rect position);

		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			MusicManagerView.Label(ref position, label);
			if (MusicManagerView.WrongTypeMessage(position, prop, attributeName))
				return;

			SerializedProperty itemId = prop.FindPropertyRelative("itemId");
			SerializedProperty guidA = prop.FindPropertyRelative("bankGuidA");
			SerializedProperty guidB = prop.FindPropertyRelative("bankGuidB");
			SerializedProperty guidC = prop.FindPropertyRelative("bankGuidC");
			SerializedProperty guidD = prop.FindPropertyRelative("bankGuidD");

			ID currentId = new ID(guidA.intValue, guidB.intValue, guidC.intValue, guidD.intValue, itemId.intValue);

			if (viewModel == null || view == null)
			{
				viewModel = new MusicManagerViewModel(currentId);
				view = new MusicManagerView(viewModel);
			}

			if (view.NoBanksMessage(position))
				return;

			EditorGUI.BeginChangeCheck();

			view.BanksPopup(ref position);
			ID newId = GetID(position);

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

	[CustomPropertyDrawer(typeof(PlaylistIDAttribute))]
	internal class PlaylistIDDrawer : MusicBankDrawerBase
	{
		public PlaylistIDDrawer()
		{
			attributeName = "PlaylistID";
		}

		internal override ID GetID(Rect position)
		{
			return view.PlaylistsPopup(position);
		}
	}

	[CustomPropertyDrawer(typeof(MusicPlayerIDAttribute))]
	internal class MusicPlayerIDDrawer : MusicBankDrawerBase
	{
		public MusicPlayerIDDrawer()
		{
			attributeName = "MusicPlayerID";
		}

		internal override ID GetID(Rect position)
		{
			return view.PlayersPopup(position);
		}
	}
}
