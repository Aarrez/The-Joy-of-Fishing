using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBusControls : MonoBehaviour
{
	public Text text = null;

	private bool useVoiceStealing = false;
	private SoundSpawner spawner = null;

	private void Awake()
	{
		spawner = GetComponent<SoundSpawner>();
	}

	private void Start()
	{
		SetVoiceStealing(useVoiceStealing);
	}

	public void SetVoiceStealing(bool value)
	{
		useVoiceStealing = value;

		if (spawner == null)
			return;

		Stem.Sound sound = Stem.SoundManager.GetSound(spawner.soundName);
		if (sound == null)
			return;

		Stem.SoundBus bus = sound.Bus;
		if (bus != null)
			bus.AllowVoiceStealing = useVoiceStealing;

		if (text != null)
			text.text = string.Format("Bus \"{0}\", Voices: {1}", bus.Name, bus.Polyphony);
	}
}
