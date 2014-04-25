using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Hierarchy : SRAutoSingleton<Hierarchy>
{

//#if UNITY_EDITOR

	private readonly char[] _seperator = new[] {'/'};

	private readonly Dictionary<string, Transform> _cache = new Dictionary<string, Transform>();

	public Transform this[string key]
	{
		get
		{

			Transform t;

			// Check cache
			if (_cache.TryGetValue(key, out t))
				return t;

			var find = GameObject.Find(key);

			if (find) {

				t = find.transform;
				_cache.Add(key, t);

				return t;

			}

			// Find container parent
			var elements = key.Split(_seperator, StringSplitOptions.RemoveEmptyEntries);

			// Create new container
			t = new GameObject(elements.Last()).transform;
			_cache.Add(key, t);

			// If root
			if (elements.Length == 1) 
				return t;

			t.parent = this[string.Join("/", elements, 0, elements.Length - 1)];

			return t;

		}
	}

/*#else

	public Transform this[string key]
	{
		get { return null; }
	}

#endif*/

}
