using UnityEngine;
using UnityEngine.UI;

public class HealBar : MonoBehaviour
{
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Player player;

    private void Update()
    {
        currentHealthBar.fillAmount = player.currentHealth / 10f;
    }
}
