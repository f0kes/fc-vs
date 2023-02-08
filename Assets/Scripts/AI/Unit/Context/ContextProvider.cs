using System.Collections.Generic;
using DefaultNamespace.AI.Unit;
using UnityEngine;

namespace AI.Unit.Context
{
	public abstract class ContextProvider
	{
		public abstract List<Vector2> GetContext(Vector2 agentPos, FlockParameters parameters);
	}
}