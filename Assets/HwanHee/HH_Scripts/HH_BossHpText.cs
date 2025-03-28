using UnityEngine;
using UnityEngine.UI;

public class HH_BossHpText : MonoBehaviour
{
    BossAI boss;
    Text text;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossAI>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = boss.currentHP + " / " + boss.maxHP;
    }
}
