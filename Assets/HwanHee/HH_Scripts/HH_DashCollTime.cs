using UnityEngine;
using UnityEngine.UI;

public class HH_DashCollTime : MonoBehaviour
{
    Image Gage;
    HH_Knight player;
    float val = 0;

    private void Awake()
    {
        Gage = GetComponent<Image>();
        player = GameObject.FindWithTag("Player").GetComponent<HH_Knight>();
    }

    void Update()
    {
        if (player.CanUseDash)
        {
            val = Mathf.InverseLerp(player.DashCoolTime, 0f, player.DashCoolTimer);
            Gage.fillAmount = val;
        }
    }
}
