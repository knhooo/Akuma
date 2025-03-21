using UnityEngine;

public class HH_Blood : MonoBehaviour
{
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        transform.SetParent(player.transform);
    }

    void AnimationFinished()
    {
        Destroy(gameObject);
    }
}
