using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Stem
{
	internal class NameDuplicatesManager
	{
		private Dictionary<string, int> nameDuplicates = new Dictionary<string, int>();
		private Func<string, string> decorator = null;

		internal NameDuplicatesManager(Func<string, string> dec)
		{
			decorator = dec;
		}

		internal string GrabName(string name)
		{
			string decoratedName = (decorator != null) ? decorator(name) : name;

			if (nameDuplicates.ContainsKey(name))
			{
				int duplicateNumber = ++nameDuplicates[name];
				decoratedName = string.Format("{0}[{1}]", decoratedName, duplicateNumber);
			}
			else
				nameDuplicates.Add(name, 0);

			return decoratedName;
		}

		internal void ReleaseName(string name)
		{
			if (!nameDuplicates.ContainsKey(name))
				return;

			if(--nameDuplicates[name] < 0)
				nameDuplicates.Remove(name);
		}
	}

	internal static class LayoutUtilities
	{
		internal static float RandomRangeField(string name, float min, float max, Rect rect, ref float fixedValue, ref bool randomize, ref Vector2 randomRange)
		{
			float offset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			int numLines = (randomize) ? 3 : 2;

			rect.y += offset;
			randomize = EditorGUI.Toggle(rect, string.Format("Randomize {0}", name), randomize);

			if (randomize)
			{
				rect.y += offset;

				string label = string.Format("{0} Range", name);
				EditorGUI.MinMaxSlider(rect, new GUIContent(label), ref randomRange.x, ref randomRange.y, min, max);

				rect.y += offset;
				rect.x += EditorGUIUtility.labelWidth;
				rect.width -= EditorGUIUtility.labelWidth;

				float lineWidth = rect.width;
				float lineStart = rect.x;

				float labelWidth = 30;
				float fieldWidth = EditorGUIUtility.fieldWidth + 10;
				float scale = Mathf.Clamp01(lineWidth / (2.0f * (labelWidth + fieldWidth)));

				labelWidth *= scale;
				fieldWidth *= scale;

				rect.x = lineStart;
				rect.width = labelWidth;
				EditorGUI.LabelField(rect, "Min");

				rect.x += labelWidth;
				rect.width = fieldWidth;
				randomRange.x = EditorGUI.FloatField(rect, randomRange.x);
				randomRange.x = Mathf.Min(randomRange.x, randomRange.y);

				rect.x = lineStart + lineWidth - labelWidth - fieldWidth;
				rect.width = labelWidth;
				EditorGUI.LabelField(rect, "Max");

				rect.x += labelWidth;
				rect.width = fieldWidth;
				randomRange.y = EditorGUI.FloatField(rect, randomRange.y);
				randomRange.y = Mathf.Max(randomRange.x, randomRange.y);
			}
			else
			{
				rect.y += offset;
				fixedValue = EditorGUI.Slider(rect, name, fixedValue, min, max);
			}

			return numLines * offset;
		}

		internal static float SliderWithLabels(string name, float value, string leftLabel, string rightLabel, float min, float max)
		{
			Rect position = EditorGUILayout.GetControlRect(false, 1.5f * EditorGUIUtility.singleLineHeight);
			position.height = EditorGUIUtility.singleLineHeight;

			float result = EditorGUI.Slider(position, name, value, min, max);

			position.height = EditorGUIUtility.singleLineHeight;
			position.y += position.height * 0.75f;
			position.x += EditorGUIUtility.labelWidth;
			position.width -= EditorGUIUtility.labelWidth + 54;

			GUIStyle style = GUI.skin.label;
			bool oldEnabled = GUI.enabled;
			GUI.enabled = false;

			style.fontSize = 9;
			style.alignment = TextAnchor.UpperLeft;
			EditorGUI.LabelField (position, leftLabel, style);

			style.alignment = TextAnchor.UpperRight;
			EditorGUI.LabelField (position, rightLabel, style);

			GUI.enabled = oldEnabled;

			return result;
		}
	}
}
