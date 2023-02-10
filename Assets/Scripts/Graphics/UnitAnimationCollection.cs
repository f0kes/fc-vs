using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Graphics
{
	
	[System.Serializable]
	//Create asset menu
	[CreateAssetMenu(fileName = "UnitAnimationCollection", menuName = "UnitAnimationCollection", order = 0)]
	public class UnitAnimationCollection : ScriptableObject
	{
		public UnitAnimation[] Animations;
		private Dictionary<UnitAnimationType, UnitAnimation> _animations = new Dictionary<UnitAnimationType, UnitAnimation>();
		private bool _isInitialized;
		//indexer
		public UnitAnimation this[UnitAnimationType type]
		{
			get
			{
				if (!_isInitialized)
				{
					Initialize();
				}
				return _animations[type];
			}
		}

		public void Initialize()
		{
			_animations = new Dictionary<UnitAnimationType, UnitAnimation>();
			foreach (var animation in Animations)
			{
				_animations.Add(animation.Type, animation);
			}
			_isInitialized = true;
		}
	}
}