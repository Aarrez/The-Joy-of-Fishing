using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SampleSelector : MonoBehaviour
{
	public string[] scenes = null;

	private Dropdown cachedDropdown = null;
	private bool initialized = false;

	private void Awake()
	{
		cachedDropdown = GetComponent<Dropdown>();
		if (cachedDropdown != null && scenes != null && scenes.Length > 0)
		{
			cachedDropdown.options.Clear();

			int sceneIndex = 0;
			for (int i = 0; i < scenes.Length; i++)
			{
				if (scenes[i] == gameObject.scene.name)
					sceneIndex = i;

				Dropdown.OptionData data = new Dropdown.OptionData();
				data.text = scenes[i];

				cachedDropdown.options.Add(data);
			}

			cachedDropdown.captionText.text = scenes[sceneIndex];
			cachedDropdown.value = sceneIndex;
		}
		initialized = true;
	}

	public void SwitchScene()
	{
		if (!initialized)
			return;

		int index = cachedDropdown.value;
		if (index < 0 || index >= scenes.Length)
			return;

		SceneManager.LoadScene(scenes[index]);
	}
}
