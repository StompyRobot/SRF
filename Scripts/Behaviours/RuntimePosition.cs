using UnityEngine;
using System.Collections;

namespace SRF.Behaviours
{

	/// <summary>
	/// Set localPosition as soon as this object is created. (Useful for setting up UI workspaces in editor)
	/// </summary>
	[AddComponentMenu(Internal.ComponentMenuPaths.RuntimePosition)]
	public class RuntimePosition : MonoBehaviour
	{

		public Vector3 RunPosition;

		public bool UseLocal = true;

		private void Awake()
		{

			if (UseLocal)
				transform.localPosition = RunPosition;
			else
				transform.position = RunPosition;

			Destroy(this);

		}

	}

}