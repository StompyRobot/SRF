using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Hierarchy : SRAutoSingleton<Hierarchy>
{

	private static readonly char[] Seperator = new[] {'/'};

	private static readonly Dictionary<string, Transform> Cache = new Dictionary<string, Transform>();

	public static Transform Get(string key)
	{

		Transform t;

		// Check cache
		if (Cache.TryGetValue(key, out t))
			return t;

		var find = GameObject.Find(key);

		if (find) {

			t = find.transform;
			Cache.Add(key, t);

			return t;

		}

		// Find container parent
		var elements = key.Split(Seperator, StringSplitOptions.RemoveEmptyEntries);

		// Create new container
		t = new GameObject(elements.Last()).transform;
		Cache.Add(key, t);

		// If root
		if (elements.Length == 1)
			return t;

		t.parent = Get(string.Join("/", elements, 0, elements.Length - 1));

		return t;

	}

	[Obsolete("Use static Get() instead")]
	public Transform this[string key]
	{
		get { return Get(key); }
	}

}
