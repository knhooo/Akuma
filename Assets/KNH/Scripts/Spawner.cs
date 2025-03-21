using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;

    float cycle;//몬스터 스폰 주기
    float timer;
    int spawnIndex = 0;
    float frequency = 1f;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        Time.timeScale = 2f;//테스트용 배속 
    }
    void Update()
    {
        cycle += Time.deltaTime;
        timer += Time.deltaTime;
        

        if (timer > 60 && timer < 120)
        {
            spawnIndex = 1;
            frequency = 0.8f;
        }
        else if (timer > 120 && timer <180)
        {
            spawnIndex = 2;
            frequency = 0.6f;
        }
        else if(timer > 180 && timer < 240)
        { 
            spawnIndex = 3;
            frequency = 0.4f;

        }
        else if(timer > 240 && timer < 300)
        {
            spawnIndex = 4;
            frequency = 0.2f;
        }
        else if(timer > 300)//보스
        {
            //spawnIndex = 5;
        }
        
    }
    private void LateUpdate()
    {
        if (cycle > frequency)
        {
            Spawn(spawnIndex);//첫번째 몬스터
            cycle = 0;
        }
    }

    void Spawn(int index)
    {
        GameObject enemy = GameManager.instance.pool.Get(index);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
    }
}
