using AI.Unit.Context;
using DefaultNamespace.AI.Unit;
using UnityEngine;

namespace AI.Unit.Behaviours
{
	public class FollowPointBehaviour : FlockBehaviour
	{
		public FollowPointBehaviour(ContextProvider provider) : base(provider)
		{
		}
		public override Vector2 CalculateMove(Vector2 agentPos, FlockParameters parameters)
		{
			var context = Provider.GetContext(agentPos, parameters);
			var move = Vector2.zero;
			var target = context[0];
			var distance = (target - agentPos).sqrMagnitude;
			if(distance > 0.7f)
			{
				move = (target - agentPos);
			}
			return move;
		}

	}
}