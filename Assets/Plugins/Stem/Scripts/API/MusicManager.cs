using System.Collections.ObjectModel;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// A music callback function, called when the music player changes playback state (playing, stopped, paused).
	/// </summary>
	/// <param name="player">A reference to a music player.</param>
	public delegate void PlaybackChangedDelegate(MusicPlayer player);

	/// <summary>
	/// A music callback function, called when the music player transitions to a new track.
	/// </summary>
	/// <param name="player">A reference to a music player.</param>
	/// <param name="oldTrack">An old track that the music player was playing.</param>
	/// <param name="newTrack">A new track that the music player will play next.</param>
	public delegate void TrackChangedDelegate(MusicPlayer player, PlaylistTrack oldTrack, PlaylistTrack newTrack);

	/// <summary>
	/// The main class for music playback and bank management.
	/// </summary>
	public static class MusicManager
	{
		private static BankManager<MusicBank, MusicManagerRuntime> bankManager = new BankManager<MusicBank, MusicManagerRuntime>();
		private static bool shutdown = false;

		/// <summary>
		/// The delegate informing about playback start in any of the music players.
		/// </summary>
		public static event PlaybackChangedDelegate OnPlaybackStarted;

		/// <summary>
		/// The delegate informing about playback stop in any of the music players.
		/// </summary>
		/// <remarks>
		/// <para>This delegate will only be called after the music player fades out.</para>
		/// </remarks>
		public static event PlaybackChangedDelegate OnPlaybackStopped;

		/// <summary>
		/// The delegate informing about playback pause in any of the music players.
		/// </summary>
		/// <remarks>
		/// <para>This delegate will only be called after the music player fades out.</para>
		/// </remarks>
		public static event PlaybackChangedDelegate OnPlaybackPaused;

		/// <summary>
		/// The delegate informing about tracks changes in any of the music players.
		/// </summary>
		public static event TrackChangedDelegate OnTrackChanged;

		/// <summary>
		/// The collection of all registered music banks.
		/// </summary>
		/// <value>A reference to a read-only collection of music banks.</value>
		public static ReadOnlyCollection<MusicBank> Banks
		{
			get { return bankManager.Banks; }
		}

		/// <summary>
		/// The primary music bank that will be searched first in case of name collisions.
		/// </summary>
		/// <value>A reference to a primary music bank.</value>
		public static MusicBank PrimaryBank
		{
			get { return bankManager.PrimaryBank; }
			set { bankManager.PrimaryBank = value; }
		}

		/// <summary>
		/// Registers new music bank.
		/// </summary>
		/// <param name="bank">A reference to a music bank to register.</param>
		/// <returns>
		/// True, if music bank was succesfully registered. False otherwise.
		/// </returns>
		public static bool RegisterBank(MusicBank bank)
		{
			return bankManager.RegisterBank(bank);
		}

		/// <summary>
		/// Deregisters existing music bank.
		/// </summary>
		/// <param name="bank">A reference to a music bank to deregister.</param>
		/// <returns>
		/// True, if music bank was succesfully deregistered. False otherwise.
		/// </returns>
		public static bool DeregisterBank(MusicBank bank)
		{
			return bankManager.DeregisterBank(bank);
		}

		/// <summary>
		/// Sets a playlist to a music player.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="playlistID">ID referring to the playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If music player was playing another track it'll automatically crossfade to first track of the new playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void SetPlaylist(ID playerID, ID playlistID, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.SetPlaylist(): can't find music player, ID: {0}", playerID);
				return;
			}

			Playlist playlist = FetchPlaylist(playlistID);
			if (playlist == null)
			{
				Debug.LogWarningFormat("MusicManager.SetPlaylist(): can't find playlist, ID: {0}", playlistID);
				return;
			}

			runtime.SetPlaylist(playlist, fade);
		}

		/// <summary>
		/// Sets a playlist to a music player.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="playlistName">Name of the playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have playlists with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found playlist will be used.</para>
		/// <para>If music player was playing another track it'll automatically crossfade to first track of the new playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void SetPlaylist(ID playerID, string playlistName, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.SetPlaylist(): can't find music player, ID: {0}", playerID);
				return;
			}

			Playlist playlist = FetchPlaylist(playlistName);
			if (playlist == null)
			{
				Debug.LogWarningFormat("MusicManager.SetPlaylist(): can't find \"{0}\" playlist", playlistName);
				return;
			}

			runtime.SetPlaylist(playlist, fade);
		}

		/// <summary>
		/// Sets a playlist to a music player.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="playlistID">ID referring to the playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>If music player was playing another track it'll automatically crossfade to first track of the new playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void SetPlaylist(string playerName, ID playlistID, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.SetPlaylist(): can't find \"{0}\" music player", playerName);
				return;
			}

			Playlist playlist = FetchPlaylist(playlistID);
			if (playlist == null)
			{
				Debug.LogWarningFormat("MusicManager.SetPlaylist(): can't find playlist, ID: {0}", playlistID);
				return;
			}

			runtime.SetPlaylist(playlist, fade);
		}

		/// <summary>
		/// Sets a playlist to a music player.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="playlistName">Name of the playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>If music player was playing another track it'll automatically crossfade to first track of the new playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void SetPlaylist(string playerName, string playlistName, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.SetPlaylist(): can't find \"{0}\" music player", playerName);
				return;
			}

			Playlist playlist = FetchPlaylist(playlistName);
			if (playlist == null)
			{
				Debug.LogWarningFormat("MusicManager.SetPlaylist(): can't find \"{0}\" playlist", playlistName);
				return;
			}

			runtime.SetPlaylist(playlist, fade);
		}

		/// <summary>
		/// Searches for the specified playlist with a matching ID.
		/// </summary>
		/// <param name="id">ID referring to the playlist.</param>
		/// <returns>
		/// A reference to a playlist, if found. Null reference otherwise.
		/// </returns>
		public static Playlist GetPlaylist(ID id)
		{
			if (shutdown)
				return null;

			return FetchPlaylist(id);
		}

		/// <summary>
		/// Searches for the specified playlist with a matching name.
		/// </summary>
		/// <param name="name">Name of the playlist.</param>
		/// <returns>
		/// A reference to a playlist, if found. Null reference otherwise.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have playlists with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found playlist will be used.</para>
		/// </remarks>
		public static Playlist GetPlaylist(string name)
		{
			if (shutdown)
				return null;

			return FetchPlaylist(name);
		}

		/// <summary>
		/// Searches for the specified music player with a matching ID.
		/// </summary>
		/// <param name="id">ID referring to the music player.</param>
		/// <returns>
		/// A reference to a music player, if found. Null reference otherwise.
		/// </returns>
		public static MusicPlayer GetMusicPlayer(ID id)
		{
			if (shutdown)
				return null;

			return FetchMusicPlayer(id);
		}

		/// <summary>
		/// Searches for the specified music player with a matching name.
		/// </summary>
		/// <param name="name">Name of the music player.</param>
		/// <returns>
		/// A reference to a music player, if found. Null reference otherwise.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// </remarks>
		public static MusicPlayer GetMusicPlayer(string name)
		{
			if (shutdown)
				return null;

			return FetchMusicPlayer(name);
		}

		/// <summary>
		/// Checks whether or not specified music player with a matching ID is playing.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <returns>
		/// True, if the found music player is playing. False otherwise.
		/// </returns>
		public static bool IsPlaying(ID playerID)
		{
			if (shutdown)
				return false;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.IsPlaying(): can't find music player, ID: {0}", playerID);
				return false;
			}

			return runtime.IsPlaying;
		}

		/// <summary>
		/// Checks whether or not specified music player with a matching name is playing.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <returns>
		/// True, if the found music player is playing. False otherwise.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// </remarks>
		public static bool IsPlaying(string playerName)
		{
			if (shutdown)
				return false;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.IsPlaying(): can't find \"{0}\" music player", playerName);
				return false;
			}

			return runtime.IsPlaying;
		}

		/// <summary>
		/// Advances music player to next track.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Next(ID playerID, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Next(): can't find music player, ID: {0}", playerID);
				return;
			}

			runtime.Next(fade);
		}

		/// <summary>
		/// Advances music player to next track.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Next(string playerName, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Next(): can't find \"{0}\" music player", playerName);
				return;
			}

			runtime.Next(fade);
		}

		/// <summary>
		/// Advances music player to previous track.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Prev(ID playerID, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Prev(): can't find music player, ID: {0}", playerID);
				return;
			}

			runtime.Prev(fade);
		}

		/// <summary>
		/// Advances music player to previous track.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Prev(string playerName, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Prev(): can't find \"{0}\" music player", playerName);
				return;
			}

			runtime.Prev(fade);
		}

		/// <summary>
		/// Advances music player to a track by index.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="track">Zero-based index of the track in the current playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>Target track must be one of current playlist tracks.</para>
		/// <para>The index value represents track order as they appear in the playlist (e.g. setting index to one will seek to the second playlist track and so on).
		/// Shuffle order is ignored.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Seek(ID playerID, int track, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Seek(): can't find music player, ID: {0}", playerID);
				return;
			}

			runtime.Seek(track, fade);
		}

		/// <summary>
		/// Advances music player to a track with a matching name.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="track">Name of the track in the current playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>Target track must be one of current playlist tracks.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Seek(ID playerID, string track, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Seek(): can't find music player, ID: {0}", playerID);
				return;
			}

			runtime.Seek(track, fade);
		}

		/// <summary>
		/// Advances music player to a track by index.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="track">Zero-based index of the track in the current playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>Target track must be one of current playlist tracks.</para>
		/// <para>The index value represents track order as they appear in the playlist (e.g. setting index to one will seek to the second playlist track and so on).
		/// Shuffle order is ignored.</para>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Seek(string playerName, int track, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Seek(): can't find \"{0}\" music player", playerName);
				return;
			}

			runtime.Seek(track, fade);
		}

		/// <summary>
		/// Advances music player to a track with a matching name.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="track">Name of the track in the current playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>Target track must be one of current playlist tracks.</para>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Seek(string playerName, string track, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Seek(): can't find \"{0}\" music player", playerName);
				return;
			}

			runtime.Seek(track, fade);
		}

		/// <summary>
		/// Plays all music players from all music banks.
		/// </summary>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Play(float? fade = null)
		{
			if (shutdown)
				return;

			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				MusicBank bank = bankManager.Banks[i];
				MusicManagerRuntime runtime = bankManager.FetchRuntime(i);

				foreach (MusicPlayer player in bank.Players)
				{
					MusicPlayerRuntime playerRuntime = runtime.GetMusicPlayerRuntime(player);
					if (playerRuntime == null)
					{
						Debug.LogWarningFormat("MusicManager.Play(): can't find music player, ID: {0}", player.ID);
						continue;
					}

					playerRuntime.Play(fade);
				}
			}
		}

		/// <summary>
		/// Plays music player.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Play(ID playerID, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Play(): can't find music player, ID: {0}", playerID);
				return;
			}

			runtime.Play(fade);
		}

		/// <summary>
		/// Plays music player.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Play(string playerName, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Play(): can't find \"{0}\" music player", playerName);
				return;
			}

			runtime.Play(fade);
		}

		/// <summary>
		/// Stops all music players from all music banks.
		/// </summary>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Stop(float? fade = null)
		{
			if (shutdown)
				return;

			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				MusicBank bank = bankManager.Banks[i];
				MusicManagerRuntime runtime = bankManager.FetchRuntime(i);

				foreach (MusicPlayer player in bank.Players)
				{
					MusicPlayerRuntime playerRuntime = runtime.GetMusicPlayerRuntime(player);
					if (playerRuntime == null)
					{
						Debug.LogWarningFormat("MusicManager.Stop(): can't find music player, ID: {0}", player.ID);
						continue;
					}

					playerRuntime.Stop(fade);
				}
			}
		}

		/// <summary>
		/// Stops music player.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Stop(ID playerID, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Stop(): can't find music player, ID: {0}", playerID);
				return;
			}

			runtime.Stop(fade);
		}

		/// <summary>
		/// Stops music player.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Stop(string playerName, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Stop(): can't find \"{0}\" music player", playerName);
				return;
			}

			runtime.Stop(fade);
		}

		/// <summary>
		/// Pauses all music players from all music banks.
		/// </summary>
		/// <remarks>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// </remarks>
		public static void Pause()
		{
			if (shutdown)
				return;

			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				MusicBank bank = bankManager.Banks[i];
				MusicManagerRuntime runtime = bankManager.FetchRuntime(i);

				foreach (MusicPlayer player in bank.Players)
				{
					MusicPlayerRuntime playerRuntime = runtime.GetMusicPlayerRuntime(player);
					if (playerRuntime == null)
					{
						Debug.LogWarningFormat("MusicManager.Pause(): can't find music player, ID: {0}", player.ID);
						continue;
					}

					playerRuntime.Pause();
				}
			}
		}

		/// <summary>
		/// Pauses music player.
		/// </summary>
		/// <param name="playerID">ID referring to the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Pause(ID playerID, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerID);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Pause(): can't find music player, ID: {0}", playerID);
				return;
			}

			runtime.Pause(fade);
		}

		/// <summary>
		/// Pauses music player.
		/// </summary>
		/// <param name="playerName">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, the primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. <see cref="SetPlaylist(string, string, float?)"/> to assign a playlist.</para>
		/// <para>Non-null crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Pause(string playerName, float? fade = null)
		{
			if (shutdown)
				return;

			MusicPlayerRuntime runtime = FetchMusicPlayerRuntime(playerName);
			if (runtime == null)
			{
				Debug.LogWarningFormat("MusicManager.Pause(): can't find \"{0}\" music player", playerName);
				return;
			}

			runtime.Pause(fade);
		}

		internal static void Init()
		{
			for (int i = 0; i < bankManager.Banks.Count; i++)
				bankManager.FetchRuntime(i);

			shutdown = false;
		}

		internal static void Shutdown()
		{
			shutdown = true;
		}

		internal static MusicPlayer FetchMusicPlayer(ID id)
		{
			// Check all banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				MusicBank bank = bankManager.Banks[i];
				MusicPlayer player = bank.GetMusicPlayer(id);

				if (player != null)
					return player;
			}

			return null;
		}

		internal static MusicPlayer FetchMusicPlayer(string name)
		{
			// Check primary bank first
			int primaryIndex = bankManager.PrimaryBankIndex;
			if (primaryIndex != -1)
			{
				MusicBank bank = bankManager.Banks[primaryIndex];

				if (bank.Runtime.ContainsMusicPlayer(name))
					return bank.Runtime.GetMusicPlayer(name);
			}

			// Check other banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				MusicBank bank = bankManager.Banks[i];

				// Skip primary bank
				if (i == primaryIndex)
					continue;

				if (bank.Runtime.ContainsMusicPlayer(name))
					return bank.Runtime.GetMusicPlayer(name);
			}

			return null;
		}

		internal static Playlist FetchPlaylist(ID id)
		{
			// Check all banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				MusicBank bank = bankManager.Banks[i];
				Playlist playlist = bank.GetPlaylist(id);

				if (playlist != null)
					return playlist;
			}

			return null;
		}

		internal static Playlist FetchPlaylist(string name)
		{
			// Check primary bank first
			int primaryIndex = bankManager.PrimaryBankIndex;
			if (primaryIndex != -1)
			{
				MusicBank bank = bankManager.Banks[primaryIndex];

				if (bank.Runtime.ContainsPlaylist(name))
					return bank.Runtime.GetPlaylist(name);
			}

			// Check other banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				// Skip primary bank
				if (i == primaryIndex)
					continue;

				MusicBank bank = bankManager.Banks[i];

				if (bank.Runtime.ContainsPlaylist(name))
					return bank.Runtime.GetPlaylist(name);
			}

			return null;
		}

		internal static MusicPlayerRuntime FetchMusicPlayerRuntime(ID id)
		{
			// Check all banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				MusicBank bank = bankManager.Banks[i];
				MusicPlayer player = bank.GetMusicPlayer(id);

				if (player != null)
				{
					MusicManagerRuntime runtime = bankManager.FetchRuntime(i);
					return runtime.GetMusicPlayerRuntime(player);
				}
			}

			return null;
		}

		internal static MusicPlayerRuntime FetchMusicPlayerRuntime(string name)
		{
			// Check primary bank first
			int primaryIndex = bankManager.PrimaryBankIndex;
			if (primaryIndex != -1)
			{
				MusicBank bank = bankManager.Banks[primaryIndex];

				if (bank.Runtime.ContainsMusicPlayer(name))
				{
					MusicPlayer player = bank.GetMusicPlayer(name);
					MusicManagerRuntime runtime = bankManager.FetchRuntime(primaryIndex);
					return runtime.GetMusicPlayerRuntime(player);
				}
			}

			// Check other banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				// Skip primary bank
				if (i == primaryIndex)
					continue;

				MusicBank bank = bankManager.Banks[i];

				if (bank.Runtime.ContainsMusicPlayer(name))
				{
					MusicPlayer player = bank.GetMusicPlayer(name);
					MusicManagerRuntime runtime = bankManager.FetchRuntime(i);
					return runtime.GetMusicPlayerRuntime(player);
				}
			}

			return null;
		}

		internal static void InvokePlaybackStart(MusicPlayer player)
		{
			if (OnPlaybackStarted != null)
				OnPlaybackStarted(player);
		}

		internal static void InvokePlaybackStop(MusicPlayer player)
		{
			if (OnPlaybackStopped != null)
				OnPlaybackStopped(player);
		}

		internal static void InvokePlaybackPause(MusicPlayer player)
		{
			if (OnPlaybackPaused != null)
				OnPlaybackPaused(player);
		}

		internal static void InvokeTrackChange(MusicPlayer player, PlaylistTrack oldTrack, PlaylistTrack newTrack)
		{
			if (OnTrackChanged != null)
				OnTrackChanged(player, oldTrack, newTrack);
		}
	}
}
