 using UnityEngine;

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

    public int Hp { get { return hp; } set { hp = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Exp { get { return exp; } set { exp = value; } }
    public int MaxExp { get { return maxExp; } set { maxExp = value; } }
    public int Level { get { return level; } set { level = value; } }

    public virtual void TakeDamage(int dmg)
    {

    }

    public virtual void GetExperience(int ex)
    {

    }
}
