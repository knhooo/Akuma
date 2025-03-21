using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Player player;
    GameObject playerObject;
    [SerializeField] Image hpBar;//체력바 이미지
    [SerializeField] Image expBar;//경험치바 이미지
    [SerializeField] TextMeshProUGUI levelText;//레벨 텍스트

    void Start()
    {
        //씬에 player오브젝트가 생성되었는지 확인
        if (GameManager.instance.player != null)
        {
            player = GameManager.instance.player.GetComponent<Player>();
        }

    }
    void Update()
    {
        if (player != null)
        {
            //체력바 업데이트
            hpBar.fillAmount = (float)player.Hp / (float)player.MaxHp;
            //경험치바 업데이트
            expBar.fillAmount = (float)player.Exp / (float)player.MaxExp;
            //레벨 업데이트
            levelText.text = "Lv." + player.Level.ToString();
        }
    }
}