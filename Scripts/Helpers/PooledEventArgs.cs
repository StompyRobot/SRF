using Smooth.Pools;
using UnityEngine;
using System.Collections;

namespace SRF.Helpers
{

	public abstract class PooledEventArgs<T> where T : PooledEventArgs<T>, new()
	{

		protected PooledEventArgs()
		{
			
		}

		/// <summary>
		/// Override in child classes to reset event args to default values
		/// </summary>
		protected abstract void Reset();

		/// <summary>
		/// Singleton List<T> pool.
		/// </summary>
		private static class InternalPool
		{

			private static readonly Pool<T> _Instance = new Pool<T>(
				() => new T(),
				t => t.Reset());

			/// <summary>
			/// Singleton List<T> pool instance.
			/// </summary>
			public static Pool<T> Instance
			{
				get { return _Instance; }
			}

		}

		public static T Borrow()
		{
			return InternalPool.Instance.Borrow();
		}

		public static void Release(T t)
		{
			InternalPool.Instance.Release(t);
		}

	}

}