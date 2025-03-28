using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected int hp = 100;
    [SerializeField]
    protected int maxHp = 100;
    [SerializeField]
    protected int level = 1;
    [SerializeField]
    protected int exp = 0;
    [SerializeField]
    protected int maxExp = 100;
    [SerializeField]
    protected int attack = 10;
    [SerializeField]
    protected float speed = 3f;
    [SerializeField]
    protected float skillCoolTime = 5.0f;
    [SerializeField]
    protected float dashCoolTime = 5.0f;
    [SerializeField]
    protected int enemyCount = 0;
    
    protected float dashCoolTimer = 0f;
    protected float skillCoolTimer = 0f;

    protected bool canUseDash = true;
    protected bool canUseSkill = true;

    public bool isSkillClick = false;
    public bool isDashClick = false;

    public int Hp { get { return hp; } set { hp = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Exp { get { return exp; } set { exp = value; } }
    public int MaxExp { get { return maxExp; } set { maxExp = value; } }
    public int Level { get { return level; } set { level = value; } }
    public float SkillCoolTime { get { return skillCoolTime; } }
    public float SkillCoolTimer { get { return skillCoolTimer; } }
    public float DashCoolTime { get { return dashCoolTime; } }
    public float DashCoolTimer { get { return dashCoolTimer; } }
    public int EnemyCount { get { return enemyCount; } set { enemyCount = value; } }

    public bool CanUseSkill { get { return canUseSkill; } }
    public bool CanUseDash { get { return canUseDash; } }

    [SerializeField]
    protected GameObject logPrefab;

    protected bool GodMode = false;

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && GodMode)
            GodMode = false;

        else if (Input.GetKeyDown(KeyCode.H) && !GodMode)
        {
            hp = maxHp;
            GodMode = true;
        }
    }

    public virtual void TakeDamage(int dmg)
    {
        //·Î±× ¶ç¿ì±â
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + 1, 0);
        GameObject log = Instantiate(logPrefab, vec, Quaternion.identity);
        log.transform.SetParent(transform);
        log.GetComponent<LogText>().SetPlayerDmgLog(dmg);
    }

    public virtual void GetExperience(int ex)
    {

    }
}
