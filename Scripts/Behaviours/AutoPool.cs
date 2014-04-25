using UnityEngine;
using System.Collections;

public class AutoPool : SRMonoBehaviour
{

	public float Delay = 1f;

	private float _triggerTime = 0;

	void OnEnable()
	{
		_triggerTime = Time.time + Delay;
	}

	void Update()
	{

		if (Time.time >= _triggerTime) {
			ObjectPoolController.Destroy(CachedGameObject);
		}

	}
	
}
