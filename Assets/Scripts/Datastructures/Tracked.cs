using System;

namespace Datastructures
{

	public class Tracked<T>
	{
		public T Value{get; private set;}
		public event Action<T> OnValueChanged;
		public Tracked(T value)
		{
			Value = value;
		}
		public void Set(T value)
		{
			Value = value;
			OnValueChanged?.Invoke(value);
		}
	}
}