using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class HH_Mushroom : MonoBehaviour
{
    enum MSMState { Idle, Run, Attack, TakeDmg, Death }
    enum Dir { left, right }
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private GameObject player;
    MSMState state = MSMState.Idle;
    private Dir dir = Dir.right;
    float distanceToPlayer;
    bool isLookAround = true;
    bool isAttackFinish = true;

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

        // 플레이어가 추적 범위 안에 있으면 추적 시작
        if (distanceToPlayer <= chaseRange && state != MSMState.Attack)
        {
            ChasePlayer();
        }

        // 공격하다가 멀어졌을 경우
        if (distanceToPlayer > attackRange && state == MSMState.Attack && isAttackFinish)
        {
            state = MSMState.Run;
            anim.SetBool("Run", true);
            anim.SetBool("Attack", false);

        }

        // 추적범위에서 멀어졌을 경우
        if (distanceToPlayer > chaseRange && state != MSMState.Idle)
        {
            isLookAround = true;
            state = MSMState.Idle;
            anim.SetBool("Idle", true);
            anim.SetBool("Run", false);
            anim.SetBool("Attack", false);
            anim.SetBool("TakeHit", false);
        }

        // 쫓아갈 때 방향 설정
        if (state != MSMState.Run)
            return;
        Vector3 forward = transform.forward;
        Vector3 toTarget = player.transform.position - transform.position;
        Vector3 crossProduct = Vector3.Cross(forward, toTarget);

        if (crossProduct.y > 0 && dir == Dir.left)
        {
            dir = Dir.right;
            spriteRenderer.flipX = false;
        }

        else if (crossProduct.y < 0 && dir == Dir.right)
        {
            dir = Dir.left;
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (state == MSMState.Death)
            return;

        if (state == MSMState.Run)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == MSMState.Death)
            return;

        if (collision.CompareTag("PlayerAttack"))
        {
            if (!player)
                return;
            TakeDamage(player.GetComponent<HH_Knight>().Attack);

        }
    }

    private void ChasePlayer()
    {
        isLookAround = false;
        state = MSMState.Run;
        anim.SetBool("Run", true);
        anim.SetBool("Idle", false);

        // 공격범위에 들어올 경우
        if (distanceToPlayer <= attackRange)
        {
            isAttackFinish = false;
            state = MSMState.Attack;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", true);
        }
    }

    private void Attack()
    {
        if (state == MSMState.Death)
            return;

        HH_Knight _player = player.GetComponent<HH_Knight>();
        _player.TakeDamage(attack);
    }

    private void AttackFinish()
    {
        if (state == MSMState.Death)
            return;

        isAttackFinish = true;
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


    public void TakeDamage(int dmg)
    {
        if (state == MSMState.Death)
            return;

        state = MSMState.TakeDmg;
        anim.SetBool("TakeHit", true);
        hp -= dmg;
        if (hp <= 0)
            DIE();
    }

    private void DIE()
    {
        state = MSMState.Death;
        anim.SetTrigger("Death");
    }

    private void DestroyMushroom()
    {
        Destroy(gameObject);
    }
}
