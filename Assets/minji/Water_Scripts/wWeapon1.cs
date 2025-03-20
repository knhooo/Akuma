using UnityEngine;

public class wWeapon1 : MonoBehaviour
{

    public int pwid;
    //public int pwprefabId;
    public float pwdamage;
    public int pwcount;
    public float pwspeed;
    public GameObject w1;

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    public void Init()
    {
        switch(pwid)
        {
            case 0:
                pwspeed = -150;
                Batch();
                break;
            default:
                break;
        }
    }

    void Batch()
    {
        for(int index = 0;index<pwcount;index++)
        {
            Transform xy = w1.gameObject.transform;
            w1.gameObject.GetComponent<wSkill1>().Init(pwdamage, -1);

        }
    }
}
