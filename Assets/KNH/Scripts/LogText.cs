using UnityEngine;
using TMPro;

public class LogText : MonoBehaviour
{
    TMP_Text text;
    float time = 0;
    float endTime = 1;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    public void SetHpLog(int amount)//체력 회복 아이템 획득시
    {
        text.text = "<color=green>+"+amount+ "</color>";
    }

    public void SetPosition(int position)//로그 여러개 있을 때 위로 이동
    {
        GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + new Vector3(0, position / 2f + 0.63f, 0);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= endTime)
        {
            Destroy(gameObject);
        }
    }
}
