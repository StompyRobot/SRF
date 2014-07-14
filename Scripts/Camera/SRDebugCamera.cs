using UnityEngine;
using System.Collections;

namespace SRF.Camera
{

	[RequireComponent(typeof (UnityEngine.Camera))]
	public class SRDebugCamera : SRMonoBehaviourEx
	{

		[RequiredField(true)] public UnityEngine.Camera Camera;

		protected override void FixedUpdate()
		{

			base.FixedUpdate();

			SRDebug.FlushFixedUpdateLines();
			SRDebug.IsFixedUpdate = true;

		}

		protected override void Update()
		{

			base.Update();

			SRDebug.FlushLines();
			SRDebug.IsFixedUpdate = false;

		}


		private void OnPostRender()
		{
			SRDebug.DrawDebugFrame(Camera);
		}

	}

}