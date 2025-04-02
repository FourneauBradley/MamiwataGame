using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class TouchInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static int touchDirection = 0;
        [SerializeField] private bool isLeft;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isLeft) { 
                touchDirection = -1;
            }
            else
            {
                touchDirection = 1 ;
            }

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            touchDirection = 0;
        }
    }
}