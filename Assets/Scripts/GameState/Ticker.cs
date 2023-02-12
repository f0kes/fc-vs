using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameState
{
	public static class Ticker
	{
		
		 
		public class OnTickEventArgs : EventArgs
		{
			public int Tick;
			public bool Simulating;
			public float DeltaTime;
		}

		public class OnUpdateEventArgs : EventArgs
		{
			public float DeltaTime;
		}

		private static float _tickRate = 240f;
		public static float TickInterval => 1f / _tickRate;
		private static float _currentTickTime = 0f;
		private static float _timeSinceLastTick = 0f;

		private static int _currentTick;

		private static bool _isPaused = false;
		private static  bool TickOnUpdate = true;
		
		private static List<ITickable> _tickables = new List<ITickable>();
		private static List<ITickable> _tickablesToAdd = new List<ITickable>();
		private static List<ITickable> _tickablesToRemove = new List<ITickable>();


		public static int CurrentTick
		{
			get => _currentTick;
			set => _currentTick = value;
		}
		

		public static async void InvokeInTime(Action toInvoke, float time)
		{
			float timePassed = 0;
			float timeStart = Time.time;
			while (timePassed < time)
			{
				timePassed += timeStart - Time.time;
				await Task.Yield();
			}

			toInvoke.Invoke();
		}


		public static void Update()
		{
			if(_isPaused) return;
			_currentTickTime += Time.deltaTime;
			_timeSinceLastTick += Time.deltaTime;
			if(TickOnUpdate)
			{
				Tick();
				_timeSinceLastTick = 0;
			}
			else
			{
				while (_currentTickTime >= TickInterval)
				{
					_currentTickTime -= TickInterval;
					Tick();
					_timeSinceLastTick = 0;
				}
			}
		}

		public static void Pause()
		{
			_isPaused = true;
		}

		public static void Unpause()
		{
			_isPaused = false;
		}

		public static void Tick(bool simulating = false)
		{
			_currentTick++;
			var args = new OnTickEventArgs { Tick = _currentTick, Simulating = simulating, DeltaTime = _timeSinceLastTick };
			_tickables.AddRange(_tickablesToAdd);
			_tickablesToRemove.ForEach(tickable => _tickables.Remove(tickable));
			_tickablesToAdd.Clear();
			_tickablesToRemove.Clear();
			foreach(var tickable in _tickables)
			{
				tickable.OnTickStart(args);
				tickable.OnTick(args);
				tickable.OnTickEnd(args);
			}
		}
		public static void AddTickable(ITickable tickable)
		{
			_tickablesToAdd.Add(tickable);
		}
		public static void RemoveTickable(ITickable tickable)
		{
			_tickablesToRemove.Add(tickable);
		}

		public static float TicksToSeconds(int ticks)
		{
			return ticks * TickInterval;
		}

		public static int SecondsToTicks(float seconds)
		{
			return (int)(seconds / TickInterval);
		}
	}
}