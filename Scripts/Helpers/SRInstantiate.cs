using UnityEngine;


public static class SRInstantiate
{

	public static T Instantiate<T>(T prefab) where T : Component
	{
		return (T)Object.Instantiate(prefab);
	}
	
	public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return (T)Object.Instantiate(prefab, position, rotation);
	}

}