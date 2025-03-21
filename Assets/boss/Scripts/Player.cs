using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected int hp = 100;
    [SerializeField]
    protected int attack = 10;
    [SerializeField]
    protected float speed = 3f;

    public int Hp { get { return hp; } set { hp = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public float Speed { get { return speed; } set { speed = value; } }

    public virtual void TakeDamage(int dmg)
    {

    }
}
