using UnityEngine;
using UnityEngine.AI;

public class BossStats : MonoBehaviour
{
    public float maxHealth = 100f;  // ������ �ִ� ü��
    public float currentHealth;     // ������ ���� ü��
    public float damage = 10f;      // ������ ���ݷ�
    public float defense = 5f;      // ������ ���� (���Ƿ� �߰��� �� ����)
    public bool isDead = false;     // ������ �׾����� Ȯ���ϴ� ����

    private NavMeshAgent agent;     // NavMeshAgent
    private Animator animator;      // Animator

    void Start()
    {
        // �ʱ� ü�� ����
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // ü�� ���� �޼���
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return; // �׾����� �� �̻� ���ظ� ���� ����

        // ���¿� ���� ���� ���� ���
        float actualDamage = Mathf.Max(damageAmount - defense, 0);  // ������ ����� ���� ���
        currentHealth -= actualDamage;

        // ü�� ���� �ִϸ��̼� Ʈ���� (Hurt �ִϸ��̼�)
        animator.SetTrigger("Hurt");

        // ü���� 0 �����̸� ��� ó��
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // ���� ó�� �޼���
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death"); // Death �ִϸ��̼� ����
        agent.isStopped = true; // ������ �� �̻� �������� ����
        // �߰������� ���� �� ó���� �۾� (��: ������ ���, ���� ���� ��)
    }

    // ���� ���� �޼��� (�÷��̾�� ���ظ� �ִ� �޼���)
    public void AttackPlayer(GameObject player)
    {
        if (isDead) return; // �׾����� �������� ����

        // �÷��̾�� ���� �ֱ�
        //player.GetComponent<PlayerHealth>().TakeDamage(damage);  // �÷��̾ ���� Health ��ũ��Ʈ�� ������ �ִٰ� ����
    }

    // ü�� ȸ�� �޼��� (�ʿ�� �߰� ����)
    public void Heal(float healAmount)
    {
        if (isDead) return; // �׾����� ȸ������ ����

        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }

    // ���� ���� üũ �޼��� (��: ���� ����, ��ȭ ��� �ߵ� ��)
    void Update()
    {
        // ���� ��� ü�� 30% ������ �� ��ȭ ��� �ߵ�
        if (currentHealth <= maxHealth * 0.3f && !isDead)
        {
            // ��ȭ ��� ó�� (�ӵ� ���� ��)
            if (!agent.isStopped)
            {
                agent.speed = 6f; // �̵� �ӵ� ����
                // �߰����� ��ȭ ��� �ִϸ��̼��̳� ȿ�� ó�� ����
            }
        }
    }
}
