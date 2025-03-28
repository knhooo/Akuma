using UnityEngine;
using UnityEngine.UI;

public class HH_StatusText : MonoBehaviour
{
    [SerializeField]
    string statusStr;

    Player player;
    Text text;
    int maxStatus = 0;
    int status = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (statusStr == "Hp" || statusStr == "hp")
        {
            maxStatus = player.MaxHp;
            status = player.Hp;
        }
        else if (statusStr == "Exp" || statusStr == "exp")
        {
            maxStatus = player.MaxExp;
            status = player.Exp;
        }

        text.text = status + " / " + maxStatus;
    }
}
