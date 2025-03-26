using UnityEngine;
using UnityEngine.UI;

public class HH_SkillCoolTime : MonoBehaviour
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
        val = Mathf.InverseLerp(player.SkillCoolTime, 0f, player.SkillCoolTimer);
        Gage.fillAmount = val;
    }
}
