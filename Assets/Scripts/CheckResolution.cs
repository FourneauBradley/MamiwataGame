using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CheckResolution : MonoBehaviour
    {
        [SerializeField] GameObject RotateScreenBg;
        public static bool isSmall = false;
        void Update()
        {

            if (Application.isMobilePlatform)
            {
                isSmall = true;
            }
            if (Screen.height > Screen.width)
            {
                Time.timeScale = 0;
                RotateScreenBg.SetActive(true);
            }
            else if (Screen.height <= Screen.width)
            {
                Time.timeScale = 1;
                RotateScreenBg.SetActive(false);
            }
            float screenWidthInInches = Screen.width / Screen.dpi;
            float screenHeightInInches = Screen.height / Screen.dpi;
            float screenDiagonal = Mathf.Sqrt(Mathf.Pow(screenWidthInInches, 2) + Mathf.Pow(screenHeightInInches, 2));
            
        }
    }
}