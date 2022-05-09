using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	internal class AudioEventsInspectorViewModel
	{
		internal SerializedObject serializedObject;
		internal List<ReorderableList> actionLists = new List<ReorderableList>();
		internal List<ReorderableList> conditionLists = new List<ReorderableList>();

		internal void Init(SerializedObject obj)
		{
			actionLists.Clear();
			conditionLists.Clear();

			serializedObject = obj;

			SerializedProperty events = serializedObject.FindProperty("events");

			for (int i = 0; i < events.arraySize; i++)
				OnAudioEventAdded(events.GetArrayElementAtIndex(i));
		}

		internal void OnAudioEventAdded(SerializedProperty audioEvent)
		{
			SerializedProperty actions = audioEvent.FindPropertyRelative("actions");
			SerializedProperty conditions = audioEvent.FindPropertyRelative("conditions");

			ReorderableList actionList = new ReorderableList(serializedObject, actions, true, true, true, true);

			actionList.elementHeightCallback = (_1) => { return AudioEventsInspector.SoundActionElementHeight(actionList, _1); };
			actionList.drawElementCallback = (_1, _2, _3, _4) => { AudioEventsInspector.SoundActionElement(actionList, _1, _2, _3, _4); };
			actionList.drawHeaderCallback = AudioEventsInspector.SoundActionHeader;

			actionLists.Add(actionList);

			ReorderableList conditionList = new ReorderableList(serializedObject, conditions, true, true, true, true);
			conditionList.elementHeightCallback = (_1) => { return AudioEventsInspector.SoundConditionElementHeight(conditionList, _1); };
			conditionList.drawElementCallback = (_1, _2, _3, _4) => { AudioEventsInspector.SoundConditionElement(conditionList, _1, _2, _3, _4); };
			conditionList.drawHeaderCallback = AudioEventsInspector.SoundConditionHeader;

			conditionLists.Add(conditionList);
		}

		internal void OnAudioEventRemoved(int index)
		{
			actionLists.RemoveAt(index);
			conditionLists.RemoveAt(index);
		}
		

		internal static bool HasConditions(AudioEventType type)
		{
			switch (type)
			{
				case AudioEventType.Start:
				case AudioEventType.OnEnable:
				case AudioEventType.OnDisable:
				case AudioEventType.OnDestroy:
				case AudioEventType.OnMouseDown:
				case AudioEventType.OnMouseEnter:
				case AudioEventType.OnMouseExit:
				case AudioEventType.OnMouseUp: return false;
				case AudioEventType.OnTriggerEnter:
				case AudioEventType.OnTriggerExit:
				case AudioEventType.OnTriggerEnter2D:
				case AudioEventType.OnTriggerExit2D:
				case AudioEventType.OnCollisionEnter:
				case AudioEventType.OnCollisionExit:
				case AudioEventType.OnCollisionEnter2D:
				case AudioEventType.OnCollisionExit2D: return true;
				default:
				{
					Debug.LogErrorFormat("AudioEventsInspectorViewModel.HasConditions(): unknown event type \"{0}\"", type.ToString());
					return false;
				}
			}
		}
	}

	[CustomEditor(typeof(AudioEvents))]
	internal class AudioEventsInspector : Editor
	{
		private GUILayoutOption defaultWidth = GUILayout.Width(22);
		private AudioEventsInspectorViewModel viewModel = new AudioEventsInspectorViewModel();

		public void OnEnable()
		{
			viewModel.Init(serializedObject);
		}

		public override void OnInspectorGUI()
		{
			SerializedProperty events = serializedObject.FindProperty("events");
			if (events == null)
				return;

			for (int i = 0; i < events.arraySize; i++)
				OnAudioEventGUI(events, i);

			if (GUILayout.Button("Add Event"))
				AddAudioEvent(events);

			serializedObject.ApplyModifiedProperties();
		}

		private void AddAudioEvent(SerializedProperty events)
		{
			events.InsertArrayElementAtIndex(events.arraySize);

			SerializedProperty newElement = events.GetArrayElementAtIndex(events.arraySize - 1);

			SerializedProperty expanded = newElement.FindPropertyRelative("expanded");
			SerializedProperty name = newElement.FindPropertyRelative("name");
			SerializedProperty type = newElement.FindPropertyRelative("type");

			expanded.boolValue = true;
			name.stringValue = "New Event";
			type.enumValueIndex = (int)AudioEventType.Start;

			viewModel.OnAudioEventAdded(newElement);
		}

		private void RemoveAudioEvent(SerializedProperty events, int index)
		{
			events.DeleteArrayElementAtIndex(index);
			viewModel.OnAudioEventRemoved(index);
		}

		internal static void TraverseAudioEventAction(SerializedProperty action, Action<SerializedProperty> callback)
		{
			// Find proper sound action instance
			SerializedProperty start = action.FindPropertyRelative("type");
			int numSteps = start.enumValueIndex;

			do
			{
				start.NextVisible(false);
			} while (numSteps-- > 0);

			// Iterate for each serialized field
			SerializedProperty end = start.GetEndProperty();
			start.NextVisible(true);

			while(!SerializedProperty.EqualContents(start, end))
			{
				if (callback != null)
					callback(start);

				start.NextVisible(true);
			}
		}

		internal static float SoundConditionElementHeight(ReorderableList list, int index)
		{
			int numLines = 1;

			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * numLines;
		}

		internal static void SoundConditionHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Conditions");
		}

		internal static void SoundConditionElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty conditions = list.serializedProperty;
			SerializedProperty condition = conditions.GetArrayElementAtIndex(index);
			SerializedProperty typeProp = condition.FindPropertyRelative("type");
			AudioEventConditionType type = (AudioEventConditionType)typeProp.enumValueIndex;

			rect.height = EditorGUIUtility.singleLineHeight;
			rect.width /= 2;

			EditorGUI.PropertyField(rect, typeProp, GUIContent.none, false);

			rect.x += rect.width;

			switch (type)
			{
				case AudioEventConditionType.Name:
				case AudioEventConditionType.Tag: EditorGUI.PropertyField(rect, condition.FindPropertyRelative("nameOrTag"), GUIContent.none, false); break;
				case AudioEventConditionType.LayerMask: EditorGUI.PropertyField(rect, condition.FindPropertyRelative("layerMask"), GUIContent.none, false); break;
				case AudioEventConditionType.GameObject: EditorGUI.PropertyField(rect, condition.FindPropertyRelative("gameObject"), GUIContent.none, false); break;
			}
		}

		internal static float SoundActionElementHeight(ReorderableList list, int index)
		{
			int numLines = 1;

			SerializedProperty actions = list.serializedProperty;
			SerializedProperty action = actions.GetArrayElementAtIndex(index);

			TraverseAudioEventAction(action, (x) => { numLines++; });
			numLines++; // extra line for better UI readability

			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * numLines;
		}

		internal static void SoundActionHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Actions");
		}

		internal static void SoundActionElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty actions = list.serializedProperty;

			SerializedProperty action = actions.GetArrayElementAtIndex(index);
			SerializedProperty typeProp = action.FindPropertyRelative("type");

			float offset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			rect.height = EditorGUIUtility.singleLineHeight;

			EditorGUI.PropertyField(rect, typeProp, GUIContent.none, false);
			rect.y += offset;

			TraverseAudioEventAction(action,
				(x) =>
				{
					EditorGUI.PropertyField(rect, x, false);
					rect.y += offset;
				}
			);
		}

		private void OnAudioEventGUI(SerializedProperty events, int index)
		{
			SerializedProperty audioEvent = events.GetArrayElementAtIndex(index);
			if (audioEvent == null)
				return;

			SerializedProperty expanded = audioEvent.FindPropertyRelative("expanded");
			SerializedProperty name = audioEvent.FindPropertyRelative("name");
			SerializedProperty type = audioEvent.FindPropertyRelative("type");

			SerializedProperty actions = audioEvent.FindPropertyRelative("actions");
			SerializedProperty conditions = audioEvent.FindPropertyRelative("conditions");

			if (expanded == null || name == null || type == null || actions == null || conditions == null)
				return;

			EditorGUILayout.BeginHorizontal();

			expanded.boolValue = GUILayout.Toggle(expanded.boolValue, GUIContent.none, "foldout", GUILayout.ExpandWidth(false));
			name.stringValue = EditorGUILayout.TextField(name.stringValue, GUILayout.ExpandWidth(true));
			type.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup((AudioEventType)type.enumValueIndex));

			if (GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
			{
				EditorGUILayout.EndHorizontal();
				RemoveAudioEvent(events, index);
				return;
			}

			EditorGUILayout.EndHorizontal();

			if (expanded.boolValue)
			{
				EditorGUILayout.BeginVertical("groupbox");

				if (AudioEventsInspectorViewModel.HasConditions((AudioEventType)type.enumValueIndex))
				{
					ReorderableList conditionList = viewModel.conditionLists[index];
					conditionList.DoLayoutList();
				}
				else
					conditions.ClearArray();

				ReorderableList actionList = viewModel.actionLists[index];
				actionList.DoLayoutList();

				EditorGUILayout.EndVertical();
			}
		}
	}
}

#endif
