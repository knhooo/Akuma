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
            Debug.Log("�������� ���Ϳ� ����!");

            HH_Monster monster = collision.GetComponent<HH_Monster>();
            if (monster != null)
            {
                monster.SendMessage("TakeDamageFromArrow", skillDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
