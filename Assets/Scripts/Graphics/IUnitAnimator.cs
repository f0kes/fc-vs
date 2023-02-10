using Enums;

namespace Graphics
{
	public interface IUnitAnimator
	{
		public void SetAnimation(UnitAnimationType animationType,bool forceInterrupt=false, bool loop = true, float time = 1f);
		public int CurrentFrameIndex { get; }
		
	}
}