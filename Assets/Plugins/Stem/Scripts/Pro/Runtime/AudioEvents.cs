using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	public enum AudioEventType
	{
		Start,
		OnEnable,
		OnDisable,
		OnDestroy,
		OnMouseDown,
		OnMouseEnter,
		OnMouseExit,
		OnMouseUp,
		OnTriggerEnter,
		OnTriggerExit,
		OnTriggerEnter2D,
		OnTriggerExit2D,
		OnCollisionEnter,
		OnCollisionExit,
		OnCollisionEnter2D,
		OnCollisionExit2D,
	}

	[System.Serializable]
	internal class AudioEvent
	{
		[SerializeField]
		internal AudioEventType type = AudioEventType.OnTriggerEnter;

		[SerializeField]
		internal string name = null;

		[SerializeField]
		internal bool expanded = false;

		[SerializeField]
		internal List<AudioEventAction> actions = new List<AudioEventAction>();

		[SerializeField]
		internal List<AudioEventCondition> conditions = new List<AudioEventCondition>();

		internal void Call(GameObject gameObject)
		{
			for (int i = 0; i < conditions.Count; i++)
				if (!conditions[i].Match(gameObject))
					return;

			for (int i = 0; i < actions.Count; i++)
				actions[i].Call();
		}
	}

	public class AudioEvents : MonoBehaviour
	{
		[SerializeField]
		private List<AudioEvent> events = new List<AudioEvent>();

		private void Start() { Call(null, AudioEventType.Start); }
		private void OnEnable() { Call(null, AudioEventType.OnEnable); }
		private void OnDisable() { Call(null, AudioEventType.OnDisable); }
		private void OnDestroy() { Call(null, AudioEventType.OnDestroy); }

		private void OnMouseDown() { Call(null, AudioEventType.OnMouseDown); }
		private void OnMouseEnter() { Call(null, AudioEventType.OnMouseEnter); }
		private void OnMouseExit() { Call(null, AudioEventType.OnMouseExit); }
		private void OnMouseUp() { Call(null, AudioEventType.OnMouseUp); }

		private void OnTriggerEnter(Collider collider)
		{
			Call(collider.gameObject, AudioEventType.OnTriggerEnter);
		}

		private void OnTriggerExit(Collider collider)
		{
			Call(collider.gameObject, AudioEventType.OnTriggerExit);
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			Call(collider.gameObject, AudioEventType.OnTriggerEnter2D);
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			Call(collider.gameObject, AudioEventType.OnTriggerExit2D);
		}

		private void OnCollisionEnter(Collision collision)
		{
			Call(collision.collider.gameObject, AudioEventType.OnCollisionEnter);
		}

		private void OnCollisionExit(Collision collision)
		{
			Call(collision.collider.gameObject, AudioEventType.OnCollisionExit);
		}

		private void OnCollisionEnter(Collision2D collision)
		{
			Call(collision.collider.gameObject, AudioEventType.OnCollisionEnter2D);
		}

		private void OnCollisionExit(Collision2D collision)
		{
			Call(collision.collider.gameObject, AudioEventType.OnCollisionExit2D);
		}

		private void Call(GameObject gameObject, AudioEventType type)
		{
			for (int i = 0; i < events.Count; i++)
			{
				AudioEvent AudioEvent = events[i];
				if (AudioEvent.type == type)
					AudioEvent.Call(gameObject);
			}
		}
	}
}

#endif
