using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;  // 스프라이트 렌더러
    private Animator animator;   // Animator

    private bool isHurt = false; // 피해 상태 체크
    private bool isDead = false; // 보스의 사망 상태 체크
    private bool isMoving = true;  // 이동 상태 체크

    public Transform player;  // 플레이어의 위치
    public float followRange = 10f;  // 플레이어를 추적할 거리
    public float attackRange = 1f;  // 공격 범위

    public float moveSpeed = 1f;  // 이동 속도
    public float stoppingDistance = 0.5f;  // 목표와의 최소 거리

    int health = 100;  // 보스 체력

    public GameObject spellPrefab; // Spell 프리팹
    public Transform spellSpawnPoint; // Spell 생성 위치

    public int spellCount = 5; // 한 번에 생성할 Spell 개수
    public float spellRangeX = 3f; // X축 랜덤 범위
    public float spellRangeY = 2f; // Y축 랜덤 범위

    public Camera mainCamera; // 메인 카메라


    void Start()
    {
        // 스프라이트 렌더러와 Animator 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 초기 애니메이션 설정
        animator.SetBool("isWalking", false);  // 초기 상태는 걷지 않음
        animator.SetBool("isAttacking", false);  // 초기 상태는 공격하지 않음
    }

    void Update()
    {       
        if (isDead) return;  // 보스가 죽었으면 더 이상 행동하지 않음

        if (!isHurt) // 피해 상태가 아니라면
        {
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
        }
        // 보스가 캐스팅 할 때
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Cast 실행됨");
            StartCoroutine(PlayCastAnimation());
            CastSpell();
        }


        // 보스가 피해를 입었을 때
        if (Input.GetKeyDown(KeyCode.Space)) // 예시로 Space키를 누르면 피해 입음
        {
            if (isDead) return; // 이미 죽은 상태라면 적용 안 함
            else
            {
                Debug.Log("Hurt Trigger 실행됨");
                TakeDamage(10); // 예시로 입혀봄               
            }                         
        }
        
        // 보스 죽음 테스트
        if (Input.GetKeyDown(KeyCode.X))
        {
            Die();
        }

        // 보스가 죽었을 때
        if (health <= 0)
        {
            if (isDead) return;
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
        if (isDead) return; // 이미 죽은 상태면 더 이상 피해를 받지 않음
        else
        {
            // 피해 처리
            Debug.Log($"Damage 실행됨. 데미지 값: {damage}");
            health -= damage;

            // 체력이 0 이하로 떨어지면 죽음 처리
            if (health <= 0)
            {
                Die();
                return;
            }

            // 피해 애니메이션 실행
            StartCoroutine(PlayHurtAnimation());
        }
    }

    //캐스팅
    void CastSpell()
    {
        Debug.Log("Cast 시작! 1초 후 Spell 발동 예정");
        Invoke("SpawnSpell", 1f);
    }

    //스펠 소환
    void SpawnSpell()
    {
        Debug.Log("Spell 실행됨!");
        for (int i = 0; i < spellCount; i++)
        {
            Vector3 randomPosition = GetRandomSpellPositionInCameraView();

            // Spell 생성
            Instantiate(spellPrefab, randomPosition, Quaternion.identity);
        }
    }

    //스펠 범위 카메라 시야 내로 설정
    private Vector3 GetRandomSpellPositionInCameraView()
    {
        // 카메라의 화면 크기와 범위 계산
        float screenMinX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;
        float screenMaxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)).x;
        float screenMinY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).y;
        float screenMaxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane)).y;

        // 랜덤 위치 계산 (카메라 화면 내)
        float randomX = Random.Range(screenMinX, screenMaxX);
        float randomY = Random.Range(screenMinY, screenMaxY);

        // z값은 고정, 2D에서 Z는 0으로 유지
        return new Vector3(randomX, randomY, 0f);
    }

    //피해 상태 애니메이션
    private IEnumerator PlayHurtAnimation()
    {
        isHurt = true;  // 보스를 피해 상태로 설정
        animator.ResetTrigger("HurtTrigger"); // 이전 트리거 제거
        animator.SetTrigger("HurtTrigger");  // 피해 애니메이션 실행

        // 이동 멈춤
        isMoving = false;  // 이동 상태를 멈추도록 설정

        // 2️. 애니메이션 재생 시간만큼 대기 (애니메이션 길이에 맞춰 대기)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 이동 재개
        isMoving = true;  // 피해 애니메이션이 끝나면 이동을 재개
        isHurt = false;  // 피해 상태 해제
    }


    //캐스팅 애니메이션
    private IEnumerator PlayCastAnimation()
    {
        animator.SetTrigger("Cast"); // 캐스팅 애니메이션 실행

        // 1️. 이동 멈춤
        float originalSpeed = moveSpeed;
        moveSpeed = 0f;

        // 2️. 애니메이션 재생 시간만큼 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 3️. 이동 재개
        moveSpeed = originalSpeed;
    }

    //보스 사망
    void Die()
    {
        if (isDead) return; // 이미 죽은 상태라면 리턴
        isDead = true;  // 보스 죽음 상태로 설정
        animator.SetTrigger("Dead");  // 죽음 애니메이션 실행

        // 애니메이션이 끝날 때까지 대기
        StartCoroutine(DieAfterAnimation());               
    }

    // 애니메이션이 끝난 후 보스 제거
    private IEnumerator DieAfterAnimation()
    {
        // 애니메이션이 실행 중일 때
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션이 끝날 때까지 대기
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;  // 매 프레임마다 애니메이션 진행 상태를 확인
        }

        // 죽은 후 보스 제거
        Destroy(gameObject);
    }
}
