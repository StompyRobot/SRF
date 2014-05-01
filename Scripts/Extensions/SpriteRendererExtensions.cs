using Holoville.HOTween;
using UnityEngine;
using System.Collections;

public static class SpriteRendererExtensions
{

	public static void TweenAlpha(this SpriteRenderer @this, float targetAlpha, float duration, bool realTime = false)
	{

		if (Mathf.Approximately(duration, 0)) {

			@this.color = @this.color.Alpha(targetAlpha);
			return;

		}

		var p = new TweenParms();

		if (realTime)
			p.UpdateType(UpdateType.TimeScaleIndependentUpdate);

		p.Prop("color", @this.color.Alpha(targetAlpha));

		HOTween.To(@this, duration, p);

	}

}
