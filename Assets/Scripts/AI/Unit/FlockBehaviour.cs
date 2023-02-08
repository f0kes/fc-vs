using AI.Unit.Context;
using DefaultNamespace.AI.Unit;
using UnityEngine;

namespace AI.Unit
{
	public abstract class FlockBehaviour 
	{
		protected readonly ContextProvider Provider;
		
		protected FlockBehaviour( ContextProvider provider)
		{
			Provider = provider;
		}
		public abstract Vector2 CalculateMove(Vector2 agentPos, FlockParameters parameters);
		
	}
}