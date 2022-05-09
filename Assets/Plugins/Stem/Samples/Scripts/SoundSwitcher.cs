using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSwitcher : MonoBehaviour
{
	public string sound2DName = null;
	public string sound3DName = null;

	private bool use3D = false;
	private SoundSpawner spawner = null;

	private void Awake()
	{
		spawner = GetComponent<SoundSpawner>();
		Set3D(use3D);
	}

	public void Set3D(bool value)
	{
		use3D = value;
		if (spawner)
			spawner.soundName = (use3D) ? sound3DName : sound2DName;
	}
}
