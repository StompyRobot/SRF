using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SRRadar<T> : SRMonoBehaviour where T : SRMonoBehaviour
{

	/// <summary>
	/// Ship detected by radar
	/// </summary>
	public interface IBandit
	{

		float Distance { get; }
		T Unit { get; }

	}

	class Bandit : IBandit
	{

		public float Distance { get; set; }

		public T Unit { get; set; }

	}

	private static readonly Dictionary<int, T> Cache = new Dictionary<int, T>();

	[Range(0.01f, 2)]
	public float UpdateFrequency = 0.5f;

	private float _nextUpdate;

	public IList<IBandit> NearUnits
	{
		get { return _readOnlyNearUnits ?? (_readOnlyNearUnits = _nearUnits.AsReadOnly()); }
	}

	public float Range = 10;

	public LayerMask Mask;

	public bool SearchParents = false;

	private readonly SRList<IBandit> _nearUnits = new SRList<IBandit>(16);
	private IList<IBandit> _readOnlyNearUnits;

	void Update()
	{

		if (RealTime.time > _nextUpdate) {

			PerformScan();
			_nextUpdate = RealTime.time + UpdateFrequency;

		} else {

			// Check for null entries (ships destroyed)
			for (int i = _nearUnits.Count - 1; i >= 0; --i) {

				if (_nearUnits[i].Unit == null) {
					_nearUnits.RemoveAt(i);
				}

			}

		}

	}

	bool HasFound(T t)
	{
		for (int i = 0; i < _nearUnits.Count; i++) {
			if (_nearUnits[i].Unit == t)
				return true;
		}
		return false;
	}

	void PerformScan()
	{

		var nearby = Physics.OverlapSphere(CachedTransform.position, Range, Mask);

		_nearUnits.Clear();

		if (nearby.Length == 0) {
			return;
		}

		for (int i = 0; i < nearby.Length; i++) {

			var n = nearby[i];
			var go = n.gameObject;

			if (go == CachedGameObject)
				continue;

			T unit;

			var id = n.GetInstanceID();

			if (!Cache.TryGetValue(id, out unit)) {
				unit = SearchParents ? NGUITools.FindInParents<T>(go) : n.GetComponent<T>();
				Cache.Add(id, unit);
			}

			if (unit == null) {
				Debug.LogWarning(
					"SRRadar: Object on radar layer missing target type. Layer: {0} Target: {1}".Fmt(LayerMask.LayerToName(Mask),
						typeof (T)), go);
				continue;
			}

			_nearUnits.Add(new Bandit() {
				Distance = (unit.CachedTransform.position - CachedTransform.position).magnitude,
				Unit = unit
			});

		}

		_nearUnits.Sort((p, q) => p.Distance.CompareTo(q.Distance));

	}

	static void CleanCache()
	{

		Cache.Clear();

	}

}
