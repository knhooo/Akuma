using UnityEngine;
using UnityEngine.UI;

public class HH_SkillCoolTime : MonoBehaviour
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
        if(player.CanUseSkill)
        {
            val = Mathf.InverseLerp(player.SkillCoolTime, 0f, player.SkillCoolTimer);
            Gage.fillAmount = val;
        }
    }
}
