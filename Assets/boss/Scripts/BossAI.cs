using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform
    public float followRange = 10f;  // 플레이어를 추적할 거리
    public float attackRange = 2f;  // 공격 범위

    private NavMeshAgent agent;  // NavMeshAgent
    private Animator animator;   // Animator
    private bool isDead = false; // 보스의 상태 체크

    int health = 0;

    void Start()
    {
        // NavMeshAgent 컴포넌트를 가져옴
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");  // 초기 애니메이션을 Idle로 설정

        // 소환되자마자 플레이어를 추적하기 시작
        agent.SetDestination(player.position);  // 소환되자마자 플레이어의 위치로 이동
    }

    void Update()
    {
        if (isDead) return; // 보스가 죽었으면 더 이상 행동하지 않음

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어를 추적
        if (distanceToPlayer <= followRange)
        {
            agent.SetDestination(player.position);  // 플레이어 위치로 이동
            animator.SetBool("isWalking", true);  // 걷는 애니메이션 실행
        }
        else
        {
            animator.SetBool("isWalking", false);  // 걷는 애니메이션 종료
        }

        // 공격 범위에 들어갔을 때 공격 시작
        if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("Attack");  // 공격 애니메이션 실행
        }

        // 보스가 피해를 입었을 때
        if (Input.GetKeyDown(KeyCode.H)) // 예시로 H키를 누르면 피해 입음
        {
            animator.SetTrigger("Hurt");  // 피해 애니메이션 실행
        }

        // 보스가 죽었을 때
        if (health <= 0)  // 체력이 0 이하일 경우 죽음 처리
        {
            Die();
        }

        // 소환 후에도 계속해서 플레이어를 추적
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            // 플레이어의 위치로 계속 추적
            agent.SetDestination(player.position);
        }
    }
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");  // 죽음 애니메이션 실행
        agent.isStopped = true;  // 죽으면 이동 멈추기
    }
}
