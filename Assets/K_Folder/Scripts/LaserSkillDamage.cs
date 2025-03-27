using UnityEngine;

public class LaserSkillDamage : MonoBehaviour
{
    public int damage = 30;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            HH_Monster monster = other.GetComponent<HH_Monster>();
         /*   if (monster != null)
            {
                monster.TakeHit(damage); // ¶Ç´Â TakeHit
            }*/
        }
    }
}
