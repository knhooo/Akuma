using UnityEngine;
using UnityEngine.AI;

public class BossStats : MonoBehaviour
{
    public float maxHealth = 100f;  // 보스의 최대 체력
    public float currentHealth;     // 보스의 현재 체력
    public float damage = 10f;      // 보스의 공격력
    public float defense = 5f;      // 보스의 방어력 (임의로 추가할 수 있음)
    public bool isDead = false;     // 보스가 죽었는지 확인하는 변수

    private NavMeshAgent agent;     // NavMeshAgent
    private Animator animator;      // Animator

    void Start()
    {
        // 초기 체력 설정
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // 체력 감소 메서드
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return; // 죽었으면 더 이상 피해를 받지 않음

        // 방어력에 따라 실제 피해 계산
        float actualDamage = Mathf.Max(damageAmount - defense, 0);  // 방어력을 고려한 피해 계산
        currentHealth -= actualDamage;

        // 체력 감소 애니메이션 트리거 (Hurt 애니메이션)
        animator.SetTrigger("Hurt");

        // 체력이 0 이하이면 사망 처리
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // 죽음 처리 메서드
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death"); // Death 애니메이션 실행
        agent.isStopped = true; // 죽으면 더 이상 움직이지 않음
        // 추가적으로 죽은 후 처리할 작업 (예: 아이템 드롭, 보스 제거 등)
    }

    // 보스 공격 메서드 (플레이어에게 피해를 주는 메서드)
    public void AttackPlayer(GameObject player)
    {
        if (isDead) return; // 죽었으면 공격하지 않음

        // 플레이어에게 피해 주기
        //player.GetComponent<PlayerHealth>().TakeDamage(damage);  // 플레이어가 따로 Health 스크립트를 가지고 있다고 가정
    }

    // 체력 회복 메서드 (필요시 추가 가능)
    public void Heal(float healAmount)
    {
        if (isDead) return; // 죽었으면 회복하지 않음

        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }

    // 보스 상태 체크 메서드 (예: 상태 점검, 강화 모드 발동 등)
    void Update()
    {
        // 예를 들어 체력 30% 이하일 때 강화 모드 발동
        if (currentHealth <= maxHealth * 0.3f && !isDead)
        {
            // 강화 모드 처리 (속도 증가 등)
            if (!agent.isStopped)
            {
                agent.speed = 6f; // 이동 속도 증가
                // 추가적인 강화 모드 애니메이션이나 효과 처리 가능
            }
        }
    }
}
