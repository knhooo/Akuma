// ArcherAnim.cs
using UnityEngine;
using System.Collections;

public class ArcherAnim : Player
{
    private Animator animator;
    private Rigidbody2D rb;

    public GameObject hitEffectPrefab;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 10f;
    public float shootInterval = 1f;

    public AudioClip shootSound;
    public AudioClip skillSound;
    private AudioSource audioSource;

    private bool canDash = true;
    private bool canSkill = true;
    private bool isPerformingSkill = false;
    private bool isDashing = false;

    [SerializeField] private int skillAttackBoost = 20;
    [SerializeField] private float skillDuration = 1f;
    [SerializeField] private float skillSoundDelay = 0.3f;
    [SerializeField] private int levelUpExp = 10;
    [SerializeField] private GameObject laserHitBox;
    [SerializeField] private float dashSpeedMultiplier = 1.5f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float shootIntervalDecrease = 0.05f;
    [SerializeField] private float minShootInterval = 0.5f;
    [SerializeField] private float dashForce = 20f;

    private int skillBonus = 0;
    public int CurrentAttack => attack + skillBonus;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        dashCoolTimer = dashCoolTime;
        skillCoolTimer = skillCoolTime;
    }

    void Start()
    {
        rb.gravityScale = 0;
        StartCoroutine(AutoShoot());
        if (laserHitBox != null)
            laserHitBox.SetActive(false);
    }

    void Update()
    {
        HandleMovement();

        if (exp >= maxExp)
        {
            LevelUp();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isPerformingSkill)
        {
            if (!isDashClick)
                isDashClick = true;
            animator.SetTrigger("isDash");
            StartCoroutine(StartDashCooldown());
        }

        if (Input.GetMouseButtonDown(1) && canSkill && !isPerformingSkill)
        {
            if (!isSkillClick)
                isSkillClick = true;
            animator.SetTrigger("isLaser");
            StartCoroutine(StartSkillCooldown());
        }

        if (!canDash) dashCoolTimer += Time.deltaTime;
        if (!canSkill) skillCoolTimer += Time.deltaTime;
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2(moveX * speed, moveY * speed);
        animator.SetBool("isMoving", moveX != 0 || moveY != 0);

        if (moveX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);

        if (!GodMode)
            Hp -= dmg;

        animator.SetTrigger("isDamaged");

        if (Hp <= 0)
        {
            animator.SetTrigger("isDeath");
            StopAllCoroutines();
            this.enabled = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    IEnumerator AutoShoot()
    {
        while (true)
        {
            if (!isPerformingSkill)
            {
                Transform target = FindClosestMonster();
                if (target != null)
                {
                    ShootAtTarget(target.position);
                }
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }

    Transform FindClosestMonster()
    {

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");


        GameObject[] allEnemies = new GameObject[monsters.Length + bosses.Length];
        monsters.CopyTo(allEnemies, 0);
        bosses.CopyTo(allEnemies, monsters.Length);

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in allEnemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }


    void ShootAtTarget(Vector3 targetPos)
    {
        Vector2 direction = (targetPos - firePoint.position).normalized;

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        arrow.transform.localScale = Vector3.one;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * arrowSpeed;
        }

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.damage = CurrentAttack;
        }

        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    IEnumerator StartDashCooldown()
    {
        canDash = false;
        isPerformingSkill = true;
        dashCoolTimer = 0f;

        // ⏱ 대시 시작 딜레이 (예: 애니메이션 준비 시간)
        float dashStartDelay = 0.2f;
        yield return new WaitForSeconds(dashStartDelay);

        // 👉 바라보는 방향 기준으로 대시 방향 설정
        Vector2 dashDirection = rb.linearVelocity.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        float dashDistance = 2f;
        float dashSpeed = 10f;
        float moved = 0f;

        // 👉 충돌 비활성화
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;

        while (moved < dashDistance)
        {
            float moveStep = dashSpeed * Time.deltaTime;
            transform.Translate(dashDirection * moveStep);
            moved += moveStep;
            yield return null;
        }

        // 👉 잠깐 멈추는 딜레이 (선택 사항)
        float dashStopDelay = 0.3f;
        yield return new WaitForSeconds(dashStopDelay);

        // 👉 충돌 다시 활성화
        if (col != null)
            col.isTrigger = false;

        isPerformingSkill = false;

        while (dashCoolTimer < dashCoolTime)
            yield return null;

        canDash = true;
    }



    IEnumerator StartSkillCooldown()
    {
        canSkill = false;
        isPerformingSkill = true;
        skillCoolTimer = 0f;

        skillBonus = skillAttackBoost;

        yield return new WaitForSeconds(skillSoundDelay);

        if (skillSound != null && audioSource != null)
            audioSource.PlayOneShot(skillSound);

        if (laserHitBox != null)
        {
            laserHitBox.SetActive(false);
            laserHitBox.SetActive(true);

            LaserHitBox laser = laserHitBox.GetComponent<LaserHitBox>();
            if (laser != null)
                laser.SetDamage(CurrentAttack);
        }

        yield return new WaitForSeconds(skillDuration);

        if (laserHitBox != null)
            laserHitBox.SetActive(false);

        skillBonus = 0;
        isPerformingSkill = false;

        while (skillCoolTimer < skillCoolTime)
        {
            yield return null;
        }

        canSkill = true;
    }

    public override void GetExperience(int ex)
    {
        exp += ex;
        if (exp >= maxExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        exp = exp - maxExp;
        maxExp += levelUpExp;

        if (shootInterval > minShootInterval)
        {
            shootInterval = Mathf.Max(minShootInterval, shootInterval - shootIntervalDecrease);
        }
    }
}