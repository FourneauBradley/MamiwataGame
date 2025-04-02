using TMPro;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class TextAnim : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToAnim;
    [SerializeField] private float zoomSpeed = 2f; 
    [SerializeField] private float zoomAmount = 0.1f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float minAngle = -10f; // Angle minimum
    [SerializeField] private float maxAngle = 10f; // Angle maximum

    private Vector3 baseScale;
    private float rotationDirection = 1f;
    private float currentAngle = 0f;
    private void Start()
    {
        if (textToAnim == null) textToAnim = GetComponent<TextMeshProUGUI>();
        baseScale = textToAnim.transform.localScale;
    }

    private void Update()
    {
        float scaleModifier = 1 + Mathf.Sin(Time.time * zoomSpeed) * zoomAmount;
        textToAnim.transform.localScale = baseScale * scaleModifier;
        currentAngle += rotationSpeed * rotationDirection * Time.deltaTime;

        if (currentAngle > maxAngle)
        {
            currentAngle = maxAngle;
            rotationDirection = -1f;
        }
        else if (currentAngle < minAngle)
        {
            currentAngle = minAngle;
            rotationDirection = 1f;
        }

        textToAnim.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
