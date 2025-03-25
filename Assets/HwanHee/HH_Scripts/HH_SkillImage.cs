using UnityEngine;
using UnityEngine.UI;

public class HH_SkillImage : MonoBehaviour
{
    Image img;
    int playerClass;

    void Awake()
    {
        img = GetComponent<Image>();

        playerClass = PlayerPrefs.GetInt("classNo");
        Sprite newSprite = null;

        // 전사
        if (playerClass == 0)
        {
            newSprite = Resources.Load<Sprite>("KnightSkillImg");
        }

        // 마법사
        else if (playerClass == 1)
        {
            newSprite = Resources.Load<Sprite>("WaterSkillImg");
        }

        // 궁수
        else if (playerClass == 2)
        {
            newSprite = Resources.Load<Sprite>("ArcherSkillImg");
        }

        img.sprite = newSprite;
    }
}
