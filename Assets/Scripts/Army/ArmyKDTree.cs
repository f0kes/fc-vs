using System.Collections.Generic;
using System.Linq;
using Datastructures.KDTree;
using DataStructures.ViliWonka.KDTree;
using GameState;
using UnityEngine;

namespace Army
{
	public class ArmyKDTree
	{
		private List<Unit> _units;
		private KDTree _kdTree = new KDTree();
		private KDQuery _kdQuery = new KDQuery();
		public ArmyKDTree(List<Unit> units)
		{
			_units = units;
			//TODO:build here
		}
		public List<Unit> GetNearestUnits(Vector2 position, float radius)
		{
			var result = new List<int>();
			var nearestUnits = new List<Unit>();
			_kdQuery.KNearest(_kdTree, position, 10, result);
			foreach(var index in result)
			{
				nearestUnits.Add(_units[index]);
			}
			return nearestUnits;
		}
		public List<Unit> SortByDistance(List<Unit> units, Vector2 position)
		{
			return units.OrderBy(u => (position - u.Position).sqrMagnitude).ToList();
		}
		public List<Unit> GetNearestUnitsRadius(Vector2 position, float radius)
		{
			var result = new List<int>();
			var nearestUnits = new List<Unit>();

			_kdQuery.Radius(_kdTree, position, radius, result);
			foreach(var index in result)
			{
				nearestUnits.Add(_units[index]);
			}
			return nearestUnits;
		}

		public void Build(List<Vector2> positions)
		{
			_kdTree.Build(positions);
		}
	}
}