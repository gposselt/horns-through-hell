using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UserInfoText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private TextMeshProUGUI textBox;
    private Player player;

    private void Start()
    {
        textBox = GetComponent<TextMeshProUGUI>();
        player = PlayerSpawnBlock.Instance.player;
    }

    void Update()
    {
        textBox.text = $"Health: {player.playerHp}\nSelected: {player.activeMask}";
    }
}
