using UnityEngine;
using UnityEngine.UI;

public class HH_DashButtonClick : MonoBehaviour
{
    public Text text;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public float speed = 2f;
    Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (!player.isDashClick)
        {
            float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * speed, 1));
            text.transform.localScale = new Vector3(scale, scale, 1);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}