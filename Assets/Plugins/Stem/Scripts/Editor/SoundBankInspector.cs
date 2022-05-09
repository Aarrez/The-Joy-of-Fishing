using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditorInternal;

namespace Stem
{
	internal class SoundBankInspectorViewModel
	{
		private SoundBank soundBank = null;

		private Dictionary<Sound, ReorderableList> reorderableLists = new Dictionary<Sound, ReorderableList>();

		private Dictionary<SoundBus, int> soundBusIndices = new Dictionary<SoundBus, int>();
		private List<string> soundBusNames = new List<string>();
		private string[] soundBusNamesWrapper = null;
		private NameDuplicatesManager soundBusDuplicateManager = null;

		internal SoundBankInspectorViewModel(SoundBank bank)
		{
			soundBusDuplicateManager = new NameDuplicatesManager(null);
			soundBank = bank;

			for (int i = 0; i < bank.Buses.Count; i++)
			{
				SoundBus bus = bank.Buses[i];
				string busName = soundBusDuplicateManager.GrabName(bus.Name);

				soundBusNames.Add(busName);
				soundBusIndices.Add(bus, i);
			}

			soundBusNamesWrapper = soundBusNames.ToArray();

			bank.OnSoundBusAdded += OnSoundBusAdded;
			bank.OnSoundBusRemoved += OnSoundBusRemoved;
			bank.OnSoundBusRenamed += OnSoundBusRenamed;
		}

		internal void Shutdown()
		{
			reorderableLists.Clear();

			soundBusNames.Clear();
			soundBusIndices.Clear();
			soundBusNamesWrapper = null;

			soundBank.OnSoundBusAdded -= OnSoundBusAdded;
			soundBank.OnSoundBusRemoved -= OnSoundBusRemoved;
			soundBank.OnSoundBusRenamed -= OnSoundBusRenamed;
			soundBank = null;
		}

		internal string[] FetchSoundBusNames()
		{
			return soundBusNamesWrapper;
		}

		internal int FetchSoundBusIndex(SoundBus bus)
		{
			if (bus == null)
				return -1;

			int index = -1;
			soundBusIndices.TryGetValue(bus, out index);

			return index;
		}

		internal ReorderableList FetchReorderableList(Sound sound)
		{
			ReorderableList list = null;
			if (!reorderableLists.TryGetValue(sound, out list))
			{
				list = new ReorderableList(sound.VariationsRaw, typeof(SoundVariation), true, true, true, true);
				list.elementHeightCallback = (_1) => { return SoundVariationElementHeight(list, _1); };
				list.drawElementCallback = (_1, _2, _3, _4) => { SoundVariationElement(list, _1, _2, _3, _4); };
				list.drawHeaderCallback = SoundVariationHeader;

				reorderableLists.Add(sound, list);
			}

			return list;
		}

		private void OnSoundBusAdded(SoundBus bus)
		{
			string busName = soundBusDuplicateManager.GrabName(bus.Name);
			soundBusNames.Add(busName);
			soundBusNamesWrapper = soundBusNames.ToArray();

			soundBusIndices.Add(bus, soundBank.Buses.Count - 1);
		}

		private void OnSoundBusRemoved(SoundBus bus, int index)
		{
			soundBusDuplicateManager.ReleaseName(bus.Name);
			soundBusNames.RemoveAt(index);
			soundBusNamesWrapper = soundBusNames.ToArray();

			soundBusIndices.Clear();
			for (int i = 0; i < soundBank.Buses.Count; i++)
				soundBusIndices.Add(soundBank.Buses[i], i);
		}

		private void OnSoundBusRenamed(SoundBus bus, int index, string oldName, string newName)
		{
			soundBusDuplicateManager.ReleaseName(oldName);
			string busName = soundBusDuplicateManager.GrabName(newName);

			soundBusNames[index] = busName;
			soundBusNamesWrapper[index] = busName;
		}

		private static float SoundVariationElementHeight(ReorderableList list, int index)
		{
			SoundVariation variation = (SoundVariation)list.list[index];

			int numLines = 8;

			if (variation.RandomizeVolume)
				numLines += 1;

			if (variation.RandomizePitch)
				numLines += 1;

			if (variation.RandomizeDelay)
				numLines += 1;

			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * numLines;
		}

