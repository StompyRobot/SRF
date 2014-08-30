#if ENABLE_4_6_FEATURES

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{
	public class FlashGraphic : UIBehaviour, IPointerDownHandler, IPointerUpHandler
	{

		public Graphic Target;

		public Color DefaultColor = new Color(1,1,1,0);
		public Color FlashColor = Color.white;

		public float DecayTime = 0.15f;

		protected override void OnEnable()
		{
			base.OnEnable();
			Target.CrossFadeColor(DefaultColor, 0f, true, true);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Target.CrossFadeColor(FlashColor, 0f, true, true);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Target.CrossFadeColor(DefaultColor, DecayTime, true, true);
		}

	}
}

#endif