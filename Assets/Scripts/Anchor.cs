using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class Anchor : MonoBehaviour
    {
        [SerializeField] AnchorPos anchorPos;
        [SerializeField] Vector3 anchorOffset;
        public float referenceOrthoSize = 7.2f;
        public Vector3 referenceScale = Vector3.one;
        public bool scaleX = false;
        public bool scaleY = false;
        public bool scaleZ = false;
        private Camera cam;

        void Start()
        {
            cam = Camera.main;
            referenceOrthoSize=cam.GetComponent<SizeHandler>().baseOrthoSize;
        }

        void Update()
        {
            if (cam == null) return;
            float currentWidth = 2f * cam.orthographicSize * cam.aspect;
            float currentHeight = 2f * cam.orthographicSize;

            float referenceWidth = 2f * referenceOrthoSize * 16f / 9f;
            float referenceHeight = 2f * referenceOrthoSize;

            float scaleFactorX = currentWidth / referenceWidth;
            float scaleFactorY = currentHeight / referenceHeight;

            Vector3 newScale = transform.localScale;

            if (scaleX) newScale.x = referenceScale.x * scaleFactorX;
            if (scaleY) newScale.y = referenceScale.y * scaleFactorY;
            if (scaleZ) newScale.z = referenceScale.z * scaleFactorX;

            transform.localScale = newScale;
            float camHeight = cam.orthographicSize * 2f;
            float camWidth = camHeight * cam.aspect;

            Vector3 camPos = cam.transform.position;

            float left = camPos.x - camWidth / 2f;
            float right = camPos.x + camWidth / 2f;
            float top = camPos.y + camHeight / 2f;
            float bottom = camPos.y - camHeight / 2f;
            float centerX = camPos.x;
            float centerY = camPos.y;

            Vector3 targetPos = Vector3.zero;

            switch (anchorPos)
            {
                case AnchorPos.TopLeft:
                    targetPos = new Vector3(left, top, 0);
                    break;
                case AnchorPos.TopCenter:
                    targetPos = new Vector3(centerX, top, 0);
                    break;
                case AnchorPos.TopRight:
                    targetPos = new Vector3(right, top, 0);
                    break;
                case AnchorPos.MiddleLeft:
                    targetPos = new Vector3(left, centerY, 0);
                    break;
                case AnchorPos.MiddleCenter:
                    targetPos = new Vector3(centerX, centerY, 0);
                    break;
                case AnchorPos.MiddleRight:
                    targetPos = new Vector3(right, centerY, 0);
                    break;
                case AnchorPos.BottomLeft:
                    targetPos = new Vector3(left, bottom, 0);
                    break;
                case AnchorPos.BottomCenter:
                    targetPos = new Vector3(centerX, bottom, 0);
                    break;
                case AnchorPos.BottomRight:
                    targetPos = new Vector3(right, bottom, 0);
                    break;
            }

            transform.position = targetPos + anchorOffset;
        }
    }
    public enum AnchorPos
    {
        TopLeft,TopCenter, TopRight,MiddleLeft,MiddleCenter, MiddleRight,BottomLeft,BottomCenter,BottomRight
    }
}
