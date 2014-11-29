using UnityEngine;
using System.Collections;

namespace SRF.Internal
{

	internal static class ComponentMenuPaths
	{

		public const string PathRoot = "SRF";


		#region Behaviours

		public const string BehavioursRoot = PathRoot + "/Behaviours";

		public const string DestroyOnDisable = BehavioursRoot + "/Destroy On Disable";
		public const string DontDestroyOnLoad = BehavioursRoot + "/Don't Destroy On Load";
		public const string MatchTransform = BehavioursRoot + "/Match Transform";
		public const string LookAt = BehavioursRoot + "/LookAt";
		public const string MatchForwardDirection = BehavioursRoot + "/Match Forward Direction";

		public const string RuntimePosition = BehavioursRoot + "/Runtime Position";
		public const string ScrollTexture = BehavioursRoot + "/Scroll Texture";
		public const string SmoothFloatBehaviour = BehavioursRoot + "/Smooth Float";
		public const string SmoothFollow2D = BehavioursRoot + "/Smooth Follow (2D)";
		public const string SmoothMatchTransform = BehavioursRoot + "/Match Transform (Smooth)";
		public const string SpawnPrefab = BehavioursRoot + "/Spawn Prefab";
		public const string Velocity = BehavioursRoot + "/Velocity";

		#endregion

	}

}