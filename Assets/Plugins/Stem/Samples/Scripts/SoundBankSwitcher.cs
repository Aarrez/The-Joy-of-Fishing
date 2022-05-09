using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBankSwitcher : MonoBehaviour
{
	public Stem.SoundBank[] skins = null;
	public Dropdown dropdown = null;

	private void Start()
	{
		if (skins.Length > 0)
			Stem.SoundManager.PrimaryBank = skins[0];

		if (dropdown != null && skins.Length > 0)
		{
			dropdown.options.Clear();
			for (int i = 0; i < skins.Length; i++)
			{
				Dropdown.OptionData data = new Dropdown.OptionData();
				data.text = skins[i].name;

				dropdown.options.Add(data);
			}

			dropdown.captionText.text = skins[0].name;
			dropdown.value = 0;
		}
	}

	public void SetBank(int index)
	{
		if (dropdown == null)
			return;

		if (skins == null)
			return;

		Stem.SoundManager.PrimaryBank = skins[index];
	}
}
