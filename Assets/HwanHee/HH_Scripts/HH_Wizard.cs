using Unity.VisualScripting;
using UnityEngine;

public class HH_Wizard : HH_Monster
{
    float fireDelayTime = 2f;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    protected float maxAttackRange = 5f;

    float fireTimer = 0f;
    bool canAttack = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (state == State.Death)
            return;

        if (!canAttack)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireDelayTime)
            {
                canAttack = true;
                fireTimer = 0;
            }
        }

        base.Update();
    }

    protected override void Idle()
    {
        if (distanceToPlayer <= attackRange)
        {
            state = State.Attack;
            anim.SetBool("Idle", false);
            anim.SetBool("Attack", true);
            return;
        }

        if (canAttack && distanceToPlayer > maxAttackRange)
        {
            state = State.Run;
            anim.SetBool("Idle", false);
            anim.SetBool("Run", true);
        }
    }

    protected override void Run()
    {
        if (canAttack && distanceToPlayer <= attackRange)
        {
            state = State.Attack;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", true);
        }
    }

    protected override void Attack()
    {
        if (distanceToPlayer > maxAttackRange)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Run", true);
            state = State.Run;
        }
    }

    protected override void TakeHit()
    {
        if (isTakeHitOver)
        {
            state = State.Idle;
            anim.SetBool("TakeHit", false);
            anim.SetBool("Idle", true);
            knockBackTimer = 0f;
        }
    }

    protected override void AttackPlayer()
    {
        CreateProjectile();
    }

    void CreateProjectile()
    {
        canAttack = false;
        dirToPlayer = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject _proj = Instantiate(projectile, transform.position + dirToPlayer, rotation);
        _proj.GetComponent<HH_WizardProjectile>().Attack = attack;
    }
}
