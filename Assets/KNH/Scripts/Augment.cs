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
        Time.timeScale = 1f;//시간정지해제
        gameObject.SetActive(false);
    }
    public void SetAttack(int amount)
    {
        player.GetComponent<Player>().Attack += amount;
        Time.timeScale = 1f;//시간정지해제
        gameObject.SetActive(false);
    }
    public void SetSpeed(int amount)
    {
        player.GetComponent<Player>().Speed += amount;
        Time.timeScale = 1f;//시간정지해제
        gameObject.SetActive(false);
    }
}
