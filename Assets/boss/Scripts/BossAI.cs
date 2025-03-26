using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Camera mainCamera; // 메인 카메라
    private SpriteRenderer spriteRenderer;  // 스프라이트 렌더러
    private Animator animator;   // Animator

    private bool isHurt = false; // 피해 상태 체크
    private bool isDead = false; // 보스의 사망 상태 체크
    private bool isMoving = true;  // 이동 상태 체크
    private bool isEnraged = false; // 강화 모드 여부 확인
    private bool isCasting = false;

    public Transform player;  // 플레이어의 위치
    public float followRange = 10f;  // 플레이어를 추적할 거리
    public float attackRange = 1f;  // 공격 범위
    public float attackDamage = 10f;  // 공격 데미지
    public float attackCooldown = 2f;  // 공격 쿨타임
    private float lastAttackTime = 0f;  // 마지막 공격 시간

    public float moveSpeed = 1f;  // 이동 속도
    public float stoppingDistance = 0.5f;  // 목표와의 최소 거리

    public float maxHP = 500f; // 보스 최대 체력
    public float currentHP = 500f;  // 보스 현재 체력

    public GameObject redDotPrefab;  // RedDot 프리팹
    public GameObject bossSpellPrefab; // Spell 프리팹
    public Transform spellSpawnPoint; // Spell 생성 위치
    public int spellCount = 10; // 한 번에 생성할 Spell 개수
    public float spellRangeX = 3f; // X축 랜덤 범위
    public float spellRangeY = 2f; // Y축 랜덤 범위

    private float castCooldown = 8f; // 기본 쿨타임 설정
    private float nextCastTime = 2f; // 다음 Cast 가능 시간

    // 강화 모드에서 순간이동 관련 변수 추가
    private float teleportCooldown = 3f;  // 순간이동 쿨타임
    private float teleportTimer = 0f;     // 순간이동 타이머

    //탄막
    public GameObject radialBulletPrefab;  // 방사형 탄막 프리팹
    public GameObject trackingBulletPrefab;  // 추적 탄막 프리팹
    public Transform firePoint;             // 발사 지점

    public int bulletCount = 8; // 한 번에 발사할 탄 수
    public float bulletSpeed = 3f; // 방사형 탄막 속도
    public float bulletFireRate = 3f; // 방사형 탄막 발사 주기

    public int trackingBulletCount = 5; // 한 번에 발사할 탄 수
    public float trackingBulletSpeed = 3f; // 추적 탄막 속도
    public float trackingInterval = 0.5f; // 추적 탄막 간격    

    private float nextTrTime = 6f; // 추적 탄막 생성 주기
    private float TrCooldown = 6f; // 추적 탄막 쿨타임
    private bool isTrFiring = false;


    void Start()
    {
        // 스프라이트 렌더러와 Animator 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 초기 애니메이션 설정
        animator.SetBool("isWalking", false);  // 초기 상태는 걷지 않음
        animator.SetBool("isAttacking", false);  // 초기 상태는 공격하지 않음

        currentHP = maxHP;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(FireRadialBullets());  // 방사형 탄막 루틴

    }
    
    void Update()
    {       
        if (isDead) return;  // 보스가 죽었으면 더 이상 행동하지 않음
        
        // 플레이어를 추적
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 공격 쿨타임 지나고, 범위 내에 플레이어가 있으면 공격 실행
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;  // 마지막 공격 시간 갱신
        }

        // Cast 패턴
        if (Time.time >= nextCastTime && !isCasting)
        {
            CastSpell();
            nextCastTime = Time.time + castCooldown; // 다음 Cast 타이밍 설정
        }

        // 추적 탄막 패턴
        if (Time.time >= nextTrTime && !isTrFiring)
        {
            FireTrackingBullets();
            nextTrTime = Time.time + TrCooldown;
        }

        // 피해 상태 아닐 때
        if (!isHurt)
        {                    
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

        // 캐스팅 테스트(C버튼)
        if (Input.GetKeyDown(KeyCode.C))
        {
            CastSpell();
        }

        // 보스 데미지 테스트(Space 버튼)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDead) TakeDamage(10);
        }
        
        // 보스 죽음 테스트(X 버튼)
        if (Input.GetKeyDown(KeyCode.X))
        {
            Die();
        }                

        // 체력이 30% 이하 & 강화 모드가 아니라면 강화 모드 진입
        if (currentHP <= maxHP * 0.3f && !isEnraged)
        {
            EnterEnragedMode();
        }
        // 보스가 체력 0이면 죽음
        if (currentHP <= 0 && !isDead)
        {
            Die();
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


    // 보스 이동
    void MoveTowardsPlayer()
    {
        if (!isMoving) return;
        // 플레이어 방향으로 이동
        Vector3 direction = (player.position - transform.position).normalized;
        // transform을 직접 수정하여 이동
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    // 보스 데미지 받는 기능
    public void TakeDamage(int damage)
    {
        if (isDead) return; // 이미 죽은 상태면 더 이상 피해를 받지 않음        
        Debug.Log($"Damage 실행됨. 데미지 값: {damage}");
        currentHP -= damage;
        Debug.Log("보스 체력: " + currentHP);
        // 체력이 0 이하로 떨어지면 죽음 처리
        if (currentHP <= 0)
        {
            Die();
            return;
        }        
        
        // 피해 애니메이션 실행
        StartCoroutine(PlayHurtAnimation());        
    }

    // 플레이어 공격 함수
    private void AttackPlayer()
    {
        //실제 공격 실행 (플레이어에게 데미지 주기)
        player.GetComponent<Player>().TakeDamage(Mathf.RoundToInt(attackDamage));
        Debug.Log("보스가 플레이어를 공격했습니다! 데미지: " + attackDamage);
    }

    // 강화 모드
    private void EnterEnragedMode()
    {
        isEnraged = true;
        Debug.Log("⚡ 강화 패턴 발동!");
        moveSpeed *= 3f;
        animator.speed = 1.5f;
        castCooldown *= 0.5f;
        spriteRenderer.color = Color.red;
        StartCoroutine(TeleportCoroutine());
    }

    // 캐스팅
    void CastSpell()
    {
        Debug.Log("Cast 실행됨");
        StartCoroutine(PlayCastAnimation());        
    }

    // 스펠 소환
    void SpawnSpell()
    {
        Debug.Log("Spell 실행됨");
        for (int i = 0; i < spellCount; i++)
        {
            // RedDot를 랜덤한 위치에 생성
            Vector3 randomPosition = GetRandomSpellPositionInCameraView();
            GameObject redDot = Instantiate(redDotPrefab, randomPosition, Quaternion.identity);

            // RedDot가 일정 시간 후 삭제된 후 BossSpell이 생성되도록 하는 코루틴
            StartCoroutine(HandleRedDotAndSpell(redDot));
        }
    }

    // 스펠 범위 카메라 시야 내로 설정
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

   
    // 피해 상태 애니메이션
    private IEnumerator PlayHurtAnimation()
    {
        isHurt = true;  // 보스를 피해 상태로 설정
        isMoving = false; // 이동 멈춤
        animator.ResetTrigger("HurtTrigger"); // 이전 트리거 제거
        animator.SetTrigger("HurtTrigger");  // 피해 애니메이션 실행

        // 애니메이션 재생 시간만큼 대기 (애니메이션 길이에 맞춰 대기)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 이동 재개
        isMoving = true;  // 피해 애니메이션이 끝나면 이동을 재개
        isHurt = false;  // 피해 상태 해제
    }


    // 캐스팅 애니메이션
    private IEnumerator PlayCastAnimation()
    {
        isMoving = false;
        isCasting = true;
        animator.SetTrigger("Cast"); // 캐스팅 애니메이션 실행

        // 강화 모드일 경우 애니메이션이 빨리 끝남
        float castDuration = 0.5f / animator.speed;
        yield return new WaitForSeconds(castDuration);

        // 애니메이션 재생 시간만큼 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 이동 재개
        isCasting = false; // Cast 상태 해제
        isMoving = true;

        SpawnSpell();
    }


    // RedDot 사라진 후 BossSpell을 생성하는 코루틴
    IEnumerator HandleRedDotAndSpell(GameObject redDot)
    {
        // RedDot n초 후 삭제
        yield return new WaitForSeconds(2f);

        // RedDot 위치 저장 후 삭제
        Vector3 spawnPosition = redDot.transform.position;
        Destroy(redDot);

        // RedDot이 삭제된 후 같은 위치에 BossSpell 생성
        GameObject bossSpell = Instantiate(bossSpellPrefab, spawnPosition, Quaternion.identity);

        // BossSpell에 explosionPoint 설정
        BossSpell bossSpellScript = bossSpell.GetComponent<BossSpell>();
        if (bossSpellScript != null)
        {
            bossSpellScript.explosionPoint = bossSpell.transform;  // explosionPoint를 BossSpell의 위치로 설정
        }
    }

    // 순간이동 코루틴
    private IEnumerator TeleportCoroutine()
    {
        while (isEnraged)  // 강화 모드가 유지되는 동안
        {
            teleportTimer -= Time.deltaTime;  // 쿨타임 감소
            // 쿨타임이 끝나면 순간이동 실행
            if (teleportTimer <= 0f)
            {
                Teleport();  // 순간이동 함수 호출
                teleportTimer = teleportCooldown;  // 쿨타임 리셋
            }
            yield return null;  // 매 프레임마다 대기
        }
    }


    // 순간이동 함수
    private void Teleport()
    {
        // 플레이어 근처에 랜덤 위치를 생성
        Vector3 teleportPosition = GetRandomTeleportPosition();
        // 보스 순간이동
        transform.position = teleportPosition;
        // 순간이동 후 잠시 이동속도 증가 후 원래 속도로 복귀
        StartCoroutine(AfterTeleport());
    }


    // 순간이동 후 행동
    private IEnumerator AfterTeleport()
    {
        // 잠시 대기 (순간이동 후 짧은 시간 멈추기)
        float prevspd = moveSpeed;
        moveSpeed = 6f; Debug.Log("⚡ 이동 속도 증가: " + moveSpeed);
        yield return new WaitForSeconds(0.8f);
        moveSpeed = prevspd; Debug.Log("🛑 원래 속도로 복귀: " + moveSpeed);
    }


    // 순간이동 위치 계산
    private Vector3 GetRandomTeleportPosition()
    {
        // 플레이어 주변 랜덤 위치 계산 (플레이어 중심으로 반경 5m 내외)
        float offsetX = Random.Range(-5f, 5f);
        float offsetY = Random.Range(-5f, 5f);

        // 새로운 위치 반환
        return player.position + new Vector3(offsetX, offsetY, 0f);
    }

    // 방사형 탄막 코루틴
    private IEnumerator FireRadialBullets()
    {
        while (true)
        {
            FireRadialPattern(); // 방사형 탄막 발사
            yield return new WaitForSeconds(bulletFireRate);
        }
    }

    // 방사형 탄막 패턴
    void FireRadialPattern()
    {
        int bulletCount = 12; // 한 번에 발사할 탄 수
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float radian = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            GameObject bullet = Instantiate(radialBulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(direction);
            angle += angleStep;
        }
    }

    // 추적 탄막
    public void FireTrackingBullets()
    {
        StartCoroutine(FireTrackingBurst());
    }

    // 추적 탄막 코루틴
    private IEnumerator FireTrackingBurst()
    {
        for (int i = 0; i < trackingBulletCount; i++)
        {
            SpawnTrackingBullet();
            yield return new WaitForSeconds(trackingInterval); // 0.1초 간격으로 발사
        }
    }

    // 추적 탄막 생성하는 메서드
    private void SpawnTrackingBullet()
    {
        if (player == null) return; // 플레이어가 없으면 실행 X

        GameObject bullet = Instantiate(trackingBulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            rb.linearVelocity = direction * trackingBulletSpeed;
        }
    }

    // 보스 사망
    void Die()
    {
        if (isDead) return; // 이미 죽은 상태라면 리턴
        isDead = true;  // 보스 죽음 상태로 설정
        spriteRenderer.color = Color.white;
        animator.SetTrigger("Dead");  // 죽음 애니메이션 실행
        // 애니메이션이 끝날 때까지 대기
        StartCoroutine(DieAfterAnimation());
        Debug.Log("💀 보스 죽음");
    }


    // 애니메이션이 끝난 후 보스 제거
    private IEnumerator DieAfterAnimation()
    {        
        // 애니메이션이 끝날 때까지 대기
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;  // 매 프레임마다 애니메이션 진행 상태를 확인
        }
        // 죽은 후 보스 제거
        Destroy(gameObject);
    }
}
