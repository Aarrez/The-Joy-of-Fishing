using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !STEM_DEBUG_SKIP_PRO

namespace Stem
{
	public partial class SoundBusRuntime
	{
		private float currentPlayCooldown = 0.0f;

		private bool IsPlayLimited()
		{
			return currentPlayCooldown > 0.0f;
		}

		private void ResetPlayLimit()
		{
			currentPlayCooldown = bus.PlayLimitInterval;
		}

		private void UpdatePlayLimit(float deltaTime)
		{
			currentPlayCooldown -= deltaTime;
		}
	}
}

#endif
