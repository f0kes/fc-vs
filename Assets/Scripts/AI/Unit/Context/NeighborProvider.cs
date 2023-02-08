using System.Collections.Generic;
using System.Linq;
using Army;
using DefaultNamespace.AI.Unit;
using UnityEngine;

namespace AI.Unit.Context
{
	public class NeighborProvider : ContextProvider
	{
		private readonly ArmyKDTree _armyKDTree;
		public NeighborProvider(ArmyKDTree armyKDTree)
		{
			_armyKDTree = armyKDTree;
		}

		public override List<Vector2> GetContext(Vector2 agentPos, FlockParameters parameters)
		{
			return _armyKDTree.GetNearestUnits(agentPos, parameters.neighborRadius)
				.Select(u => u.Position)
				.ToList();
		}
	}
}