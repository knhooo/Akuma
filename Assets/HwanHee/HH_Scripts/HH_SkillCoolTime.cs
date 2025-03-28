using UnityEngine;
using UnityEngine.UI;

public class HH_SkillCoolTime : MonoBehaviour
{
    [SerializeField] string skillStr;
    Image Gage;
    Player player;
    float val = 0;
    float skillCoolTime;
    float skillCoolTimer;

    private void Start()
    {
        Gage = GetComponent<Image>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (skillStr == "Skill" || skillStr == "skill")
        {
            skillCoolTime = player.SkillCoolTime;
        }
        else if (skillStr == "Dash" || skillStr == "dash")
        {
            skillCoolTime = player.DashCoolTime;
        }
    }

    void Update()
    {
        if (skillStr == "Skill" || skillStr == "skill")
        {
            skillCoolTimer = player.SkillCoolTimer;
        }
        else if (skillStr == "Dash" || skillStr == "dash")
        {
            skillCoolTimer = player.DashCoolTimer;
        }

        val = Mathf.InverseLerp(skillCoolTime, 0f, skillCoolTimer);
        Gage.fillAmount = val;
    }
}
