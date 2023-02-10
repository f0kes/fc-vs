using System;
using Enums;

namespace Graphics
{
	[Serializable]
	public struct UnitAnimation
	{
		public UnitAnimationType Type;
		public int[] Frames;
		public bool Interruptable;
	}
}