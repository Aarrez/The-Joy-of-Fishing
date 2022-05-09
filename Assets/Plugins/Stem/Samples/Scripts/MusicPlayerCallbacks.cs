using UnityEngine;

public class MusicPlayerCallbacks : MonoBehaviour
{
	[Stem.MusicPlayerID]
	public Stem.ID id = Stem.ID.None;

	private Stem.MusicPlayer cachedPlayer;

	private void Start()
	{
		cachedPlayer = Stem.MusicManager.GetMusicPlayer(id);
		if (cachedPlayer != null)
		{
			cachedPlayer.OnPlaybackStarted += OnPlaybackStarted;
			cachedPlayer.OnPlaybackStopped += OnPlaybackStopped;
			cachedPlayer.OnPlaybackPaused += OnPlaybackPaused;
			cachedPlayer.OnTrackChanged += OnTrackChanged;
		}
	}

	private void OnDestroy()
	{
		Stem.MusicManager.Stop(id);

		if (cachedPlayer != null)
		{
			cachedPlayer.OnPlaybackStarted -= OnPlaybackStarted;
			cachedPlayer.OnPlaybackStopped -= OnPlaybackStopped;
			cachedPlayer.OnPlaybackPaused -= OnPlaybackPaused;
			cachedPlayer.OnTrackChanged -= OnTrackChanged;
		}
	}

	private void OnPlaybackStarted(Stem.MusicPlayer player)
	{
		Debug.LogFormat("[Local Callback] {0}: playback started", player.Name);
	}

	private void OnPlaybackStopped(Stem.MusicPlayer player)
	{
		Debug.LogFormat("[Local Callback] {0}: playback stopped", player.Name);
	}

	private void OnPlaybackPaused(Stem.MusicPlayer player)
	{
		Debug.LogFormat("[Local Callback] {0}: playback paused", player.Name);
	}

	private void OnTrackChanged(Stem.MusicPlayer player, Stem.PlaylistTrack oldTrack, Stem.PlaylistTrack newTrack)
	{
		string oldTrackName = (oldTrack != null) ? oldTrack.Name : "none";
		string newTrackName = (newTrack != null) ? newTrack.Name : "none";

		Debug.LogFormat("[Local Callback] {0}: track changed from {1} to {2}", player.Name, oldTrackName, newTrackName);
	}
}
