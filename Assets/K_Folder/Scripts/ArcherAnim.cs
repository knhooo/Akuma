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
    private AudioSource audioSource;

    public float dashCooldown = 2f;
    public float laserCooldown = 3f;

    private bool canDash = true;
    private bool canLaser = true;
    private bool isPerformingSkill = false; // 👉 스킬 사용 중 여부

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        rb.gravityScale = 0;
        StartCoroutine(AutoShoot());
    }

    void Update()
    {
        HandleMovement();

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isPerformingSkill)
        {
            animator.SetTrigger("isDash");
            StartCoroutine(DashRoutine());
        }

        // Laser
        if (Input.GetMouseButtonDown(1) && canLaser && !isPerformingSkill)
        {
            animator.SetTrigger("isLaser");
            StartCoroutine(LaserRoutine());
        }
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
            if (!isPerformingSkill) // 👉 스킬 중이 아닐 때만 공격
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

    IEnumerator DashRoutine()
    {
        canDash = false;
        isPerformingSkill = true;

        yield return new WaitForSeconds(0.5f); // 대시 애니메이션 길이
        isPerformingSkill = false;

        yield return new WaitForSeconds(dashCooldown - 0.5f);
        canDash = true;
    }

    IEnumerator LaserRoutine()
    {
        canLaser = false;
        isPerformingSkill = true;

        yield return new WaitForSeconds(1f); // 레이저 애니메이션 길이
        isPerformingSkill = false;

        yield return new WaitForSeconds(laserCooldown - 1f);
        canLaser = true;
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
}
