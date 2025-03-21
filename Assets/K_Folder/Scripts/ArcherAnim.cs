using UnityEngine;

public class ArcherAnim : Player
{
    private Animator animator;
    private Rigidbody2D rb;
    public bool allowVerticalMovement = true; // Whether vertical movement is allowed

    public GameObject arrowPrefab; // Arrow prefab
    public Transform firePoint; // The position where the arrow will spawn
    public float arrowSpeed = 10f; // Arrow speed

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void TakeDamage(int dmg)
    {
        if (hp <= 0) return;

        hp -= dmg;

        if (hp <= 0)
        {
            Die(); // Archer 클래스 내부에 정의된 Die() 호출
            return;
        }

        animator.SetTrigger("isDamaged");
    }

    void Die()
    {
        animator.SetTrigger("isDeath");
        this.enabled = false; // 스크립트 비활성화 (입력, 이동 등 차단)
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            TakeDamage(10); // 고정 데미지 10 (필요 시 조정 가능)
        }
    }

    // 또는 몬스터가 트리거 콜라이더일 경우
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            TakeDamage(10);
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Remove gravity (allow vertical movement without gravity)
    }

    void Update()
    {
        HandleMovement();
        HandleAttackInput();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = allowVerticalMovement ? Input.GetAxis("Vertical") : 0f; // Get vertical input if allowed

        rb.linearVelocity = new Vector2(moveX * speed, moveY * speed); // Handle X and Y movement

        animator.SetBool("isMoving", moveX != 0 || moveY != 0);

        if (moveX > 0)
            transform.localScale = new Vector3(1, 1, 1); // Flip character to face right
        else if (moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Flip character to face left
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click to attack (for special attack)
        {
            animator.SetTrigger("1Attack");
        }
        else if (Input.GetMouseButtonDown(0)) // Left-click to attack (shoot arrow)
        {
            animator.SetTrigger("2Attack");
            Invoke("ShootArrow", 0.4f); // Shoot arrow after 0.4 seconds delay
        }
    }

    // Function to shoot the arrow
    public void ShootArrow()
    {
        Debug.Log("ShootArrow function executed!"); // Log to check if function is called
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = transform.localScale.x; // Get character's facing direction (left or right)
                rb.linearVelocity = new Vector2(direction * arrowSpeed, 0); // Apply velocity to the arrow
            }
        }
    }
}
