using UnityEngine;

public class HH_GodMode : MonoBehaviour
{
    Player player;
    bool isActive = false;
    CanvasGroup canvasGroup;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (player.GodMode && !isActive)
        {
            canvasGroup.alpha = 1f;
            isActive = true;
        }

        else if (!player.GodMode && isActive)
        {
            canvasGroup.alpha = 0f;
            isActive = false;
        }
    }
}
