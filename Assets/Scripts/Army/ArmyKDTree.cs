using System.Collections.Generic;
using System.Linq;
using Army.Units;
using Datastructures.KDTree;
using DataStructures.ViliWonka.KDTree;
using DefaultNamespace;
using GameState;
using UnityEngine;

namespace Army
{
	public class ArmyKDTree
	{
		private List<UnitGroup> _unitGroups = new List<UnitGroup>();
		private List<Unit> _units;
		private KDTree _kdTree = new KDTree();
		private KDQuery _kdQuery = new KDQuery();
		private int _lastBuildTick = 0;
		public ArmyKDTree()
		{
			//TODO:build here
		}
		public void AddUnitGroup(UnitGroup unitGroup)
		{
			_unitGroups.Add(unitGroup);
		}
		public void RemoveUnitGroup(UnitGroup unitGroup)
		{
			_unitGroups.Remove(unitGroup);
		}
		private void UpdateUnitList()
		{
			var units = new List<Unit>();
			foreach(var unitGroup in _unitGroups)
			{
				units.AddRange(unitGroup.Units);
			}
			_units = units;
		}
		public List<Unit> GetNearestUnits(Vector2 position, int k)
		{
			RebuildIfNeeded();
			var result = new List<int>();
			var nearestUnits = new List<Unit>();
			_kdQuery.KNearest(_kdTree, position, k, result);
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
		public List<Unit> GetUnitsInRadius(Vector2 position, float radius)
		{
			RebuildIfNeeded();
			var result = new List<int>();
			var nearestUnits = new List<Unit>();

			_kdQuery.Radius(_kdTree, position, radius, result);
			foreach(var index in result)
			{
				nearestUnits.Add(_units[index]);
			}
			return nearestUnits;
		}
		public void ForceRebuild()
		{
			Build(_units.GetUnitPositions());
		}
		private void RebuildIfNeeded()
		{
			if(Ticker.CurrentTick <= _lastBuildTick) return;
			UpdateUnitList();
			Build(_units.GetUnitPositions());
		}

		private void Build(Vector2[] positions)
		{
			_lastBuildTick = Ticker.CurrentTick;
			_kdTree.Build(positions);
		}
	}
}