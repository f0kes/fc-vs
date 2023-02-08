using System.Collections.Generic;
using DefaultNamespace.AI.Unit;
using UnityEngine;

namespace AI.Unit.Context
{
	public class EmptyProvider : ContextProvider
	{

		public override List<Vector2> GetContext(Vector2 agentPos, FlockParameters parameters)
		{
			return new List<Vector2>();
		}
	}
}