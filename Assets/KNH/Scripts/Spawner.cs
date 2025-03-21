using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;

    float cycle;//���� ���� �ֱ�
    float timer;
    int spawnIndex = 0;
    float frequency = 1f;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        Time.timeScale = 2f;//�׽�Ʈ�� ��� 
    }
    void Update()
    {
        cycle += Time.deltaTime;
        timer += Time.deltaTime;
        

        if (timer > 60 && timer < 120)
        {
            Debug.Log(timer);
            spawnIndex = 1;
            frequency = 0.8f;
        }
        else if (timer > 120 && timer <180)
        {
            Debug.Log(timer);
            spawnIndex = 2;
            frequency = 0.6f;
        }
        else if(timer > 180 && timer < 240)
        {
            
            //spawnIndex = 3;
            
        }
        else if(timer > 240 && timer < 300)
        {
            frequency = 0.2f;
        }
        else if(timer > 300)//����
        {
            //spawnIndex = 5;
        }
        
    }
    private void LateUpdate()
    {
        if (cycle > frequency)
        {
            Spawn(spawnIndex);//ù��° ����
            cycle = 0;
        }
    }

    void Spawn(int index)
    {
        GameObject enemy = GameManager.instance.pool.Get(index);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
    }
}
