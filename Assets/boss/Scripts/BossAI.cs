using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isHurt = false;
    private bool isDead = false;
    private bool isMoving = true;
    private bool isEnraged = false;
    private bool isCasting = false;

    [Header("추적 및 근접공격")]
    public Transform player;
    public Transform playerAttack;
    private float followRange = float.MaxValue;
    public float attackRange = 5f;
    public float attackDamage = 10f;
    private float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    [Header("이동")]
    public float moveSpeed = 1.5f;
    private const float StoppingDistance = 0.01f;

    [Header("체력")]
    public float maxHP = 5000f;
    public float currentHP = 5000f;

    [Header("주문 공격")]
    public GameObject redDotPrefab;
    public GameObject bossSpellPrefab;
    public Transform spellSpawnPoint;
    public int spellCount = 12;
    public float spellRangeX = 3f;
    public float spellRangeY = 2f;
    private float castCooldown = 8f;
    private float nextCastTime = 2f;

    [Header("[강화 패턴]순간이동")]
    private const float TeleportCooldown = 3f;
    private float teleportTimer = 0f;

    [Header("탄막")]
    public GameObject radialBulletPrefab;
    public GameObject trackingBulletPrefab;
    public Transform firePoint;
    public int bulletCount = 8;
    public float bulletSpeed = 3f;
    public float bulletFireRate = 3f;
    public int trackingBulletCount = 5;
    public float trackingBulletSpeed = 7f;
    public float trackingInterval = 0.5f;
    private const float TrCooldown = 6f;
    private float nextTrTime = 6f;
    private bool isTrFiring = false;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        currentHP = maxHP;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(FireRadialBullets());
    }

    void Update()
    {
        if (isDead) return;

        if (currentHP <= 0 && !isDead)
        {
            currentHP = 0;
            Die();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        HandleAttack(distanceToPlayer);
        HandleCasting();
        HandleTrackingBullets();
        HandleMovement(distanceToPlayer);
        HandleEnragedMode();
        HandleSpriteFlip();

        if (Input.GetKeyDown(KeyCode.C)) CastSpell();
        if (Input.GetKeyDown(KeyCode.Space)) TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.X)) Die();
    }

    private void HandleAttack(float distanceToPlayer)
    {
        attackCooldown = currentHP <= maxHP * 0.6f ? 1.5f : 2f;

        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void HandleCasting()
    {
        if (Time.time >= nextCastTime && !isCasting)
        {
            CastSpell();
            nextCastTime = Time.time + castCooldown;
        }
    }

    private void HandleTrackingBullets()
    {
        if (Time.time >= nextTrTime && !isTrFiring)
        {
            FireTrackingBullets();
            nextTrTime = Time.time + TrCooldown;
        }
    }

    private void HandleMovement(float distanceToPlayer)
    {
        if (!isHurt)
        {
            if (distanceToPlayer <= followRange && distanceToPlayer > StoppingDistance)
            {
                animator.SetBool("isWalking", true);
                MoveTowardsPlayer();
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            animator.SetBool("isAttacking", distanceToPlayer <= attackRange);
        }
    }

    private void HandleEnragedMode()
    {
        if (currentHP <= maxHP * 0.3f && !isEnraged)
        {
            EnterEnragedMode();
        }
    }

    private void HandleSpriteFlip()
    {
        spriteRenderer.flipX = player.position.x > transform.position.x;
    }

    void MoveTowardsPlayer()
    {
        if (!isMoving) return;
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        Debug.Log($"Damage 실행됨. 데미지 값: {damage}");
        currentHP -= damage;
        Debug.Log("보스 체력: " + currentHP);
        if (currentHP <= 0)
        {
            Die();
            return;
        }
        StartCoroutine(PlayHurtAnimation());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            TakeDamage(player.GetComponent<Player>().Attack);
        }
    }

    private void AttackPlayer()
    {
        player.GetComponent<Player>().TakeDamage(Mathf.RoundToInt(attackDamage));
        Debug.Log("보스가 플레이어를 공격했습니다! 데미지: " + attackDamage);
    }

    private void EnterEnragedMode()
    {
        isEnraged = true;
        Debug.Log("⚡ 강화 패턴 발동!");
        moveSpeed *= 3f;
        animator.speed = 1.5f;
        attackCooldown *= 0.5f;
        castCooldown *= 0.5f;
        spriteRenderer.color = Color.red;
        StartCoroutine(TeleportCoroutine());
    }

    void CastSpell()
    {
        Debug.Log("Cast 실행됨");
        StartCoroutine(PlayCastAnimation());
    }

    void SpawnSpell()
    {
        Debug.Log("Spell 실행됨");
        for (int i = 0; i < spellCount; i++)
        {
            Vector3 randomPosition = GetRandomSpellPositionInCameraView();
            GameObject redDot = Instantiate(redDotPrefab, randomPosition, Quaternion.identity);
            StartCoroutine(HandleRedDotAndSpell(redDot));
        }
    }

    private Vector3 GetRandomSpellPositionInCameraView()
    {
        float screenMinX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;
        float screenMaxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)).x;
        float screenMinY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).y;
        float screenMaxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane)).y;

        float randomX = Random.Range(screenMinX, screenMaxX);
        float randomY = Random.Range(screenMinY, screenMaxY);

        return new Vector3(randomX, randomY, 0f);
    }

    private IEnumerator PlayHurtAnimation()
    {
        isHurt = true;
        isMoving = false;
        animator.ResetTrigger("HurtTrigger");
        animator.SetTrigger("HurtTrigger");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isMoving = true;
        isHurt = false;
    }

    private IEnumerator PlayCastAnimation()
    {
        isMoving = false;
        isCasting = true;
        animator.SetTrigger("Cast");

        float castDuration = 0.5f / animator.speed;
        yield return new WaitForSeconds(castDuration);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isCasting = false;
        isMoving = true;

        SpawnSpell();
    }

    IEnumerator HandleRedDotAndSpell(GameObject redDot)
    {
        yield return new WaitForSeconds(2f);

        Vector3 spawnPosition = redDot.transform.position;
        Destroy(redDot);

        GameObject bossSpell = Instantiate(bossSpellPrefab, spawnPosition, Quaternion.identity);
        BossSpell bossSpellScript = bossSpell.GetComponent<BossSpell>();
        if (bossSpellScript != null)
        {
            bossSpellScript.explosionPoint = bossSpell.transform;
        }
    }

    private IEnumerator TeleportCoroutine()
    {
        while (isEnraged)
        {
            teleportTimer -= Time.deltaTime;
            if (teleportTimer <= 0f)
            {
                Teleport();
                teleportTimer = TeleportCooldown;
            }
            yield return null;
        }
    }

    private void Teleport()
    {
        Vector3 teleportPosition = GetRandomTeleportPosition();
        transform.position = teleportPosition;
        StartCoroutine(AfterTeleport());
    }

    private IEnumerator AfterTeleport()
    {
        float prevspd = moveSpeed;
        moveSpeed = 6f;
        Debug.Log("⚡ 이동 속도 증가: " + moveSpeed);
        yield return new WaitForSeconds(0.8f);
        moveSpeed = prevspd;
        Debug.Log("🛑 원래 속도로 복귀: " + moveSpeed);
    }

    private Vector3 GetRandomTeleportPosition()
    {
        float offsetX = Random.Range(-5f, 5f);
        float offsetY = Random.Range(-5f, 5f);

        return player.position + new Vector3(offsetX, offsetY, 0f);
    }

    private IEnumerator FireRadialBullets()
    {
        while (true)
        {
            FireRadialPattern();
            yield return new WaitForSeconds(bulletFireRate);
        }
    }

    void FireRadialPattern()
    {
        int bulletCount = 12;
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

    public void FireTrackingBullets()
    {
        StartCoroutine(FireTrackingBurst());
    }

    private IEnumerator FireTrackingBurst()
    {
        for (int i = 0; i < trackingBulletCount; i++)
        {
            SpawnTrackingBullet();
            yield return new WaitForSeconds(trackingInterval);
        }
    }

    private void SpawnTrackingBullet()
    {
        if (player == null) return;

        GameObject bullet = Instantiate(trackingBulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            rb.linearVelocity = direction * trackingBulletSpeed;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        spriteRenderer.color = Color.white;
        animator.SetTrigger("Dead");
        StartCoroutine(DieAfterAnimation());
        Debug.Log("💀 보스 죽음");
    }

    private IEnumerator DieAfterAnimation()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        RemoveAllRedDots();
        Destroy(gameObject);
    }

    private void RemoveAllRedDots()
    {
        GameObject[] redDots = GameObject.FindGameObjectsWithTag("RedDot");
        foreach (GameObject redDot in redDots)
        {
            Destroy(redDot);
        }
    }

    public void GameClear()
    {
        GameObject canvas = GameObject.Find("Canvas");
        canvas.GetComponent<UIManager>().GameClear();
    }
}