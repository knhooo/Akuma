using UnityEngine;

public class Augment : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void SetMaxHP(int amount)
    {
        player.GetComponent<Player>().MaxHp += amount;
        gameObject.SetActive(false);
    }
    public void SetAttack(int amount)
    {
        player.GetComponent<Player>().Attack += amount;
        gameObject.SetActive(false);
    }
    public void SetSpeed(int amount)
    {
        player.GetComponent<Player>().Speed += amount;
        gameObject.SetActive(false);
    }
}
