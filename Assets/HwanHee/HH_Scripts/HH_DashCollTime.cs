using UnityEngine;
using UnityEngine.UI;

public class HH_DashCollTime : MonoBehaviour
{
    Image Gage;
    Player player;
    float val = 0;

    private void Awake()
    {
        Gage = GetComponent<Image>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        val = Mathf.InverseLerp(player.DashCoolTime, 0f, player.DashCoolTimer);
        Gage.fillAmount = val;

    }
}
