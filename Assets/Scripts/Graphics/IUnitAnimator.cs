using Enums;
using UnityEngine;

namespace Graphics
{
	public interface IUnitAnimator
	{
		public void SetAnimation(UnitAnimationType animationType,bool forceInterrupt=false, bool loop = true, float time = 1f);
		public Color ColorAdd { get; set; }
		public int CurrentFrameIndex { get; }
		
	}
}