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
		public static readonly ArmyKDTree KDTree = new ArmyKDTree();

		private IArmyInstancer _armyInstancer;
		private IArmyMover _armyMover;
		private IInputHandler _inputHandler;
		private IInputHandler _defaultInputHandler;
		private FormationBase _formation;

		private ArmyAnimator _armyAnimator;

		[SerializeField] private SerializableArmyStats _serializableArmyStats;

		[SerializeField] private int _initialUnits = 6000;
		[SerializeField] private uint _team = 0;
		[SerializeField] private UnitAnimationCollection _unitAnimationCollection;

		//perfect density is 15 per square unit
		[SerializeField] private float _unitDensity = 15f;

		private UnitGroup _units;
		public UnitGroup Units => _units;
		public StatDict<ArmyStat> Stats = new StatDict<ArmyStat>();



		private void Awake()
		{
			Stats = _serializableArmyStats.GetStats();
			_units = new UnitGroup(this, _team);
			KDTree.AddUnitGroup(_units);

			_unitAnimationCollection.Initialize();
			_armyInstancer = GetComponent<IArmyInstancer>();
			_armyMover = GetComponent<IArmyMover>();
			_inputHandler = GetComponent<IInputHandler>();
			_defaultInputHandler = _inputHandler;


			_formation = new PointFormation();


			//Teams.AddArmyToTeam(this);
		}

		private void Start()
		{
			InitialiseArmy();
			var armyMoverArgs = new ArmyMoverArgs(_units, Stats, KDTree, Helpers.CalculateArmyRange(_initialUnits, _unitDensity), _formation)
			{
				Team = _team
			};
			_armyMover.Init(armyMoverArgs);
			SetInputHandler(_inputHandler);
			//_inputHandler.OnClick += _armyMover.MoveInRadius;
			_armyInstancer.CreateInstances(_units.Units);
		}

		public void SetInputHandler(IInputHandler inputHandler)
		{
			if(_inputHandler != null)
				_inputHandler.InputEvents.OnMove -= _armyMover.SetTarget;
			_inputHandler = inputHandler;
			_inputHandler.InputEvents.OnMove += _armyMover.SetTarget;
		}
		public void ResetInputHandler()
		{
			_inputHandler.InputEvents.OnMove -= _armyMover.SetTarget;
			_inputHandler = _defaultInputHandler;
		}


		private void InitialiseArmy()
		{
			for(int i = 0; i < _initialUnits; i++)
			{
				SpawnInRandomPosition();
			}
			_armyAnimator = new ArmyAnimator(_units);
		}
		private void SpawnInRandomPosition()
		{
			var position = transform.position;
			var pos = new Vector2(position.x, position.z);
			var randomPosition = Random.insideUnitCircle * 10f + pos;
			var animator = new DefaultUnitAnimator(_unitAnimationCollection);
			Units.AddUnit(new Unit(100, randomPosition, Stats, animator));
		}

		//TODO: change all updates to ontick
		private void Update()
		{
			_armyInstancer.UpdatePositions(_units.Units);
			var positions = _units.GetPositions().ToList();
		}

	}
}