using UnityEngine;

public class BossStats : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isEnraged = false;

    private BossAI bossAI;

    void Start()
    {
        currentHealth = maxHealth;
        bossAI = GetComponent<BossAI>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (!isEnraged && currentHealth <= maxHealth * 0.3f)
        {
            EnterEnragedMode();
        }
    }

    private void EnterEnragedMode()
    {
        isEnraged = true;
        bossAI.moveSpeed *= 1.5f;
        Debug.Log("ÆäÀÌÁî2");
    }

    private void Die()
    {
        Debug.Log("Boss »ç¸Á");
        Destroy(gameObject, 2f);
    }
}
