using System;
using System.Collections.Generic;
using System.Linq;
using Datastructures.KDTree;
using DataStructures.ViliWonka.KDTree;
using DefaultNamespace;
using DefaultNamespace.Enums;
using Formations.Scripts;
using Graphics;
using Stats;
using Stats.Structures;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Army
{

	[RequireComponent(typeof(IArmyInstancer))]
	[RequireComponent(typeof(IArmyMover))]
	[RequireComponent(typeof(IInputHandler))]
	public class Army : MonoBehaviour
	{
		private IArmyInstancer _armyInstancer;
		private IArmyMover _armyMover;
		private IInputHandler _inputHandler;
		private FormationBase _formation;
		private static ArmyKDTree _armyKDTree;

		[SerializeField] private SerializableArmyStats _serializableArmyStats;

		[SerializeField] private int _initialUnits = 6000;

		//perfect density is 15 per square unit
		[SerializeField] private float _unitDensity = 15f;
		public int Team;

		private readonly List<Unit> _units = new List<Unit>();
		public List<Unit> Units => _units;
		public StatDict<ArmyStat> Stats = new StatDict<ArmyStat>();



		private void Awake()
		{
			_armyInstancer = GetComponent<IArmyInstancer>();
			_armyMover = GetComponent<IArmyMover>();
			_inputHandler = GetComponent<IInputHandler>();

			float armyWidth = 16f / 9f;
			float armyHeight = 9f / 16f;
			_formation = new BoxFormation(armyWidth, armyHeight, false, 0.5f, _unitDensity);

			_armyKDTree = new ArmyKDTree(_units);
			//Teams.AddArmyToTeam(this);
			Stats = _serializableArmyStats.GetStats();
		}

		private void Start()
		{
			InitialiseArmy();
			var armyMoverArgs = new ArmyMoverArgs(_units, Stats, _armyKDTree, Helpers.CalculateArmyRange(_initialUnits, _unitDensity), _formation);
			_armyMover.Init(armyMoverArgs);
			_inputHandler.OnMove += _armyMover.SetTarget;
			_inputHandler.OnClick += _armyMover.MoveInRadius;
			_armyInstancer.CreateInstances(_units);
		}


		private void InitialiseArmy()
		{
			foreach(var pos in _formation.EvaluatePoints(_initialUnits))
			{
				if(_units.Count >= _initialUnits)
				{
					break;
				}
				AddUnit(new Unit(100, pos, _armyKDTree, Stats));
			}
		}
		private void AddUnit(Unit unit)
		{
			_units.Add(unit);
			unit.OnUnitKilled += RemoveUnit;

			void RemoveUnit()
			{
				_units.Remove(unit);
				unit.OnUnitKilled -= RemoveUnit;
			}
		}
		//TODO: change all updates to ontick
		private void Update()
		{
			_armyInstancer.UpdatePositions(_units);
			var positions = _units.Select(unit => unit.Position).ToList();
			//TODO: build inside kd tree, update it instead
			_armyKDTree.Build(positions);
		}

	}
}