using System.Collections.Generic;
using AI.Unit.Context;
using DefaultNamespace.AI.Unit;
using UnityEngine;

namespace AI.Unit.Behaviours
{
	public class CompositeBehaviour : FlockBehaviour
	{
		public WeightedBehaviour[] behaviors;

		public CompositeBehaviour(ContextProvider provider, List<WeightedBehaviour> weightedBehaviours) : base(provider)
		{
			behaviors = weightedBehaviours.ToArray();
		}
		public override Vector2 CalculateMove(Vector2 agentPos, FlockParameters parameters)
		{
			var context = Provider.GetContext(agentPos, parameters);

			var move = Vector2.zero;

			//iterate through behaviors
			for(int i = 0; i < behaviors.Length; i++)
			{
				var behavior = behaviors[i].Behaviour;
				var weight = behaviors[i].Weight;

				var partialMove = behavior.CalculateMove(agentPos, parameters) * weight;
				if(partialMove != Vector2.zero)
				{
					if(partialMove.sqrMagnitude > weight * weight)
					{
						partialMove.Normalize();
						partialMove *= weight;
					}

					move += partialMove;
				}
			}

			return move;
		}

	}
}