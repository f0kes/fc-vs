using System.Collections.Generic;
using System.Linq;
using GameState;
using UnityEngine;

namespace Stats
{
	namespace Structures
	{
		[System.Serializable]
		public class Stat
		{
			[SerializeField] private float baseValue;

			public float BaseValue => baseValue;

			private float _lastValue;
			private int _lastUpdateTick = -1;
			private List<StatModifier> _modifiers;

			public List<StatModifier> Modifiers => _modifiers;

			public Stat()
			{
				_modifiers = new List<StatModifier>();
			}

			public Stat(float baseValue)
			{
				_modifiers = new List<StatModifier>();
				this.baseValue = baseValue;
			}

			public float GetValue(bool forceUpdate)
			{
				if(!forceUpdate && _lastUpdateTick >= Ticker.CurrentTick) return _lastValue;
				var result = baseValue;
				foreach(var mod in _modifiers)
				{
					mod.ApplyMod(ref result, baseValue);
				}
				_lastUpdateTick = Ticker.CurrentTick;
				_lastValue = result;
				return _lastValue;
			}

			public void SetBaseValue(float val)
			{
				baseValue = val;
			}

			public float GetLastValue()
			{
				return _lastValue;
			}

			
			
			public void AddMod(StatModifier mod)
			{
				_modifiers.Add(mod);
				_modifiers = _modifiers.OrderBy(m => m.priority).ToList();
			}
			
			public void RemoveMod(StatModifier mod)
			{
				if(!_modifiers.Contains(mod)) return;
				_modifiers.Remove(mod);
				_modifiers = _modifiers.OrderBy(m => m.priority).ToList();
			}
		}
	}
}