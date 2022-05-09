using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal class Fader
	{
		internal delegate void ActionHandler();
		internal event ActionHandler OnFadeEnd;
		internal event ActionHandler OnFadeBegin;

		private float duration = 0.0f;
		private float time = 0.0f;
		private bool fading = true;
		private float volume = 0.0f;
		private float oldVolume = 0.0f;
		private float targetVolume = 0.0f;

		internal float Volume
		{
			get { return volume; }
			set
			{
				targetVolume = value;
				volume = value;
				oldVolume = value;
				fading = false;
			}
		}

		internal float TargetVolume
		{
			get { return targetVolume; }
		}

		internal bool Fading
		{
			get { return fading; }
		}

		internal void FadeIn(float d)
		{
			BeginFade(d, 1.0f);
		}

		internal void FadeOut(float d)
		{
			BeginFade(d, 0.0f);
		}

		internal void Update(float dt)
		{
			if (!fading)
				return;

			time -= dt;

			float k = Mathf.Clamp01(time / duration);
			volume = Mathf.Lerp(targetVolume, oldVolume, k);

			if (time < 0.0f)
			{
				volume = targetVolume;
				fading = false;
				if (OnFadeEnd != null)
					OnFadeEnd();
			}
		}

		private void BeginFade(float d, float v)
		{
			duration = d;
			time = d;

			oldVolume = volume;
			targetVolume = v;

			fading = true;
			if (OnFadeBegin != null)
				OnFadeBegin();

			if (duration <= 0.0f)
			{
				volume = targetVolume;
				fading = false;
				if (OnFadeEnd != null)
					OnFadeEnd();
			}
		}
	}

	internal class PlaylistAdvancer
	{
		private Playlist playlist;
		private bool shuffle;
		private bool loop;
		private int shuffleIndex;
		private int[] shuffleOrder;

		internal PlaylistAdvancer(Playlist playlist_, bool shuffle_, bool loop_)
		{
			SetPlaylist(playlist_, shuffle_, loop_);
		}

		internal void SetPlaylist(Playlist playlist_, bool shuffle_, bool loop_)
		{
			playlist = playlist_;
			shuffle = shuffle_;
			loop = loop_;

			int numTracks = playlist.Tracks.Count;

			shuffleIndex = 0;
			shuffleOrder = new int[numTracks];

			for (int i = 0; i < numTracks; i++)
				shuffleOrder[i] = i;

			if (shuffle)
				Shuffle();
		}

		internal PlaylistTrack GetTrack()
		{
			if (shuffleIndex < 0 || shuffleIndex >= shuffleOrder.Length)
				return null;

			int index = shuffleOrder[shuffleIndex];
			return playlist.Tracks[index];
		}

		internal PlaylistTrack Next()
		{
			shuffleIndex++;
			Wrap();

			if (shuffleIndex < 0 || shuffleIndex >= shuffleOrder.Length)
				return null;

			return GetTrack();
		}

		internal PlaylistTrack Prev()
		{
			shuffleIndex--;
			Wrap();

			if (shuffleIndex < 0 || shuffleIndex >= shuffleOrder.Length)
				return null;

			return GetTrack();
		}

		internal PlaylistTrack Seek(int index)
		{
			if (index < 0 || index >= playlist.Tracks.Count)
				return null;

			return Seek(playlist.Tracks[index].Name);
		}

		internal PlaylistTrack Seek(string name)
		{
			int numTracks = playlist.Tracks.Count;
			for (int i = 0; i < numTracks; i++)
			{
				int index = shuffleOrder[i];
				PlaylistTrack track = playlist.Tracks[index];
				if (track.Name == name)
				{
					shuffleIndex = i;
					return GetTrack();
				}
			}
			return null;
		}

		private void Wrap()
		{
			int numTracks = playlist.Tracks.Count;

			if (!loop)
			{
				shuffleIndex = Mathf.Clamp(shuffleIndex, -1, numTracks);
				return;
			}

			if (shuffle && (shuffleIndex >= numTracks || shuffleIndex < 0))
				Shuffle();

			shuffleIndex %= numTracks;
			while (shuffleIndex < 0)
				shuffleIndex += numTracks;
		}

		private void Shuffle()
		{
			int numTracks = playlist.Tracks.Count;
			for(int i = numTracks - 1; i >= 0; i--)
			{
				int index = Random.Range(0, numTracks);
				int temp = shuffleOrder[i];
				shuffleOrder[i] = shuffleOrder[index];
				shuffleOrder[index] = temp;
			}
		}
	}

	internal class TrackMixer
	{
		internal Fader fader = new Fader();
		internal AudioSource source = null;
		internal PlaylistTrack track = null;
	}

	internal enum PlaybackAction
	{
		Play,
		Stop,
		Pause,
	}

	internal class MusicPlayerRuntime
	{
		private GameObject root = null;
		private List<TrackMixer> usedMixers = new List<TrackMixer>();
		private List<TrackMixer> freeMixers = new List<TrackMixer>();

		private Fader playbackFader = new Fader();
		private PlaybackAction playbackAction = PlaybackAction.Stop;
		private bool isPlaying = false;

		private MusicPlayer player = null;
		private PlaylistAdvancer advancer = null;

		internal MusicPlayerRuntime(Transform transform_, MusicPlayer player_)
		{
			player = player_;

			root = new GameObject();
			root.transform.parent = transform_;
			root.name = player.Name;

			playbackFader.OnFadeBegin += OnPlaybackFadeBegin;
			playbackFader.OnFadeEnd += OnPlaybackFadeEnd;

			playbackFader.Volume = 1.0f;
		}

		internal MusicPlayer Player
		{
			get { return player; }
		}

		internal bool IsPlaying
		{
			get { return isPlaying; }
		}

		internal void Play(float? fade = null)
		{
			if (advancer == null)
				return;

			float desiredFade = fade ?? player.Fade;

			if (isPlaying)
				Reset();

			playbackAction = PlaybackAction.Play;
			playbackFader.FadeIn(desiredFade);
		}

		internal void Stop(float? fade = null)
		{
			if (advancer == null)
				return;

			float desiredFade = fade ?? player.Fade;

			if (!isPlaying)
			{
				desiredFade = 0.0f; // fade out immediately
				Reset();
			}

			playbackAction = PlaybackAction.Stop;
			playbackFader.FadeOut(desiredFade);
		}

		internal void Pause(float? fade = null)
		{
			if (!isPlaying || advancer == null)
				return;

			float desiredFade = fade ?? player.Fade;

			playbackAction = PlaybackAction.Pause;
			playbackFader.FadeOut(desiredFade);
		}

		internal void Next(float? fade = null)
		{
			if (advancer == null)
				return;

			float desiredFade = fade ?? player.Fade;
			Crossfade(advancer.GetTrack(), advancer.Next(), desiredFade);
		}

		internal void Prev(float? fade = null)
		{
			if (advancer == null)
				return;

			float desiredFade = fade ?? player.Fade;
			Crossfade(advancer.GetTrack(), advancer.Prev(), desiredFade);
		}

		internal void Seek(int index, float? fade = null)
		{
			if (advancer == null)
				return;

			float desiredFade = fade ?? player.Fade;
			Crossfade(advancer.GetTrack(), advancer.Seek(index), desiredFade);
		}

		internal void Seek(string track)
		{
			Seek(track, player.Fade);
		}

		internal void Seek(string track, float? fade = null)
		{
			if (advancer == null)
				return;

			float desiredFade = fade ?? player.Fade;
			Crossfade(advancer.GetTrack(), advancer.Seek(track), desiredFade);
		}

		internal void SetPlaylist(Playlist playlist, float? fade = null)
		{
			PlaylistTrack oldTrack = null;
			if (advancer != null)
				oldTrack = advancer.GetTrack();

			PlaylistTrack newTrack = null;

			if (playlist == null)
			{
				Stop(fade);
				advancer = null;
			}
			else
			{
				advancer = new PlaylistAdvancer(playlist, player.Shuffle, player.Loop);
				newTrack = advancer.GetTrack();
			}

			float desiredFade = fade ?? player.Fade;
			Crossfade(oldTrack, newTrack, desiredFade);
		}

		internal void Sync(int timeSamples)
		{
			for (int i = usedMixers.Count - 1; i >= 0; i--)
			{
				TrackMixer mixer = usedMixers[i];
				AudioSource source = mixer.source;
				source.timeSamples = timeSamples % source.clip.samples;
			}
		}

		internal int GetTrackTimeSamples(PlaylistTrack track)
		{
			if (advancer == null)
				return 0;

			TrackMixer mixer = GetUsedMixer(track);
			if (mixer == null)
				return 0;

			AudioSource source = mixer.source;
			return source.timeSamples;
		}

		internal int GetCurrentTrackTimeSamples()
		{
			return GetTrackTimeSamples(advancer.GetTrack());
		}

		internal void Update(float dt)
		{
			playbackFader.Update(dt);

			bool audible = player.Audible;

			for (int i = usedMixers.Count - 1; i >= 0; i--)
			{
				TrackMixer mixer = usedMixers[i];
				AudioSource source = mixer.source;
				PlaylistTrack track = mixer.track;
				Fader fader = mixer.fader;

				fader.Update(dt);
				if (fader.Volume == 0.0f)
					ReleaseUsedMixer(i);

				source.volume = playbackFader.Volume * fader.Volume * player.Volume * track.Volume;
				source.mute = !audible;
			}

			ProcessAdvance();
		}

		private void ProcessAdvance()
		{
			if (advancer == null)
				return;

			TrackMixer currentMixer = GetUsedMixer(advancer.GetTrack());
			if (currentMixer == null)
				return;

			AudioSource source = currentMixer.source;

			if (player.PlaybackMode == MusicPlayerPlaybackMode.AutoAdvance)
			{
				bool track_is_finishing = (source.loop == false) && (source.time + player.Fade >= source.clip.length);

				if (track_is_finishing)
					Crossfade(advancer.GetTrack(), advancer.Next(), player.Fade);
			}
			else if (!source.isPlaying && isPlaying)
				Stop(0.0f);
		}

		private void Reset()
		{
			for (int i = 0; i < usedMixers.Count; i++)
			{
				TrackMixer mixer = usedMixers[i];
				if (mixer.fader.TargetVolume == 0.0f)
					continue;

				mixer.source.timeSamples = 0;
			}
		}

		private void Crossfade(PlaylistTrack oldTrack, PlaylistTrack newTrack, float fade)
		{
			for (int i = 0; i < usedMixers.Count; i++)
			{
				TrackMixer mixer = usedMixers[i];
				mixer.fader.FadeOut(fade);
			}

			TrackMixer targetMixer = FetchMixer(newTrack);
			if (targetMixer != null)
				targetMixer.fader.FadeIn(fade);

			MusicManager.InvokeTrackChange(player, oldTrack, newTrack);
			player.InvokeTrackChange(oldTrack, newTrack);
		}

		private TrackMixer GetUsedMixer(PlaylistTrack track)
		{
			if (track == null)
				return null;

			for (int i = 0; i < usedMixers.Count; i++)
			{
				TrackMixer mixer = usedMixers[i];
				if (mixer.track == track)
					return mixer;
			}

			return null;
		}

		private TrackMixer FetchMixer(PlaylistTrack track)
		{
			if (track == null)
				return null;

			TrackMixer mixer = GetUsedMixer(track);
			if (mixer != null)
				return mixer;

			if (freeMixers.Count > 0)
			{
				mixer = freeMixers[freeMixers.Count - 1];
				freeMixers.RemoveAt(freeMixers.Count - 1);
			}

			if (mixer == null)
			{
				mixer = new TrackMixer();
				mixer.source = root.AddComponent<AudioSource>();
			}

			if (mixer.track != null)
				MemoryManager.Release(mixer.track.Playlist, mixer.track.Clip);

			mixer.track = track;
			mixer.source.clip = track.Clip;
			mixer.source.timeSamples = 0;
			mixer.source.outputAudioMixerGroup = player.MixerGroup;
			mixer.source.loop = player.Loop;

			// Disable track looping only if we have several tracks in the playlist and playback mode is AutoAdvance,
			// otherwise just rely on track looping
			if (player.PlaybackMode == MusicPlayerPlaybackMode.AutoAdvance && track.Playlist.Tracks.Count > 1)
				mixer.source.loop = false;

			MemoryManager.Grab(track.Playlist, track.Clip);

			if (isPlaying)
				mixer.source.Play();

			usedMixers.Add(mixer);
			return mixer;
		}

		private void ReleaseUsedMixer(int index)
		{
			TrackMixer mixer = usedMixers[index];
			usedMixers.RemoveAt(index);
			freeMixers.Add(mixer);

			if (mixer.track != null)
				MemoryManager.Release(mixer.track.Playlist, mixer.track.Clip);

			mixer.source.clip = null;
			mixer.track = null;
		}

		private void OnPlaybackFadeBegin()
		{
			switch (playbackAction)
			{
				case PlaybackAction.Play:
					for (int i = 0; i < usedMixers.Count; i++)
					{
						TrackMixer mixer = usedMixers[i];
						mixer.source.Play();
					}
					isPlaying = true;
					MusicManager.InvokePlaybackStart(player);
					player.InvokePlaybackStart();
				break;
			}
		}

		private void OnPlaybackFadeEnd()
		{
			switch (playbackAction)
			{
				case PlaybackAction.Stop:
					for (int i = 0; i < usedMixers.Count; i++)
					{
						TrackMixer mixer = usedMixers[i];
						mixer.source.Stop();
					}
					isPlaying = false;
					MusicManager.InvokePlaybackStop(player);
					player.InvokePlaybackStop();
				break;
				case PlaybackAction.Pause:
					for (int i = 0; i < usedMixers.Count; i++)
					{
						TrackMixer mixer = usedMixers[i];
						mixer.source.Pause();
					}
					isPlaying = false;
					MusicManager.InvokePlaybackPause(player);
					player.InvokePlaybackPause();
				break;
			}
		}
	}
}
