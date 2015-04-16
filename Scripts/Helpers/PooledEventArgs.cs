
using System.Collections.Generic;

namespace SRF
{

	public abstract class PooledEventArgs<T> where T : PooledEventArgs<T>, new()
	{

		private static readonly List<T> Pool = new List<T>();

		/// <summary>
		/// Override in child classes to reset event args to default values
		/// </summary>
		protected abstract void Reset();

		
		public static T Borrow()
		{

			if (Pool.Count > 0)
				return Pool.PopLast();

			return new T();

		}

		public static void Release(T t)
		{

			t.Reset();
			Pool.Add(t);

		}

	}

}