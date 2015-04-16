using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours.Camera
{

	[AddComponentMenu(ComponentMenuPaths.SRDebugCamera)]
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class SRDebugCamera : SRMonoBehaviourEx
	{

		[RequiredField(true)]
		public UnityEngine.Camera Camera;

		protected override void FixedUpdate()
		{

			base.FixedUpdate();

			SRDebugUtil.FlushFixedUpdateLines();
			SRDebugUtil.IsFixedUpdate = true;

		}

		protected override void Update()
		{

			base.Update();

			SRDebugUtil.FlushLines();
			SRDebugUtil.IsFixedUpdate = false;

		}


		private void OnPostRender()
		{
			SRDebugUtil.DrawDebugFrame(Camera);
		}

	}

}