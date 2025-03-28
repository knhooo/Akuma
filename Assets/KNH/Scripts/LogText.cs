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
    public void SetHpLog(int amount)//ü�� ȸ�� ������ ȹ���
    {
        text.text = "<color=green>+" + amount + "</color>";
    }

    public void SetDmgLog(int amount)//���Ͱ� �޴� ������
    {
        text.font = DmgFont;
        text.fontSize = 7;
        text.text = "<color=red>" + amount + "</color>";
    }

    public void SetPlayerDmgLog(int amount)//�÷��̾ �޴� ������
    {
        text.font = DmgFont;
        text.fontSize = 7;
        text.text = "<color=#FFA128>" + amount + "</color>";
    }

    public void SetPosition(int position)//�α� ������ ���� �� ���� �̵�
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
