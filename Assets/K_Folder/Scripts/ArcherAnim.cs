// ArcherAnim.cs
using UnityEngine;
using System.Collections;

public class ArcherAnim : Player
{
    private Animator animator;
    private Rigidbody2D rb;

    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 10f;
    public float shootInterval = 2f;

    public AudioClip shootSound;
    public AudioClip skillSound; // 스킬 사운드 추가
    private AudioSource audioSource;

    private bool canDash = true;
    private bool canSkill = true;
    private bool isPerformingSkill = false;

    [SerializeField] private int skillAttackBoost = 20;
    [SerializeField] private float skillDuration = 1f;
    [SerializeField] private float skillSoundDelay = 0.2f;
    [SerializeField] private int levelUpExp = 10;
    [SerializeField] private GameObject laserHitBox;

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
            animator.SetTrigger("isDash");
            StartCoroutine(StartDashCooldown());
        }

        if (Input.GetMouseButtonDown(1) && canSkill && !isPerformingSkill)
        {

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
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float dist = Vector2.Distance(transform.position, monster.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = monster.transform;
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

        yield return new WaitForSeconds(0.5f);
        isPerformingSkill = false;

        while (dashCoolTimer < dashCoolTime)
        {
            yield return null;
        }

        canDash = true;
    }

    IEnumerator StartSkillCooldown()
    {
        canSkill = false;
        isPerformingSkill = true;
        skillCoolTimer = 0f;

        int originalAttack = attack;
        attack += skillAttackBoost;

        yield return new WaitForSeconds(skillSoundDelay);

        if (skillSound != null && audioSource != null)
            audioSource.PlayOneShot(skillSound);

        // 👉 레이저 판정 활성화
        if (laserHitBox != null)
            laserHitBox.SetActive(true);

        yield return new WaitForSeconds(skillDuration);

        if (laserHitBox != null)
            laserHitBox.SetActive(false);

        attack = originalAttack;
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
        exp = maxExp - exp;
        maxExp += levelUpExp;
    }
}
