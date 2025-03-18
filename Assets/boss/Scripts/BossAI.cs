using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform player;  // �÷��̾��� Transform
    public float followRange = 10f;  // �÷��̾ ������ �Ÿ�
    public float attackRange = 2f;  // ���� ����

    private NavMeshAgent agent;  // NavMeshAgent
    private Animator animator;   // Animator
    private bool isDead = false; // ������ ���� üũ

    int health = 0;

    void Start()
    {
        // NavMeshAgent ������Ʈ�� ������
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");  // �ʱ� �ִϸ��̼��� Idle�� ����

        // ��ȯ���ڸ��� �÷��̾ �����ϱ� ����
        agent.SetDestination(player.position);  // ��ȯ���ڸ��� �÷��̾��� ��ġ�� �̵�
    }

    void Update()
    {
        if (isDead) return; // ������ �׾����� �� �̻� �ൿ���� ����

        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾ ����
        if (distanceToPlayer <= followRange)
        {
            agent.SetDestination(player.position);  // �÷��̾� ��ġ�� �̵�
            animator.SetBool("isWalking", true);  // �ȴ� �ִϸ��̼� ����
        }
        else
        {
            animator.SetBool("isWalking", false);  // �ȴ� �ִϸ��̼� ����
        }

        // ���� ������ ���� �� ���� ����
        if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("Attack");  // ���� �ִϸ��̼� ����
        }

        // ������ ���ظ� �Ծ��� ��
        if (Input.GetKeyDown(KeyCode.H)) // ���÷� HŰ�� ������ ���� ����
        {
            animator.SetTrigger("Hurt");  // ���� �ִϸ��̼� ����
        }

        // ������ �׾��� ��
        if (health <= 0)  // ü���� 0 ������ ��� ���� ó��
        {
            Die();
        }

        // ��ȯ �Ŀ��� ����ؼ� �÷��̾ ����
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            // �÷��̾��� ��ġ�� ��� ����
            agent.SetDestination(player.position);
        }
    }
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");  // ���� �ִϸ��̼� ����
        agent.isStopped = true;  // ������ �̵� ���߱�
    }
}
