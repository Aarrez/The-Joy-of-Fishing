using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	public enum AudioEventActionType
	{
		PlaySound,
		PlayTrack,
		GlobalSoundPlayback,
		SoundBusVolume,
		MusicPlayerVolume,
		MusicPlayerControl,
		MusicPlayerPlayback,
	}

	[System.Serializable]
	internal class AudioEventAction
	{
		[SerializeField]
		internal AudioEventActionType type = AudioEventActionType.PlaySound;

		[SerializeField]
		internal PlaySoundAction playSoundAction = new PlaySoundAction();

		[SerializeField]
		internal PlayTrackAction playTrackAction = new PlayTrackAction();

		[SerializeField]
		internal GlobalSoundPlaybackAction globalSoundPlaybackAction = new GlobalSoundPlaybackAction();

		[SerializeField]
		internal SoundBusVolumeAction soundBusVolumeAction = new SoundBusVolumeAction();

		[SerializeField]
		internal MusicPlayerVolumeAction musicPlayerVolumeAction = new MusicPlayerVolumeAction();

		[SerializeField]
		internal MusicPlayerControlAction musicPlayerControlAction = new MusicPlayerControlAction();

		[SerializeField]
		internal MusicPlayerPlaybackAction musicPlayerPlaybackAction = new MusicPlayerPlaybackAction();

		internal void Call()
		{
			switch (type)
			{
				case AudioEventActionType.PlaySound: playSoundAction.Call(); break;
				case AudioEventActionType.PlayTrack: playTrackAction.Call(); break;
				case AudioEventActionType.GlobalSoundPlayback: globalSoundPlaybackAction.Call(); break;
				case AudioEventActionType.SoundBusVolume: soundBusVolumeAction.Call(); break;
				case AudioEventActionType.MusicPlayerVolume: musicPlayerVolumeAction.Call(); break;
				case AudioEventActionType.MusicPlayerControl: musicPlayerControlAction.Call(); break;
				case AudioEventActionType.MusicPlayerPlayback: musicPlayerPlaybackAction.Call(); break;
				default: Debug.LogErrorFormat("AudioEventAction.Call(): unsupported action type {0}", type.ToString()); break;
			}
		}
	}

	[System.Serializable]
	internal class PlaySoundAction
	{
		[SoundID, SerializeField]
		internal Stem.ID sound = Stem.ID.None;

		[SerializeField]
		internal Transform spawnTarget = null;

		internal void Call()
		{
			SoundManager.Play3D(sound, spawnTarget ? spawnTarget.position : Vector3.zero);
		}
	}

	[System.Serializable]
	internal class PlayTrackAction
	{
		[MusicPlayerID, SerializeField]
		internal Stem.ID musicPlayer = Stem.ID.None;

		[PlaylistID, SerializeField]
		internal Stem.ID playlist = Stem.ID.None;

		[SerializeField]
		internal int trackIndex = 0;

		internal void Call()
		{
			MusicManager.SetPlaylist(musicPlayer, playlist);
			MusicManager.Seek(musicPlayer, trackIndex);
			MusicManager.Play(musicPlayer);
		}
	}

	[System.Serializable]
	internal class GlobalSoundPlaybackAction
	{
		internal enum ActionType
		{
			Stop,
			Pause,
			UnPause,
		}

		[SerializeField]
		internal ActionType action = ActionType.Stop;

		internal void Call()
		{
			switch(action)
			{
				case ActionType.Stop: SoundManager.Stop(); break;
				case ActionType.Pause: SoundManager.Pause(); break;
				case ActionType.UnPause: SoundManager.UnPause(); break;
				default: Debug.LogErrorFormat("GlobalSoundPlaybackAction.Call(): unsupported action type {0}", action.ToString()); break;
			}
		}
	}

	[System.Serializable]
	internal class SoundBusVolumeAction
	{
		[SoundBusID, SerializeField]
		internal Stem.ID soundBus = Stem.ID.None;

		[Range(0.0f, 1.0f), SerializeField]
		internal float volume = 1.0f;

		[SerializeField]
		internal bool mute = false;

		[SerializeField]
		internal bool solo = false;

		internal void Call()
		{
			SoundBus bus = SoundManager.GetSoundBus(soundBus);
			if (bus == null)
				return;

			bus.Volume = volume;
			bus.Muted = mute;
			bus.Soloed = solo;
		}
	}

	[System.Serializable]
	internal class MusicPlayerVolumeAction
	{
		[MusicPlayerID, SerializeField]
		internal Stem.ID musicPlayer = Stem.ID.None;

		[Range(0.0f, 1.0f), SerializeField]
		internal float volume = 1.0f;

		[SerializeField]
		internal bool mute = false;

		[SerializeField]
		internal bool solo = false;

		internal void Call()
		{
			MusicPlayer player = MusicManager.GetMusicPlayer(musicPlayer);
			if (player == null)
				return;

			player.Volume = volume;
			player.Muted = mute;
			player.Soloed = solo;
		}
	}

	[System.Serializable]
	internal class MusicPlayerControlAction
	{
		[MusicPlayerID, SerializeField]
		internal Stem.ID musicPlayer = Stem.ID.None;

		[SerializeField]
		internal MusicPlayerPlaybackMode playbackMode = MusicPlayerPlaybackMode.Default;

		[SerializeField]
		internal bool loop = false;

		[SerializeField]
		internal bool shuffle = false;

		internal void Call()
		{
			MusicPlayer player = MusicManager.GetMusicPlayer(musicPlayer);
			if (player == null)
				return;

			player.PlaybackMode = playbackMode;
			player.Loop = loop;
			player.Shuffle = shuffle;
		}
	}

	[System.Serializable]
	internal class MusicPlayerPlaybackAction
	{
		internal enum ActionType
		{
			Next,
			Prev,
			Play,
			Stop,
			Pause,
		}

		[MusicPlayerID, SerializeField]
		internal Stem.ID musicPlayer = Stem.ID.None;

		[SerializeField]
		internal ActionType action = ActionType.Next;

		internal void Call()
		{
			switch(action)
			{
				case ActionType.Next: MusicManager.Next(musicPlayer); break;
				case ActionType.Prev: MusicManager.Prev(musicPlayer); break;
				case ActionType.Play: MusicManager.Play(musicPlayer); break;
				case ActionType.Stop: MusicManager.Stop(musicPlayer); break;
				case ActionType.Pause: MusicManager.Pause(musicPlayer); break;
				default: Debug.LogErrorFormat("MusicPlayerPlaybackAction.Call(): unsupported action type {0}", action.ToString()); break;
			}
		}
	}
}

#endif
