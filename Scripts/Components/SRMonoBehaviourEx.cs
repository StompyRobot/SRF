using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class RequiredFieldAttribute : Attribute
{

	private bool _editorOnly = true;
	private bool _autoSearch;
	private bool _autoCreate;

	public bool AutoSearch
	{
		get { return _autoSearch; }
		set { _autoSearch = value; }
	}

	public bool AutoCreate
	{
		get { return _autoCreate; }
		set { _autoCreate = value; }
	}

	public bool EditorOnly
	{
		get { return _editorOnly; }
		set { _editorOnly = value; }
	}

	public RequiredFieldAttribute(bool autoSearch)
	{
		AutoSearch = autoSearch;
	}

	public RequiredFieldAttribute()
	{
	}

}

public abstract class SRMonoBehaviourEx : SRMonoBehaviour
{

	private struct FieldInfo
	{

		public System.Reflection.FieldInfo Field;

		public bool AutoSet;
		public bool AutoCreate;

	}

	private static Dictionary<Type, IList<FieldInfo>> _checkedFields;

	private static void CheckFields(SRMonoBehaviourEx instance, bool justSet = false)
	{

		if (_checkedFields == null)
			_checkedFields = new Dictionary<Type, IList<FieldInfo>>();

		var t = instance.GetType();

		IList<FieldInfo> cache;

		if (!_checkedFields.TryGetValue(instance.GetType(), out cache)) {

			cache = ScanType(t);

			_checkedFields.Add(t, cache);

		}

		PopulateObject(cache, instance, justSet);

	}

	private static void PopulateObject(IList<FieldInfo> cache, SRMonoBehaviourEx instance, bool justSet)
	{

		for (var i = 0; i < cache.Count; i++) {

			var f = cache[i];

			if (!EqualityComparer<System.Object>.Default.Equals(f.Field.GetValue(instance), null))
				continue;

			// If autoset is enabled on field, try and find the component on the GameObject
			if (f.AutoSet) {

				var newValue = instance.GetComponent(f.Field.FieldType);

				if (!EqualityComparer<System.Object>.Default.Equals(newValue, null)) {
					f.Field.SetValue(instance, newValue);
					continue;
				}

			}

			if (justSet)
				continue;

			if (f.AutoCreate) {

				var newValue = instance.CachedGameObject.AddComponent(f.Field.FieldType);
				f.Field.SetValue(instance, newValue);

			}

			throw new UnassignedReferenceException(
				"Field {0} is unassigned, but marked with RequiredFieldAttribute".Fmt(f.Field.Name));

		}

	}

	private static List<FieldInfo> ScanType(Type t)
	{

		var cache = new List<FieldInfo>();

		// Check for attribute added to the class
		var globalAttr =
			t.GetCustomAttributes(typeof (RequiredFieldAttribute), true).FirstOrDefault() as RequiredFieldAttribute;

		// Check each field for the attribute
		var fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance);

		for (var i = 0; i < fields.Length; i++) {

			var f = fields[i];

			var c = f.GetCustomAttributes(typeof (RequiredFieldAttribute), false).FirstOrDefault() as RequiredFieldAttribute;

			if (globalAttr != null || c != null) {

#if !UNITY_EDITOR

					if((c == null && globalAttr.EditorOnly && !globalAttr.AutoSearch) || (c != null && c.EditorOnly && !c.AutoSearch))
						continue;

#endif

				var info = new FieldInfo();
				info.Field = f;

				// Set from field attribute if it exists, falling back on the class attr
				info.AutoSet = (c == null) ? globalAttr.AutoSearch : c.AutoSearch;

				if (c != null)
					info.AutoCreate = c.AutoCreate;

				cache.Add(info);

			}

		}

		return cache;

	} 

	protected virtual void Awake()
	{

		CheckFields(this);

	}

	protected virtual void Start()
	{
		
	}

	protected virtual void Update()
	{

	}

	protected virtual void FixedUpdate()
	{
		
	}

	protected virtual void OnEnable()
	{
		
	}

	protected virtual void OnDisable()
	{
		
	}

}