		private static void SoundVariationHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Variations");
		}

		private static void SoundVariationElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
		{
			SoundVariation v = (SoundVariation)list.list[index];
			float offset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			rect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.LabelField(rect, v.Name, EditorStyles.boldLabel);

			rect.y += offset;
			v.Clip = (AudioClip)EditorGUI.ObjectField(rect, "Clip", v.Clip, typeof(AudioClip), false);

			float fixedValue = v.FixedVolume;
			bool randomize = v.RandomizeVolume;
			Vector2 randomValueRange = v.RandomVolume;

			offset = LayoutUtilities.RandomRangeField("Volume", 0.0f, 1.0f, rect, ref fixedValue, ref randomize, ref randomValueRange);
			rect.y += offset;

			v.FixedVolume = fixedValue;
			v.RandomizeVolume = randomize;
			v.RandomVolume = randomValueRange;

			fixedValue = v.FixedPitch;
			randomize = v.RandomizePitch;
			randomValueRange = v.RandomPitch;

			offset = LayoutUtilities.RandomRangeField("Pitch", -3.0f, 3.0f, rect, ref fixedValue, ref randomize, ref randomValueRange);
			rect.y += offset;

			v.FixedPitch = fixedValue;
			v.RandomizePitch = randomize;
			v.RandomPitch = randomValueRange;

			fixedValue = v.FixedDelay;
			randomize = v.RandomizeDelay;
			randomValueRange = v.RandomDelay;

			offset = LayoutUtilities.RandomRangeField("Delay", 0.0f, 10.0f, rect, ref fixedValue, ref randomize, ref randomValueRange);
			rect.y += offset;

			v.FixedDelay = fixedValue;
			v.RandomizeDelay = randomize;
			v.RandomDelay = randomValueRange;
		}
	}

	[CustomEditor(typeof(SoundBank))]
	internal partial class SoundBankInspector : Editor
	{
		private GUILayoutOption defaultWidth = GUILayout.Width(22);

		private SoundBank bank = null;
		private SoundBankInspectorViewModel viewModel = null;

		public void OnEnable()
		{
			bank = target as SoundBank;
			Clear();
		}

		public void OnDisable()
		{
			Clear();
		}

		public void OnDestroy()
		{
			Clear();
		}

		public override void OnInspectorGUI()
		{
			if (viewModel == null)
				viewModel = new SoundBankInspectorViewModel(bank);

			OnPlayerInit(bank);

			Undo.RecordObject(bank, "Sound Bank Change");
			EditorGUI.BeginChangeCheck();

			OnSoundDropAreaGUI();
			OnSoundBankMemoryManagementGUI();

			bank.ShowSounds = EditorGUILayout.Foldout(bank.ShowSounds, "Sounds");
			if (bank.ShowSounds)
			{
				List<Sound> removedSounds = new List<Sound>();
				foreach (Sound sound in bank.Sounds)
					OnSoundPanelGUI(sound, removedSounds);

				foreach (Sound sound in removedSounds)
					bank.RemoveSound(sound);

				if (GUILayout.Button("Add Sound"))
					bank.AddSound("New Sound");
			}

			bank.ShowSoundBuses = EditorGUILayout.Foldout(bank.ShowSoundBuses, "Buses");
			if (bank.ShowSoundBuses)
			{
				List<SoundBus> removedBuses = new List<SoundBus>();
				foreach (SoundBus bus in bank.Buses)
					OnSoundBusPanelGUI(bus, removedBuses);

				foreach (SoundBus bus in removedBuses)
					bank.RemoveSoundBus(bus);

				if (GUILayout.Button("Add Bus"))
					bank.AddSoundBus("New Bus");
			}

			OnSoundDrop();

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(bank);
		}

		private void Clear()
		{
			if (viewModel != null)
			{
				viewModel.Shutdown();
				viewModel = null;
			}

			OnPlayerShutdown();
		}

		private void OnSoundPanelGUI(Sound sound, List<Sound> removedSounds)
		{
			EditorGUILayout.BeginHorizontal();

			sound.Unfolded = GUILayout.Toggle(sound.Unfolded, "", "foldout", GUILayout.ExpandWidth(false));
			sound.Name = EditorGUILayout.TextField(sound.Name, GUILayout.ExpandWidth(true));

			int index = viewModel.FetchSoundBusIndex(sound.Bus);
			index = EditorGUILayout.Popup(index, viewModel.FetchSoundBusNames(), GUILayout.ExpandWidth(true));

			OnPlayerPlay(sound);

			sound.Bus = (index != -1) ? bank.Buses[index] : bank.DefaultBus;
			sound.Muted = GUILayout.Toggle(sound.Muted, "M", "button", defaultWidth, GUILayout.ExpandWidth(false));
			sound.Soloed = GUILayout.Toggle(sound.Soloed, "S", "button", defaultWidth, GUILayout.ExpandWidth(false));

			if (GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
				removedSounds.Add(sound);

			EditorGUILayout.EndHorizontal();

			if (sound.Unfolded)
			{
				EditorGUILayout.BeginVertical("groupbox");

				sound.Volume = EditorGUILayout.Slider("Volume", sound.Volume, 0.0f, 1.0f);
				EditorGUILayout.Space();

				sound.PanStereo = LayoutUtilities.SliderWithLabels("Stereo Pan", sound.PanStereo, "Left", "Right", -1.0f, 1.0f);
				sound.SpatialBlend = LayoutUtilities.SliderWithLabels("Spatial Blend", sound.SpatialBlend, "2D", "3D", 0.0f, 1.0f);

				sound.DopplerLevel = EditorGUILayout.Slider("Doppler Level", sound.DopplerLevel, 0.0f, 5.0f);
				EditorGUILayout.Space();

				sound.Spread = EditorGUILayout.Slider("Spread", sound.Spread, 0.0f, 360.0f);
				EditorGUILayout.Space();

				sound.AttenuationMode = (AttenuationMode)EditorGUILayout.EnumPopup("Volume Rolloff", (System.Enum)sound.AttenuationMode);
				sound.MinDistance = EditorGUILayout.FloatField("Min Distance", Mathf.Max(0.0f, sound.MinDistance));
				sound.MaxDistance = EditorGUILayout.FloatField("Max Distance", Mathf.Max(0.0f, sound.MaxDistance));

				sound.VariationRetriggerMode = (RetriggerMode)EditorGUILayout.EnumPopup("Retrigger Mode", (System.Enum)sound.VariationRetriggerMode);

				OnSoundMemoryManagementGUI(sound);

				ReorderableList list = viewModel.FetchReorderableList(sound);
				list.DoLayoutList();

				EditorGUILayout.EndVertical();
			}
		}

		private void OnSoundBusPanelGUI(SoundBus bus, List<SoundBus> removedBuses)
		{
			EditorGUILayout.BeginHorizontal();

			bus.Unfolded = GUILayout.Toggle(bus.Unfolded, "", "foldout", GUILayout.ExpandWidth(false));
			bus.Name = EditorGUILayout.TextField(bus.Name, GUILayout.ExpandWidth(true));
			bus.Volume = EditorGUILayout.Slider(bus.Volume, 0.0f, 1.0f);

			bus.Muted = GUILayout.Toggle(bus.Muted, "M", "button", defaultWidth, GUILayout.ExpandWidth(false));
			bus.Soloed = GUILayout.Toggle(bus.Soloed, "S", "button", defaultWidth, GUILayout.ExpandWidth(false));

			bool canRemove = (bus != bank.DefaultBus);
			if (canRemove && GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
				removedBuses.Add(bus);

			EditorGUILayout.EndHorizontal();

			if (bus.Unfolded)
			{
				EditorGUILayout.BeginVertical("groupbox");

				bus.MixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField("Output", bus.MixerGroup, typeof(AudioMixerGroup), false);
				bus.Polyphony = (byte)EditorGUILayout.IntSlider("Polyphony", bus.Polyphony, 1, 32);
				bus.AllowVoiceStealing = EditorGUILayout.Toggle("Allow Voice Stealing", bus.AllowVoiceStealing);

				OnSoundBusAdditionalGUI(bus);

				EditorGUILayout.EndVertical();
			}
		}
	}
}
