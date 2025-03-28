using UnityEngine;

public class LaserHitBox : MonoBehaviour
{
    private int skillDamage;

    public void SetDamage(int dmg)
    {
        skillDamage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            HH_Monster monster = collision.GetComponent<HH_Monster>();
            if (monster != null)
            {
                monster.SendMessage("TakeDamageFromArrow", skillDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (collision.CompareTag("Boss"))
        {
            BossAI boss = collision.GetComponent<BossAI>();
            if (boss != null)
            {
                boss.TakeDamage(skillDamage); // public 으로 바꿨는지 확인!
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }

}
