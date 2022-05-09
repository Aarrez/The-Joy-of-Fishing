using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSound : MonoBehaviour
{
	public Transform listener = null;

	public float radius = 2.5f;
	public float phase = 0.0f;
	public float baseHeight = 0.5f;
	public float heightVariation = 0.1f;
	public string soundName = null;

	private Stem.SoundInstance soundInstance = null;

	private void Start()
	{
		soundInstance = Stem.SoundManager.GrabSound(soundName);

		if (soundInstance != null)
		{
			soundInstance.Looped = true;
			soundInstance.Target = transform;
			soundInstance.Play();
		}
	}

	private void Update()
	{
		float time = Time.realtimeSinceStartup;

		float x = Mathf.Cos(time + phase) * radius;
		float z = Mathf.Sin(time + phase) * radius;
		float y = baseHeight + Mathf.Sin(3.0f * time + phase) * heightVariation;

		Vector3 position = new Vector3(x, y, z);
		Vector3 direction = new Vector3(-z, 0.0f, x);

		if (listener != null)
			direction = listener.position - position;

		transform.position = position;
		transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
	}

	private void OnDestroy()
	{
		if (soundInstance == null)
			return;

		Stem.SoundManager.ReleaseSound(soundInstance);
		soundInstance = null;
	}
}
