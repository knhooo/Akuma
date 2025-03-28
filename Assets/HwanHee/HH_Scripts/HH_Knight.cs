using System.Collections;
using UnityEditor;
using UnityEngine;

public class HH_Knight : Player
{
    [SerializeField]
    GameObject skill;
    [SerializeField]
    GameObject sword_right;
    [SerializeField]
    GameObject sword_left;
    [SerializeField]
    GameObject shield;
    [SerializeField]
    float defendTime = 3.0f;
    [SerializeField]
    float defendCoolTime = 5.0f;
    [SerializeField]
    float speedBoost = 2f;
    [SerializeField]
    GameObject blood;
    [SerializeField]
    int skillDamage = 30;
    [SerializeField]
    int levelUpExp = 10;
    [SerializeField]
    AudioSource attackSound;
    [SerializeField]
    AudioSource skillSound;

    enum Dir { left, right }
    Dir dir = Dir.right;

    enum KnightState { Attack, Skill, Defend, Roll, Death }
    KnightState state = KnightState.Attack;

    bool canUseDefend = true;
    bool GodMode = false;

    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    CircleCollider2D circleCol;

    Coroutine shieldCoroutine;
    Vector2 inputVec;

    float defendCoolTimer = 0f;
    float defendTimer = 0f;

    void Awake()
    {
        defendCoolTimer = defendCoolTime;
        skillCoolTimer = skillCoolTime;
        dashCoolTimer = dashCoolTime;

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        circleCol = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (state == KnightState.Death)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.H) && GodMode)
            GodMode = false;

        else if (Input.GetKeyDown(KeyCode.H) && !GodMode)
        {
            hp = maxHp;
            GodMode = true;
        }

        if (state != KnightState.Skill)
            skillCoolTimer += Time.deltaTime;
        if (state != KnightState.Defend)
            defendCoolTimer += Time.deltaTime;
        if (state != KnightState.Roll)
            dashCoolTimer += Time.deltaTime;

        switch (state)
        {
            case KnightState.Attack:
                StateAttack();
                break;
            case KnightState.Skill:
            case KnightState.Roll:
                stateFunction();
                break;
            case KnightState.Defend:
                Defend();
                break;
        }

        HandleKnightInput();
    }

    void FixedUpdate()
    {
        if (state == KnightState.Death || state == KnightState.Defend)
        {
            return;
        }

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (state == KnightState.Death)
        {
            return;
        }

        if (inputVec.x != 0)
        {
            dir = Dir.left;
            spriteRenderer.flipX = inputVec.x < 0;
        }

        if (!spriteRenderer.flipX)
        {
            dir = Dir.right;
        }
    }

    void StateAttack()
    {
        if (skillCoolTimer >= skillCoolTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!isSkillClick)
                    isSkillClick = true;

                canUseSkill = false;
                skillCoolTimer = 0f;
                skill.SetActive(true);
                skillSound.Play();

                state = KnightState.Skill;
                anim.SetBool("Attack", false);
                anim.SetBool("Skill", true);
                return;
            }
        }
        else if (skillCoolTimer < skillCoolTime && !canUseSkill)
            canUseSkill = true;

        if (defendCoolTimer >= defendCoolTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                canUseDefend = false;
                defendCoolTimer = 0f;

                if (shieldCoroutine != null)
                    StopCoroutine(shieldCoroutine);
                shieldCoroutine = StartCoroutine(SetShieldAlpha());
                shield.SetActive(true);
                SetAlpha(1f);

                state = KnightState.Defend;
                anim.SetBool("Attack", false);
                anim.SetBool("Defend", true);
                return;
            }
        }
        else if (defendCoolTimer < defendCoolTime && !canUseDefend)
            canUseDefend = true;


        if (dashCoolTimer >= dashCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!isDashClick)
                    isDashClick = true;

                canUseDash = false;
                dashCoolTimer = 0f;
                speed += speedBoost;

                circleCol.isTrigger = true;
                sword_left.SetActive(false);
                sword_right.SetActive(false);

                state = KnightState.Roll;
                anim.SetBool("Attack", false);
                anim.SetBool("Roll", true);
                return;
            }
        }
        else if (dashCoolTimer < dashCoolTime && !canUseDash)
            canUseDash = true;
    }

    void Defend()
    {
        defendTimer += Time.deltaTime;
        if (Input.GetMouseButtonUp(0) || defendTimer >= defendTime)
        {
            defendTimer = 0f;
            anim.speed = 1f;

            StopCoroutine(SetShieldAlpha());
            shield.SetActive(false);

            state = KnightState.Attack;
            anim.SetBool("Defend", false);
            anim.SetBool("Attack", true);
        }
    }

    void stateFunction()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f && !anim.IsInTransition(0))
        {
            state = KnightState.Attack;

            anim.SetBool("Defend", false);
            anim.SetBool("Skill", false);
            anim.SetBool("Roll", false);
            anim.SetBool("Attack", true);
        }
    }

    void HandleKnightInput()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        if (state != KnightState.Defend && anim.speed == 0)
        {
            anim.speed = 1f;
        }

        if (state == KnightState.Defend)
            return;
    }

    public override void TakeDamage(int dmg)
    {
        if (state == KnightState.Defend || state == KnightState.Death)
            return;

        if (!GodMode)
            hp -= dmg;

        if (hp <= 0)
        {
            hp = 0;
            state = KnightState.Death;
            anim.SetTrigger("Death");
            return;
        }

        StartCoroutine(TakeHitFlash());
        Instantiate(blood, transform.position, Quaternion.Euler(0, 0, 0));
    }

    protected IEnumerator TakeHitFlash()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", Color.red);
        spriteRenderer.SetPropertyBlock(mpb);

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", Color.white);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    IEnumerator SetShieldAlpha()
    {
        float t = 0f;
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;
            t = timer / defendTime;
            SetAlpha(Mathf.Lerp(1f, 0.2f, t));
            yield return null;
        }
    }

    void SetAlpha(float alpha)
    {
        Color _color = shield.GetComponent<SpriteRenderer>().color;
        _color.a = Mathf.Clamp01(alpha);
        shield.GetComponent<SpriteRenderer>().color = _color;
    }

    public override void GetExperience(int ex)
    {
        exp += ex;
        if (exp >= MaxExp)
            LevelUp();
    }

    public void LevelUp()
    {
        level++;
        exp -= MaxExp;
        maxExp += levelUpExp;
    }

    // 애니메이션 이벤트용 함수
    void ActivateSword()
    {
        if (dir == Dir.left)
        {
            sword_left.SetActive(true);
        }
        else
        {
            sword_right.SetActive(true);
        }
        attackSound.Play();
    }

    void InctivateSword()
    {
        sword_right.SetActive(false);
        sword_left.SetActive(false);
    }

    void RollOver()
    {
        speed -= speedBoost;
        circleCol.isTrigger = false;

        state = KnightState.Attack;
        anim.SetBool("Roll", false);
        anim.SetBool("Attack", true);
    }

    void AnimationStop() { anim.speed = 0f; }

    void SkillStart()
    {
        attack += skillDamage;
    }

    void SkillOver()
    {
        state = KnightState.Attack;
        anim.SetBool("Skill", false);
        anim.SetBool("Attack", true);
        skill.SetActive(false);
        attack -= skillDamage;
    }
}
