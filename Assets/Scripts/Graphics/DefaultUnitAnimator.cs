using Enums;
using GameState;
using UnityEngine;

namespace Graphics
{
	public class DefaultUnitAnimator : IUnitAnimator,ITickable
	{
		private UnitAnimationCollection _animationCollection;
		private UnitAnimationType _currentAnimationType = UnitAnimationType.Null;
		private UnitAnimation _currentAnimation;
		private int _currentFrame;
		private bool _isLoop;
		private float _timeForFrame;
		private float _timeForCurrentFrame;
		private bool _animationFinished = true;
		public Color ColorAdd{get; set;}

		public int CurrentFrameIndex{get; private set;}
		public DefaultUnitAnimator(UnitAnimationCollection animationCollection)
		{
			_animationCollection = animationCollection;
			Ticker.AddTickable(this);
		}
		~DefaultUnitAnimator()
		{
			Ticker.RemoveTickable(this);
		}

		public void OnTick(Ticker.OnTickEventArgs obj)
		{
			if(_animationFinished)
				return;
			_timeForCurrentFrame += obj.DeltaTime;
			if(_timeForCurrentFrame > _timeForFrame)
			{
				_timeForCurrentFrame = 0;
				_currentFrame++;
				if(_currentFrame >= _currentAnimation.Frames.Length)
				{
					if(_isLoop)
						_currentFrame = 0;
					else
					{
						_animationFinished = true;
						_currentFrame = _currentAnimation.Frames.Length - 1;
					}
				}
				CurrentFrameIndex = _currentAnimation.Frames[_currentFrame];
			}
		}

		public void SetAnimation(UnitAnimationType animationType, bool forceInterrupt = false, bool loop = true, float time = 1f)
		{
			if(_currentAnimationType == animationType && !forceInterrupt)
				return;
			if(!_currentAnimation.Interruptable && !_animationFinished && !forceInterrupt)
				return;
			var animation = _animationCollection[animationType];
			_currentAnimationType = animationType;
			_currentFrame = 0;
			_isLoop = loop;
			_currentAnimation = animation;
			_timeForFrame = time / _currentAnimation.Frames.Length;
			_animationFinished = false;
		}
	}

}