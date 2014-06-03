using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System.Collections;

namespace SRF.Collections
{

	/// <summary>
	/// Implementation of IDictionary which supports unity serialization
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	[Serializable]
	public class SRDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
	{

		[SerializeField]
		private SRList<TKey> _keys;

		[SerializeField]
		private SRList<TValue> _values;

		private readonly Dictionary<TKey, TValue> _internalDict = new Dictionary<TKey, TValue>();

		public void OnBeforeSerialize()
		{

			if (_keys == null)
				_keys = new SRList<TKey>(_internalDict.Count);

			if (_values == null)
				_values = new SRList<TValue>(_internalDict.Count);

			_keys.Clear(true);
			_values.Clear(true);

			_keys.AddRange(_internalDict.Keys);
			_values.AddRange(_internalDict.Values);

		}

		public void OnAfterDeserialize()
		{
			
			_internalDict.Clear();

			if (_keys == null && _values == null)
				return;

			if (_keys == null)
				throw new SerializationException("Keys list is null, but values list is not");
			
			if (_values == null)
				throw new SerializationException("Values list is null, but keys list is not");

			if (_keys.Count != _values.Count)
				throw new SerializationException("Key/Value list length mismatch");

			for (var i = 0; i < _keys.Count; i++) {

				_internalDict.Add(_keys[i], _values[i]);

			}

			_keys.Clear(true);
			_values.Clear(true);

		}

		public ICollection<TKey> Keys
		{
			get { return _internalDict.Keys; }
		}

		public ICollection<TValue> Values
		{
			get { return _internalDict.Values; }
		}

		public int Count
		{
			get { return _internalDict.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _internalDict.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			((IDictionary<TKey, TValue>) _internalDict).Add(item);
		}

		public void Clear()
		{
			_internalDict.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((IDictionary<TKey, TValue>)_internalDict).Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((IDictionary<TKey, TValue>) _internalDict).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return ((IDictionary<TKey, TValue>)_internalDict).Remove(item);
		}

		public void Add(TKey key, TValue value)
		{
			_internalDict.Add(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return _internalDict.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			return _internalDict.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _internalDict.TryGetValue(key, out value);
		}

		public TValue this[TKey key]
		{
			get { return _internalDict[key]; }
			set { _internalDict[key] = value; }
		}


	}

}