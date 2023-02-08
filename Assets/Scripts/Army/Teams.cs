using System.Collections.Generic;
using System.Linq;
using Datastructures.KDTree;
using UnityEngine;

namespace Army
{
	public static class Teams
	{
		public static KDTree[] TeamTrees;
		private static List<Army>[] _armies; 
		// init the tree for team 
		public static void InitTeamTrees(int teamcount)
		{
			TeamTrees = new KDTree[teamcount];
			_armies = new List<Army>[teamcount];
			for (int i = 0; i < teamcount; i++)
			{
				TeamTrees[i] = new KDTree();
				_armies[i] = new List<Army>();
			}
		}
		
		public static void AddArmyToTeam(Army army)
		{
			_armies[army.Team].Add(army);
		}
		//remove army from team
		public static void RemoveArmyFromTeam(Army army)
		{
			_armies[army.Team].Remove(army);
		}
		// rebuild the tree for team
		public static void RebuildTeamTree(int team)
		{
			var positions = new List<Vector2>();
			foreach (Army army in _armies[team])
			{
				positions.AddRange(army.Units.Select(unit => unit.Position));
			}
			TeamTrees[team].Build(positions);
		}
	}
}