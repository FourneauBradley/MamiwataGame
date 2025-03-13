using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private Player player;

    private void Update()
    {
        scoreTxt.text = "Score : "+ player.score;
    }
}
