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

		#region Components

		public const string ComponentsRoot = PathRoot + "/Components";

		public const string SRLineRenderer = ComponentsRoot + "/SRLineRenderer";
		public const string SelectionRoot = ComponentsRoot + "/Selection Root";

		public const string SRSpriteFadeRenderer = ComponentsRoot + "/Fade Renderer (Sprite)";
		public const string SRMaterialFadeRenderer = ComponentsRoot + "/Fade Renderer (Material)";
		public const string SRCompositeFadeRenderer = ComponentsRoot + "/Fade Renderer (Composite)";

		#endregion

		public const string SRServiceManager = PathRoot + "/Service/Service Manager";

		#region UI

		public const string UIRoot = PathRoot + "/UI";

		public const string TiltOnTouch = ComponentsRoot + "/Tilt On Touch";
		public const string ScaleOnTouch = ComponentsRoot + "/Scale On Touch";
		public const string InheritColour = ComponentsRoot + "/Inherit Colour";
		public const string FlashGraphic = ComponentsRoot + "/Flash Graphic";
		public const string CopyPreferredSize = ComponentsRoot + "/Copy Preferred Size";
		public const string SRText = ComponentsRoot + "/SRText";

		#endregion

	}

}