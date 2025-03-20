using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class HH_Mushroom : MonoBehaviour
{
    enum MSMState { Idle, Run, Attack, TakeHit, Death }
    enum Dir { left, right }

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private GameObject player;
    MSMState state = MSMState.Idle;
    private Dir dir = Dir.right;
    float distanceToPlayer;
    bool isLookAround = true;
    bool isAttackOver = true;
    bool isTakeHitOver = true;

    [SerializeField]
    int hp = 50;
    [SerializeField]
    int attack = 10;
    [SerializeField]
    float speed = 2;
    [SerializeField]
    float chaseRange = 10f;
    [SerializeField]
    float attackRange = 2f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(LookAround());
    }

    private void Update()
    {
        if (state == MSMState.Death)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case MSMState.Idle:
                Idle();
                break;
            case MSMState.Run:
                Run();
                break;
            case MSMState.Attack:
                Attack();
                break;
            case MSMState.TakeHit:
                TakeHit();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (state != MSMState.Run)
            return;
        Rigidbody2D _player = player.GetComponent<Rigidbody2D>();

        Vector2 dirVec = _player.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (state == MSMState.Run || state == MSMState.Attack)
            spriteRenderer.flipX = player.transform.position.x < rigid.position.x;
    }

    private void Idle()
    {
        if (distanceToPlayer <= attackRange)
        {
            isAttackOver = false;
            anim.SetBool("Idle", false);
            anim.SetBool("Attack", true);
            state = MSMState.Attack;
        }

        else if (distanceToPlayer <= chaseRange)
        {
            isLookAround = false;
            anim.SetBool("Idle", false);
            anim.SetBool("Run", true);
            state = MSMState.Run;
        }
    }

    private void Run()
    {
        // 공격범위 들어올 경우
        if (distanceToPlayer <= attackRange)
        {
            isAttackOver = false;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", true);
            state = MSMState.Attack;
        }

        // 멀어졌을 경우
        if (distanceToPlayer > chaseRange)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Idle", true);
            state = MSMState.Idle;
            isLookAround = true;
        }
    }

    private void Attack()
    {
        // 멀어졌을 경우
        if (distanceToPlayer > attackRange && isAttackOver)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Run", true);
            state = MSMState.Run;
        }
    }

    private void TakeHit()
    {
        if (hp <= 0)
        {
            anim.SetBool("TakeHit", false);
            anim.SetBool("Death", true);
            state = MSMState.Death;
        }

        if (isTakeHitOver)
        {
            state = MSMState.Idle;
            anim.SetBool("TakeHit", false);
            anim.SetBool("Idle", true);
            isLookAround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == MSMState.Death)
            return;

        if (collision.CompareTag("PlayerAttack"))
        {
            isAttackOver = true;
            isTakeHitOver = false;
            hp -= player.GetComponent<HH_Knight>().Attack;
            state = MSMState.TakeHit;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", false);
            anim.SetBool("TakeHit", true);
        }
    }

    IEnumerator LookAround()
    {
        while (true)
        {
            while (!isLookAround)
            {
                yield return null;  // 한 프레임을 기다리고 다시 체크
            }

            yield return new WaitForSeconds(2f);
            if (dir == Dir.right)
            {
                spriteRenderer.flipX = true;
                dir = Dir.left;
            }
            else
            {
                spriteRenderer.flipX = false;
                dir = Dir.right;
            }
        }
    }

    private void AttackPlayer()
    {
        if (distanceToPlayer <= attackRange)
        {
            HH_Knight _player = player.GetComponent<HH_Knight>();
            _player.TakeDamage(attack);
        }
    }

    private void DestroyMushroom()
    {
        Destroy(gameObject);
    }

    private void SetAttcakOver()
    {
        isAttackOver = true;
    }

    private void SetTakeHitOver()
    {
        isTakeHitOver = true;
    }
}
