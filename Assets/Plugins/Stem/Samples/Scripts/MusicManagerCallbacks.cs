using UnityEngine;

public class MusicManagerCallbacks : MonoBehaviour
{
	private void Start()
	{
		Stem.MusicManager.OnPlaybackStarted += OnPlaybackStarted;
		Stem.MusicManager.OnPlaybackStopped += OnPlaybackStopped;
		Stem.MusicManager.OnPlaybackPaused += OnPlaybackPaused;
		Stem.MusicManager.OnTrackChanged += OnTrackChanged;
	}

	private void OnDestroy()
	{
		Stem.MusicManager.OnPlaybackStarted -= OnPlaybackStarted;
		Stem.MusicManager.OnPlaybackStopped -= OnPlaybackStopped;
		Stem.MusicManager.OnPlaybackPaused -= OnPlaybackPaused;
		Stem.MusicManager.OnTrackChanged -= OnTrackChanged;
	}

	private void OnPlaybackStarted(Stem.MusicPlayer player)
	{
		Debug.LogFormat("[Global Callback] {0}: playback started", player.Name);
	}

	private void OnPlaybackStopped(Stem.MusicPlayer player)
	{
		Debug.LogFormat("[Global Callback] {0}: playback stopped", player.Name);
	}

	private void OnPlaybackPaused(Stem.MusicPlayer player)
	{
		Debug.LogFormat("[Global Callback] {0}: playback paused", player.Name);
	}

	private void OnTrackChanged(Stem.MusicPlayer player, Stem.PlaylistTrack oldTrack, Stem.PlaylistTrack newTrack)
	{
		string oldTrackName = (oldTrack != null) ? oldTrack.Name : "none";
		string newTrackName = (newTrack != null) ? newTrack.Name : "none";

		Debug.LogFormat("[Global Callback] {0}: track changed from {1} to {2}", player.Name, oldTrackName, newTrackName);
	}
}
