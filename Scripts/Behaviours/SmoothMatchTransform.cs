using UnityEngine;
using System.Collections;

public class SmoothMatchTransform : SRMonoBehaviour
{

	public Transform Target;

	public bool MatchPosition = true;
	public bool MatchRotation = true;
	public bool MatchScale = true;

	public float SmoothStrength = 15f;

	public bool IgnoreTimeScale = false;

	void Update()
	{

		if (Target == null)
			return;

		var dt = IgnoreTimeScale ? RealTime.deltaTime : Time.deltaTime;

		if(MatchPosition)
			CachedTransform.position = NGUIMath.SpringLerp(CachedTransform.position, Target.position, SmoothStrength, dt);

		if(MatchRotation)
			CachedTransform.rotation = NGUIMath.SpringLerp(CachedTransform.rotation, Target.rotation, SmoothStrength, dt);

		if(MatchScale)
			CachedTransform.localScale = NGUIMath.SpringLerp(CachedTransform.localScale, Target.localScale, SmoothStrength, dt);

	}

}
