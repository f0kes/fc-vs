using AI.Unit.Context;
using DefaultNamespace.AI.Unit;
using UnityEngine;

namespace AI.Unit.Behaviours
{
	public class AvoidanceBehavior : FlockBehaviour
	{
		public AvoidanceBehavior(ContextProvider provider) : base(provider)
		{
		}
		public override Vector2 CalculateMove(Vector2 agentPos, FlockParameters parameters)
		{
			var context = Provider.GetContext(agentPos, parameters);
			//if no neighbors, return no adjustment
			if(context.Count == 0) return Vector2.zero;

			//add all points together and average
			var avoidanceMove = Vector2.zero;
			var nAvoid = 0;

			foreach(Vector2 item in context)
			{
				if(!(Vector2.SqrMagnitude(item - agentPos) < parameters.SquareAvoidanceRadius)) continue;
				nAvoid++;
				avoidanceMove += (agentPos - item);
			}
			if(nAvoid > 0) avoidanceMove /= nAvoid;

			return avoidanceMove;
		}

	}
}