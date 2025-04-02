using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(RectTransform))]
    public class MenuSize : MonoBehaviour
    {
        RectTransform RectTransform;
        [SerializeField] VerticalLayoutGroup VerticalLayoutGroup;
        private void Start()
        {
            RectTransform = GetComponent<RectTransform>();
        }
        void Update()
        {
            if (CheckResolution.isSmall)
            {
                RectTransform.anchorMin = new Vector2(0, 0); 
                RectTransform.anchorMax = new Vector2(1, 1);
                RectTransform.offsetMin = Vector2.zero;
                RectTransform.offsetMax = Vector2.zero;
                VerticalLayoutGroup.padding = new RectOffset(300,300,100,100);
            }
        }
    }
}