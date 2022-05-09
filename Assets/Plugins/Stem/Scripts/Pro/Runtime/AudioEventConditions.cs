using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	public enum AudioEventConditionType
	{
		Tag,
		LayerMask,
		Name,
		GameObject,
	}

	[System.Serializable]
	internal class AudioEventCondition
	{
		[SerializeField]
		internal AudioEventConditionType type = AudioEventConditionType.Tag;

		[SerializeField]
		internal string nameOrTag = null;

		[SerializeField]
		internal LayerMask layerMask = 0;

		[SerializeField]
		internal GameObject gameObject = null;

		public bool Match(GameObject obj)
		{
			switch (type)
			{
				case AudioEventConditionType.Tag: return obj.tag == nameOrTag;
				case AudioEventConditionType.LayerMask: return ((1 << obj.layer) & layerMask) != 0;
				case AudioEventConditionType.Name: return obj.name == nameOrTag;
				case AudioEventConditionType.GameObject: return obj == gameObject;
				default:
				{
					Debug.LogErrorFormat("AudioEventCondition.Match(): unsupported filter type {0}", type.ToString());
					return false;
				}
			}
		}
	}
}

#endif
