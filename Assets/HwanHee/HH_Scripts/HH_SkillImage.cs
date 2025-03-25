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

        // ����
        if (playerClass == 0)
        {
            newSprite = Resources.Load<Sprite>("KnightSkillImg");
        }

        // ������
        else if (playerClass == 1)
        {
            newSprite = Resources.Load<Sprite>("WaterSkillImg");
        }

        // �ü�
        else if (playerClass == 2)
        {
            newSprite = Resources.Load<Sprite>("ArcherSkillImg");
        }

        img.sprite = newSprite;
    }
}
