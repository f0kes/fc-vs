using System;
using System.Collections.Generic;
using System.Linq;
using Army.Units;
using ArmyInput;
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
		private  ArmyKDTree _armyKDTree;

		[SerializeField] private SerializableArmyStats _serializableArmyStats;

		[SerializeField] private int _initialUnits = 6000;
		[SerializeField] private uint _team = 0;

		//perfect density is 15 per square unit
		[SerializeField] private float _unitDensity = 15f;
		public int Team;

		private readonly UnitGroup _units = new UnitGroup();
		public UnitGroup Units => _units;
		public StatDict<ArmyStat> Stats = new StatDict<ArmyStat>();



		private void Awake()
		{
			_armyInstancer = GetComponent<IArmyInstancer>();
			_armyMover = GetComponent<IArmyMover>();
			_inputHandler = GetComponent<IInputHandler>();

			float armyWidth = 16f / 9f;
			float armyHeight = 9f / 16f;
			_formation = new PointFormation();

			_armyKDTree = new ArmyKDTree(_units.Units);
			//Teams.AddArmyToTeam(this);
			Stats = _serializableArmyStats.GetStats();
		}

		private void Start()
		{
			InitialiseArmy();
			var armyMoverArgs = new ArmyMoverArgs(_units, Stats, _armyKDTree, Helpers.CalculateArmyRange(_initialUnits, _unitDensity), _formation)
			{
				Team = _team
			};
			_armyMover.Init(armyMoverArgs);
			_inputHandler.OnMove += _armyMover.SetTarget;
			_inputHandler.OnClick += _armyMover.MoveInRadius;
			_armyInstancer.CreateInstances(_units.Units);
		}


		private void InitialiseArmy()
		{
			for (int i = 0; i < _initialUnits; i++)
			{
				SpawnInRandomPosition();
			}
		}
		private void SpawnInRandomPosition()
		{
			var position = transform.position;
			var pos = new Vector2(position.x, position.z);
			var randomPosition = Random.insideUnitCircle * 10f + pos;
			Units.AddUnit(new Unit(100, randomPosition, _armyKDTree, Stats, _team));
		}
		
		//TODO: change all updates to ontick
		private void Update()
		{
			_armyInstancer.UpdatePositions(_units.Units);
			var positions = _units.GetPositions().ToList();
			_armyKDTree.Build(positions);
		}

	}
}