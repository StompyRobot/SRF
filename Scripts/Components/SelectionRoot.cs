using UnityEngine;

namespace SRF.Components
{

	/// <summary>
	/// Adding this component to a GameObject will make it behave like a prefab when clicked on in the scene view 
	/// (clicking on child elements will select this element instead)
	/// </summary>
	[SelectionBase]
	[AddComponentMenu(Internal.ComponentMenuPaths.SelectionRoot)]
	public class SelectionRoot : MonoBehaviour
	{

	}

}
