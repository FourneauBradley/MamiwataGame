using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class SizeHandler : MonoBehaviour
    {
        public float targetAspect = 16f / 9f;
        public float baseOrthoSize = 7.2f;
        private Camera cam;

        void Start()
        {
            cam = GetComponent<Camera>();
            ApplySafeAspect();
        }

        void Update()
        {
            ApplySafeAspect();
        }

        void OnEnable()
        {
            cam = GetComponent<Camera>();
            ApplySafeAspect();
        }

        void ApplySafeAspect()
        {
            if (cam == null) return;

            float screenAspect = (float)Screen.width / Screen.height;
            float scaleFactor = targetAspect / screenAspect;

            cam.orthographicSize = baseOrthoSize * Mathf.Max(1f, scaleFactor);
        }


    }
}