using UnityEngine;

public class HH_Monster : MonoBehaviour
{
    [SerializeField]
    protected int maxHp = 50;
    [SerializeField]
    protected int hp = 50;
    [SerializeField]
    protected int attack = 10;
    [SerializeField]
    protected float speed = 2f;
    [SerializeField]
    protected float attackRange = 3f;
    [SerializeField]
    protected int exp = 1;
    [SerializeField]
    GameObject item;
    [SerializeField]
    float dropRate = 10f;
    [SerializeField]
    GameObject logPrefab;

    protected enum State { Run, Attack, TakeHit, Death }
    protected State state = State.Run;

    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    protected CircleCollider2D circleCol;

    protected GameObject player;

    protected float distanceToPlayer;
    protected float knockBackSpeed = 1f;
    protected float knockBackDuration = 0.4f;
    protected float knockBackTimer = 0f;

    protected bool isTakeHitOver = true;

    protected Vector3 dirToPlayer;

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        circleCol = GetComponent<CircleCollider2D>();
    }

    protected void OnEnable()
    {
        hp = maxHp;

        state = State.Run;
        anim.SetBool("Run", true);
        anim.SetBool("Attack", false);
        anim.SetBool("TakeHit", false);
        anim.ResetTrigger("Death");
    }

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void Update()
    {
        if (state == State.Death)
            return;

        dirToPlayer = Vector3.Normalize(player.transform.position - transform.position);
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case State.Run:
                Run();
                break;
            case State.Attack:
                Attack();
                break;
            case State.TakeHit:
                TakeHit();
                break;
            default:
                break;
        }
    }

    protected void FixedUpdate()
    {
        if (state == State.Death)
        {
            rigid.linearVelocity = Vector3.zero;
            circleCol.isTrigger = false;
            return;
        }

        if (state == State.TakeHit)
        {
            knockBackTimer += Time.fixedDeltaTime;

            if (knockBackTimer <= knockBackDuration)
            {
                transform.Translate(-dirToPlayer * knockBackSpeed * Time.fixedDeltaTime);
            }
            else
            {
                knockBackTimer = 0f;
                state = State.Run;
                anim.SetBool("TakeHit", false);
                anim.SetBool("Run", true);
            }
        }

        else if (state == State.Run)
        {
            Vector2 nextVec = dirToPlayer * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.linearVelocity = Vector2.zero;
        }
        else
            rigid.linearVelocity = Vector2.zero;
    }

    protected void LateUpdate()
    {
        if (state == State.Death)
            return;
        spriteRenderer.flipX = player.transform.position.x < rigid.position.x;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Death)
            return;

        if (collision.CompareTag("PlayerAttack"))
        {
            isTakeHitOver = false;

            hp -= player.GetComponent<Player>().Attack;

            //로그 띄우기
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + 1, 0);
            GameObject log = Instantiate(logPrefab, vec, Quaternion.identity);
            log.transform.SetParent(gameObject.transform);
            log.GetComponent<LogText>().SetDmgLog(player.GetComponent<Player>().Attack);

            if (hp <= 0)
            {
                Death();
                return;
            }

            state = State.TakeHit;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", false);
            anim.SetBool("TakeHit", true);
        }
    }

    protected virtual void Run()
    {
        // 공격범위 들어올 경우
        if (distanceToPlayer <= attackRange)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Attack", true);
            state = State.Attack;
        }
    }

    protected virtual void Attack()
    {
        // 멀어졌을 경우
        if (distanceToPlayer > attackRange)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Run", true);
            state = State.Run;
        }
    }

    protected void TakeHit()
    {
        if (isTakeHitOver)
        {
            state = State.Run;
            anim.SetBool("TakeHit", false);
            anim.SetBool("Run", true);
            knockBackTimer = 0f;
        }
    }

    protected void Death()
    {
        anim.SetBool("TakeHit", false);
        anim.SetBool("Death", true);
        state = State.Death;

        player.GetComponent<Player>().GetExperience(exp);
        player.GetComponent<Player>().EnemyCount += 1;
    }

    // 애니메이션 이벤트용
    protected virtual void AttackPlayer()
    {
        if (distanceToPlayer <= attackRange)
        {
            Player _player = player.GetComponent<Player>();
            _player.TakeDamage(attack);
            //로그 띄우기
            Vector3 vec = new Vector3(_player.transform.position.x, _player.transform.position.y + 1, 0);
            GameObject log = Instantiate(logPrefab, vec, Quaternion.identity);
            log.transform.SetParent(_player.gameObject.transform);
            log.GetComponent<LogText>().SetPlayerDmgLog(attack);
        }
    }

    protected void DestroyMonster()
    {
        gameObject.SetActive(false);

        if (Random.Range(0, 100) < dropRate)
            Instantiate(item, transform.position, Quaternion.identity);
    }

    protected void SetTakeHitOver()
    {
        isTakeHitOver = true;
    }
}
