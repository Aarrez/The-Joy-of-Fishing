using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonAction
{
	Play,
	Stop,
	Pause,
	Next,
	Prev,
}

public class MusicPlayerButton : MonoBehaviour
{
	[Stem.MusicPlayerID]
	public Stem.ID musicPlayer = Stem.ID.None;
	public float smoothness = 5.0f;
	public float pressedScale = 0.1f;
	public Sprite sprite = null;
	public ButtonAction action = ButtonAction.Play;

	private Vector3 originalScale = Vector3.zero;
	private SpriteRenderer cachedSpriteRenderer = null;

	private void Awake()
	{
		originalScale = transform.localScale;
		cachedSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		if (cachedSpriteRenderer != null)
			cachedSpriteRenderer.sprite = sprite;
	}

	private void Update()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * smoothness);
	}

	private void OnMouseDown()
	{
		Vector3 scale = transform.localScale;
		scale.y *= pressedScale;

		transform.localScale = scale;

		switch (action)
		{
			case ButtonAction.Play: Stem.MusicManager.Play(musicPlayer); break;
			case ButtonAction.Stop: Stem.MusicManager.Stop(musicPlayer); break;
			case ButtonAction.Pause: Stem.MusicManager.Pause(musicPlayer); break;
			case ButtonAction.Next: Stem.MusicManager.Next(musicPlayer); break;
			case ButtonAction.Prev: Stem.MusicManager.Prev(musicPlayer); break;
		}
	}
}
