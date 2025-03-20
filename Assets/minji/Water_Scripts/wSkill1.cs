using UnityEngine;

public class wSkill1 : MonoBehaviour
{
    public float damage;
    public int per;

    public void Init(float damage,int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
