using UnityEngine;
using TMPro;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Unity.Burst.Intrinsics;

public class LogText : MonoBehaviour
{
    TMP_Text text;
    float time = 0;
    float endTime = 1;
    public TMP_FontAsset DmgFont;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    public void SetHpLog(int amount)//체력 회복 아이템 획득시
    {
        text.text = "<color=green>+" + amount + "</color>";
    }

    public void SetDmgLog(int amount)//몬스터가 받는 데미지
    {
        text.font = DmgFont;
        text.fontSize = 7;
        text.text = "<color=red>" + amount + "</color>";
    }

    public void SetPlayerDmgLog(int amount)//플레이어가 받는 데미지
    {
        text.font = DmgFont;
        text.fontSize = 7;
        text.text = "<color=#FFA128>" + amount + "</color>";
    }

    public void SetPosition(int position)//로그 여러개 있을 때 위로 이동
    {
        GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + new Vector3(0, position / 2f + 0.63f, 0);
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            HH_Monster mon = transform.parent.GetComponent<HH_Monster>();
            if (mon != null && mon.isActiveAndEnabled && mon.MaxHp == mon.Hp)
            {
                Destroy(gameObject);
            }
        }

        time += Time.deltaTime;
        transform.localScale = transform.parent.localScale;

        if (time >= endTime)
        {
            Destroy(gameObject);
        }
    }
}
