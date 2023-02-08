using System.Collections.Generic;
using Stats.Structures;

namespace Stats
{
	public class StatDict<T>
	{
		private Dictionary<T, Stat> _stats = new Dictionary<T, Stat>();
		public float this[T name, bool forceUpdate=false] => _stats[name].GetValue(forceUpdate);

		public Stat GetStat(T name)
		{
			return _stats[name];
		}

		public void SetStat(T name, Stat stat)
		{
			_stats[name] = stat;
		}
	}
}