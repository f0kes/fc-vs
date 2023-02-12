using System.Collections.Generic;
using Stats.Structures;

namespace Stats
{
	public class StatDict<T> where T : System.Enum
	{
		private Stat[] _stats;
		public float this[T name, bool forceUpdate=false] => _stats[GetIndex(name)].GetValue(forceUpdate);
		public StatDict()
		{
			//cast to int to get the number of enum values
			_stats = new Stat[System.Enum.GetValues(typeof(T)).Length];
			foreach(T name in System.Enum.GetValues(typeof(T)))
			{
				_stats[(int)(object)name] = new Stat();
			}
		}
		private int GetIndex(T name)
		{
			return (int)(object)name;
		}
		public Stat GetStat(T name)
		{
			return _stats[GetIndex(name)];
		}

		public void SetStat(T name, Stat stat)
		{
			_stats[GetIndex(name)] = stat;
		}
	}
}