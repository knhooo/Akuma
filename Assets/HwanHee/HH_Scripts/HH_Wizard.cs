using UnityEngine;

public class HH_Wizard : HH_Monster
{
    [SerializeField]
    GameObject projectile;

    [SerializeField]
    protected float maxAttackRange = 5f;

    protected override void Run()
    {
        if (distanceToPlayer <= attackRange)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Attack", true);
            state = State.Attack;
        }
    }

    protected override void Attack()
    {
        if (!player)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Attack", false);
            return;
        }

        if (distanceToPlayer > maxAttackRange)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Run", true);
            state = State.Run;
        }
    }

    protected override void AttackPlayer()
    {
        CreateProjectile();
    }

    void CreateProjectile()
    {
        dirToPlayer = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject _proj = Instantiate(projectile, transform.position + dirToPlayer, rotation);
        _proj.GetComponent<HH_WizardProjectile>().Attack = attack;
    }
}
