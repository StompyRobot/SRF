using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.SRF.Scripts.Helpers
{
	public static class SRReflection
	{

		/// <summary>
		/// Return list of all classes which subclass T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IList<Type> GetImplementors<T>() where T : class
		{

			var t = typeof (T);
			var results = new List<Type>();

			var types = Assembly.GetExecutingAssembly().GetTypes();

			foreach (var type in types) {

				if (!type.IsAbstract && t.IsAssignableFrom(type)) {
					results.Add(type);
				}

			}

			return results;

		}

	}
}
