﻿#if ENABLE_4_6_FEATURES

using SRF.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{
    /// <summary>
    /// Detects when a screen dpi exceeds what the developer considers
    /// a "retina" level display, and scales the canvas accordingly.
    /// </summary>
    [RequireComponent(typeof (CanvasScaler))]
    [AddComponentMenu(ComponentMenuPaths.RetinaScaler)]
    public class SRRetinaScaler : SRMonoBehaviour
    {
        [SerializeField] private float _retinaScale = 2f;

        [SerializeField] private int _thresholdDpi = 250;

        /// <summary>
        /// Dpi over which to apply scaling
        /// </summary>
        public int ThresholdDpi
        {
            get { return _thresholdDpi; }
        }

        public float RetinaScale
        {
            get { return _retinaScale; }
        }

        private void Start()
        {
            var dpi = Screen.dpi;

            if (dpi <= 0)
            {
                return;
            }

            if (dpi > ThresholdDpi)
            {
                var scaler = GetComponent<CanvasScaler>();

                scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                scaler.scaleFactor = RetinaScale;
            }
        }
    }
}

#endif
