using System;
using System.Collections.Generic;

namespace Stem
{
	internal class MusicBankRuntime
	{
		[NonSerialized]
		private Dictionary<string, List<Playlist> > playlistByName = new Dictionary<string, List<Playlist> >();

		[NonSerialized]
		private LocalIDStorageRuntime<Playlist> playlistByID = new LocalIDStorageRuntime<Playlist>();

		[NonSerialized]
		private Dictionary<string, List<MusicPlayer> > playerByName = new Dictionary<string, List<MusicPlayer> >();

		[NonSerialized]
		private LocalIDStorageRuntime<MusicPlayer> playerByID = new LocalIDStorageRuntime<MusicPlayer>();

		internal int SoloedMusicPlayers { get; set; }

		internal void Clear()
		{
			playlistByID.Clear();
			playlistByName.Clear();

			playerByID.Clear();
			playerByName.Clear();

			SoloedMusicPlayers = 0;
		}

		internal bool ContainsPlaylist(int id)
		{
			return playlistByID.Contains(id);
		}

		internal bool ContainsPlaylist(string name)
		{
			return playlistByName.ContainsKey(name);
		}

		internal Playlist GetPlaylist(int id)
		{
			return playlistByID.Get(id);
		}

		internal Playlist GetPlaylist(string name)
		{
			List<Playlist> playlistList = null;
			if (!playlistByName.TryGetValue(name, out playlistList))
				return null;

			if (playlistList.Count == 0)
				return null;

			return playlistList[0];
		}

		internal void AddPlaylist(Playlist playlist)
		{
			playlist.ID = playlistByID.Add(playlist.ID, playlist);

			List<Playlist> playlistList = null;
			if (!playlistByName.TryGetValue(playlist.Name, out playlistList))
			{
				playlistList = new List<Playlist>();
				playlistByName.Add(playlist.Name, playlistList);
			}

			if (!playlistList.Contains(playlist))
				playlistList.Add(playlist);
		}

		internal void RemovePlaylist(Playlist playlist)
		{
			playlistByID.Remove(playlist.ID);

			List<Playlist> playlistList = null;
			if (!playlistByName.TryGetValue(playlist.Name, out playlistList))
				return;

			int index = playlistList.IndexOf(playlist);
			if (index != -1)
				playlistList.RemoveAt(index);
		}

		internal void RenamePlaylist(Playlist playlist, string oldName, string newName)
		{
			List<Playlist> playlistList = null;
			if (!playlistByName.TryGetValue(oldName, out playlistList))
				return;

			int index = playlistList.IndexOf(playlist);
			if (index == -1)
				return;

			playlistList.RemoveAt(index);
			if (!playlistByName.TryGetValue(newName, out playlistList))
			{
				playlistList = new List<Playlist>();
				playlistByName.Add(newName, playlistList);
			}

			playlistList.Add(playlist);
		}

		internal bool ContainsMusicPlayer(int id)
		{
			return playerByID.Contains(id);
		}

		internal bool ContainsMusicPlayer(string name)
		{
			return playerByName.ContainsKey(name);
		}

		internal MusicPlayer GetMusicPlayer(int id)
		{
			return playerByID.Get(id);
		}

		internal MusicPlayer GetMusicPlayer(string name)
		{
			List<MusicPlayer> playerList = null;
			if (!playerByName.TryGetValue(name, out playerList))
				return null;

			if (playerList.Count == 0)
				return null;

			return playerList[0];
		}

		internal void AddMusicPlayer(MusicPlayer player)
		{
			player.ID = playerByID.Add(player.ID, player);

			List<MusicPlayer> playerList = null;
			if (!playerByName.TryGetValue(player.Name, out playerList))
			{
				playerList = new List<MusicPlayer>();
				playerByName.Add(player.Name, playerList);
			}

			if (!playerList.Contains(player))
				playerList.Add(player);

			if (player.Soloed)
				SoloedMusicPlayers++;
		}

		internal void RemoveMusicPlayer(MusicPlayer player)
		{
			playerByID.Remove(player.ID);

			List<MusicPlayer> playerList = null;
			if (!playerByName.TryGetValue(player.Name, out playerList))
				return;

			int index = playerList.IndexOf(player);
			if (index != -1)
				playerList.RemoveAt(index);

			if (player.Soloed)
				SoloedMusicPlayers--;
		}

		internal void RenameMusicPlayer(MusicPlayer player, string oldName, string newName)
		{
			List<MusicPlayer> playerList = null;
			if (!playerByName.TryGetValue(oldName, out playerList))
				return;

			int index = playerList.IndexOf(player);
			if (index == -1)
				return;

			playerList.RemoveAt(index);
			if (!playerByName.TryGetValue(newName, out playerList))
			{
				playerList = new List<MusicPlayer>();
				playerByName.Add(newName, playerList);
			}

			playerList.Add(player);
		}
	}
}
