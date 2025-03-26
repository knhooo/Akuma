using Mono.Cecil.Cil;
using UnityEngine;

public class Augment : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject augmentListUI;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void SetMaxHP(int amount)
    {
        player.GetComponent<Player>().MaxHp += amount;
        augmentListUI.gameObject.transform.GetComponent<AugmentList>().SetCode(0);
        EndAugment();
    }
    public void SetAttack(int amount)
    {
        player.GetComponent<Player>().Attack += amount;
        augmentListUI.gameObject.transform.GetComponent<AugmentList>().SetCode(1);
        EndAugment();
    }
    public void SetSpeed(int amount)
    {
        player.GetComponent<Player>().Speed += amount;
        augmentListUI.gameObject.transform.GetComponent<AugmentList>().SetCode(2);
        EndAugment();
    }

    void EndAugment()
    {
        Time.timeScale = 2f;//시간정지해제
        augmentListUI.gameObject.transform.GetComponent<AugmentList>().AddAugmentIcon();
        gameObject.SetActive(false);
    }
}
