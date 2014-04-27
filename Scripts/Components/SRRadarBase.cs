using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public abstract class SRRadarBase<T> : SRMonoBehaviour where T : SRMonoBehaviour
{

	private const float CacheCheckFrequency = 60f;
	private static float NextCacheCheck = CacheCheckFrequency;

	/// <summary>
	/// Ship detected by radar
	/// </summary>
	public interface IBandit
	{

		float Distance { get; }
		T Unit { get; }

	}

	protected class Bandit : IBandit
	{

		public float Distance { get; set; }

		public T Unit { get; set; }

	}

	private static readonly SRList<Bandit> BanditCache = new SRList<Bandit>(); 

	private static readonly Dictionary<int, T> Cache = new Dictionary<int, T>();

	/// <summary>
	/// Set to true, will update on FixedUpdate instead of by update frequency
	/// </summary>
	public bool UseFixedUpdate = false;

	[Range(0.01f, 2)]
	public float UpdateFrequency = 0.5f;

	private float _nextUpdate;

	public IList<IBandit> NearUnits
	{
		get { return _nearUnits.AsReadOnly(); }
	}

	public float Range = 10;

	public LayerMask Mask;

	public bool SearchParents = false;

	private readonly SRList<IBandit> _nearUnits = new SRList<IBandit>(16);

	void Update()
	{

		if (!UseFixedUpdate && RealTime.time > _nextUpdate) {

			InternalPerformScan();
			_nextUpdate = RealTime.time + UpdateFrequency;

		} else {

			// Check for null entries (ships destroyed)
			for (int i = _nearUnits.Count - 1; i >= 0; --i) {

				if (_nearUnits[i].Unit == null) {

					var b = _nearUnits[i];
					_nearUnits.RemoveAt(i);
					RecycleBandit((Bandit) b);

				}

			}

		}

	}

	void FixedUpdate()
	{

		if (!UseFixedUpdate)
			return;

		InternalPerformScan();

	}

	protected void HandleDiscoveredObject(GameObject go)
	{

		if (go == CachedGameObject)
			return;

		T unit;

		var id = go.GetInstanceID();

		if (!Cache.TryGetValue(id, out unit)) {
			unit = SearchParents ? NGUITools.FindInParents<T>(go) : go.GetComponent<T>();
			Cache.Add(id, unit);
		}

		if (unit == null) {

			Debug.LogWarning(
				"SRRadar: Object on radar layer missing target type. Layer: {0} Target: {1}".Fmt(LayerMask.LayerToName(Mask),
					typeof (T)), go);

			return;

		}

		if (HasFound(unit))
			return;

		var b = GetBandit();
		b.Unit = unit;
		b.Distance = (unit.CachedTransform.position - CachedTransform.position).magnitude;

		_nearUnits.Add(b);

	}

	protected bool HasFound(T t)
	{

		for (int i = 0; i < _nearUnits.Count; i++) {
			if (_nearUnits[i].Unit == t)
				return true;
		}

		return false;

	}

	private void InternalPerformScan()
	{

		while (_nearUnits.Count > 0) {
			RecycleBandit((Bandit)_nearUnits.PopLast());
		}

		PerformScan();

		_nearUnits.Sort((p, q) => p.Distance.CompareTo(q.Distance));

		if (RealTime.time > NextCacheCheck) {
			CleanCache();
		}

	}

	protected abstract void PerformScan();

	static void CleanCache()
	{

		// TODO: Iterate Dictionary and remove null elements instead of just clearing
		Cache.Clear();
		NextCacheCheck = RealTime.time + CacheCheckFrequency;

	}

	protected static Bandit GetBandit()
	{
		
		if(BanditCache.Count == 0)
			return new Bandit();

		return BanditCache.PopLast();

	}

	protected static void RecycleBandit(Bandit bandit)
	{
		
		BanditCache.Add(bandit);

	}

}
