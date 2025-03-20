using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class HH_Mushroom : MonoBehaviour
{
    enum MSMState { Run, Attack, TakeHit, Death }
    enum Dir { left, right }

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private GameObject player;
    MSMState state = MSMState.Run;
    private Dir dir = Dir.right;
    float distanceToPlayer;
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
    }

    private void Update()
    {
        if (state == MSMState.Death)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (state)
        {
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
        spriteRenderer.flipX = player.transform.position.x < rigid.position.x;
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
    }

    private void Attack()
    {
        if (!player)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Attack", false);
            return;
        }

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
        if (!player)
        {
            anim.SetBool("TakeHit", false);
            anim.SetBool("Run", true);
            state = MSMState.Run;
            return;
        }

        if (hp <= 0)
        {
            anim.SetBool("TakeHit", false);
            anim.SetBool("Death", true);
            state = MSMState.Death;
        }

        if (isTakeHitOver)
        {
            state = MSMState.Run;
            anim.SetBool("TakeHit", false);
            anim.SetBool("Run", true);
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
