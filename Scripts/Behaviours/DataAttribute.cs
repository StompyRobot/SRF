using UnityEngine;

namespace Scripts.Framework.Behaviours
{

	/// <summary>
	/// Quick way of attaching a component as a data container
	/// </summary>
	public abstract class DataAttribute<T> : MonoBehaviour where T : DataAttribute<T>
	{

		public static T GetOrDefault(GameObject o)
		{
			return o.GetComponent<T>();
		}

		public static T GetOrCreate(GameObject o)
		{
			return o.GetComponentOrAdd<T>();
		}

	}

}
