using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform player;  // 플레이어의 위치
    public float followRange = 10f;  // 플레이어를 추적할 거리
    public float attackRange = 1f;  // 공격 범위

    public float moveSpeed = 1f;  // 이동 속도
    public float stoppingDistance = 0.5f;  // 목표와의 최소 거리


    private bool isHurt = false; // 피해 상태 체크
    private SpriteRenderer spriteRenderer;  // 스프라이트 렌더러
    private Animator animator;   // Animator
    private bool isDead = false; // 보스의 상태 체크

    int health = 100;  // 보스 체력

    void Start()
    {
        // 스프라이트 렌더러와 Animator 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 초기 애니메이션 설정
        animator.SetBool("isWalking", false);  // 초기 상태는 걷지 않음
        animator.SetBool("isAttacking", false);  // 초기 상태는 공격하지 않음
        animator.SetBool("isDead", false);  // 초기 상태는 죽지 않음
    }

    void Update()
    {
        if (isHurt) return;  // 피해 중이면 이동 & 공격 멈춤

        if (isDead) return;  // 보스가 죽었으면 더 이상 행동하지 않음

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어를 추적
        if (distanceToPlayer <= followRange && distanceToPlayer > stoppingDistance)
        {
            animator.SetBool("isWalking", true);  // 걷는 애니메이션 실행
            MoveTowardsPlayer();  // 플레이어를 따라 이동
        }
        else
        {
            animator.SetBool("isWalking", false);  // 걷는 애니메이션 종료
        }

        // 공격 범위에 들어갔을 때 공격 시작
        if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("isAttacking", true);  // 공격 애니메이션 실행
        }
        else
        {
            animator.SetBool("isAttacking", false);  // 공격 애니메이션 종료
        }

        // 보스가 피해를 입었을 때
        if (Input.GetKeyDown(KeyCode.Space)) // 예시로 Space키를 누르면 피해 입음
        {
            Debug.Log("Hurt Trigger 실행됨");
            StartCoroutine(PlayHurtAnimation());
        }

        // 보스가 죽었을 때
        if (health <= 0)
        {
            Die();  // 죽음 처리
        }

        // 보스가 플레이어를 따라가면서 좌우 반전 처리
        if (player.position.x < transform.position.x)
        {
            // 플레이어가 보스의 오른쪽에 있으면 스프라이트를 반전
            spriteRenderer.flipX = false;
        }
        else if (player.position.x > transform.position.x)
        {
            // 플레이어가 보스의 왼쪽에 있으면 스프라이트를 정상적으로 유지
            spriteRenderer.flipX = true;
        }
    }

    void MoveTowardsPlayer()
    {
        // 플레이어 방향으로 이동
        Vector3 direction = (player.position - transform.position).normalized;

        // transform을 직접 수정하여 이동
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    public void TakeDamage(int damage)
    {
        if (isHurt) return; // 이미 피해 상태면 실행 안 함

        health -= damage;
        if (health <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(PlayHurtAnimation());
    }

    private IEnumerator PlayHurtAnimation()
    {
        isHurt = true;  // 보스를 피해 상태로 설정
        animator.SetTrigger("HurtTrigger");  // 피해 애니메이션 실행

        // 1️. 이동 멈춤
        float originalSpeed = moveSpeed;
        moveSpeed = 0f;

        // 2️. 애니메이션 재생 시간만큼 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 3️. 이동 재개
        moveSpeed = originalSpeed;
        isHurt = false;
    }

    void Die()
    {
        isDead = true;  // 보스 죽음 상태로 설정
        animator.SetBool("isDead", true);  // 죽음 애니메이션 실행

        // 죽은 후 처리 (예시로 보스를 비활성화)
        Destroy(gameObject, 2f);  // 2초 후 보스 객체 제거 (죽음 애니메이션 후)
    }
}
