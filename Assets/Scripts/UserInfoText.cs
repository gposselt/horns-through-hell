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
        if (player.activeMask == Masks.None)
        {
            textBox.text = $"Health: {player.playerHp}\nSelected: {player.activeMask}";
        } else
        {
            textBox.text = $"Health: {player.playerHp}\nSelected: {player.activeMask}\nAmmo: {player.ammo.remainingAmmo[player.activeMask]}";
        }

    }
}
