using UnityEngine;
using System.Collections;

public class DestroyOnDisable : MonoBehaviour
{

	void OnDisable()
	{
		Destroy(this.gameObject);
	}
	
}
