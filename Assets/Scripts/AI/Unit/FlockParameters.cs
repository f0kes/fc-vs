using System;
using UnityEngine;

namespace DefaultNamespace.AI.Unit
{
	[Serializable]
	public class FlockParameters
	{
		[Range(1f, 100f)]
		public float driveFactor;
		
		[Range(1f, 10f)]
		public float neighborRadius;
		[Range(0f, 1f)]
		public float avoidanceRadiusMultiplier;

		public float SquareMaxSpeed{get; private set;}
		public float SquareNeighborRadius{get; private set;}
		public float SquareAvoidanceRadius{get; private set;}
		
		
		public FlockParameters()
		{
			
			SquareNeighborRadius = neighborRadius * neighborRadius;
			SquareAvoidanceRadius = SquareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
		}

	}
}