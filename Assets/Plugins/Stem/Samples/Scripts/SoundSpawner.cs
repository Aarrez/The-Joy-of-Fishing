using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSpawner : MonoBehaviour
{
	public int gridMix = -2;
	public int gridMax = 2;
	public float gridStep = 1.0f;

	public Transform listener = null;

	public string soundName = null;
	public GameObject spawnPrefab = null;
	public float spawnInterval = 1.0f;

	private float GetSoundLength(string name)
	{
		Stem.Sound sound = Stem.SoundManager.GetSound(name);
		if (sound == null)
			return 0.0f;

		if (sound.Variations.Count == 0)
			return 0.0f;

		float soundLength = 0.0f;
		for (int i = 0; i < sound.Variations.Count; i++)
		{
			AudioClip clip = sound.Variations[i].Clip;
			if (clip == null)
				continue;

			soundLength = Mathf.Max(soundLength, clip.length);
		}

		return soundLength;
	}

	private Vector3 GetRandomGridPosition(ref int x, ref int z)
	{
		int newX = x;
		int newZ = z;

		while((newX == 0 && newZ == 0) || (newX == x && newZ == z))
		{
			newX = UnityEngine.Random.Range(gridMix, gridMax + 1);
			newZ = UnityEngine.Random.Range(gridMix, gridMax + 1);
		}

		x = newX;
		z = newZ;

		return new Vector3(x, 0.0f, z) * gridStep;
	}

	private void Spawn(float lifetime, Vector3 position)
	{
		if (spawnPrefab == null)
			return;

		Quaternion orientation = Quaternion.identity;

		if (listener != null)
			orientation = Quaternion.LookRotation(listener.position - position, Vector3.up);

		GameObject prefab = GameObject.Instantiate(spawnPrefab, position, orientation) as GameObject;
		GameObject.Destroy(prefab, lifetime);
	}

	private IEnumerator Start()
	{
		int x = 0;
		int z = 0;

		while (true)
		{
			float soundLength = GetSoundLength(soundName);
			Vector3 position = GetRandomGridPosition(ref x, ref z);

			Stem.SoundManager.Play3D(soundName, position);
			Spawn(soundLength, position);

			yield return new WaitForSeconds(spawnInterval);
		}
	}
}
