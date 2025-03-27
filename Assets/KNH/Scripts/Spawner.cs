using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;

    float cycle;//몬스터 스폰 주기
    public float timer;
    int spawnIndex = 0;
    float frequency = 1f;
    int bossCount = 0;
    bool isSpawn = true;

    [SerializeField] GameObject boss;
    GameObject bossObject;
    [SerializeField] GameObject wall;
    [SerializeField] int wallCount = 40;
    GameObject[] wallArr;
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        wallArr = new GameObject[wallCount];
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
        else if (timer > 120 && timer < 180)
        {
            spawnIndex = 2;
            frequency = 0.6f;
        }
        else if (timer > 180 && timer < 240)
        {
            spawnIndex = 3;
            frequency = 0.4f;

        }
        else if (timer > 240 && timer < 300)
        {
            spawnIndex = Random.Range(3, 5);
            frequency = 1f;
        }
        else if (timer > 300)//보스
        {
            isSpawn = false;
            if (bossCount < 1)
            {
                bossObject = Instantiate(boss, GameManager.instance.GetPlayerPos()+new Vector3(2f,2f,0), Quaternion.identity);
                GameManager.instance.boss = bossObject;
                GameManager.instance.isBoss = true;
                SpawnWall(bossObject.transform.position);//보스 주변에 벽 생성
                bossCount++;
            }
            else if(bossCount == 1)
            {
                //보스 체력 30%이하되면 벽 삭제
                if (boss.GetComponent<BossAI>().currentHP <= boss.GetComponent<BossAI>().maxHP * 0.3f)
                {
                    DestroyWall();
                }
            }
            else if (bossCount == 1 && bossObject == null)//보스 사망시
            {
                GameManager.instance.isBoss = false;
                GameManager.instance.isClear = true;
            }
        }
    }

    private void LateUpdate()
    {
        if(isSpawn == true)
        {
            if (cycle > frequency)
            {
                Spawn(spawnIndex);//첫번째 몬스터
                cycle = 0;
            }
        }
    }

    void Spawn(int index)
    {
        GameObject enemy = GameManager.instance.pool.Get(index);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }

     void SpawnWall(Vector3 center)
    {
        for(int i=0; i< wallCount; i++)
        {
            float angle =  i * 360f / wallCount; 
            float radian = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
            float x = center.x + Mathf.Cos(radian) * 20;
            float y = center.y + Mathf.Sin(radian) * 20;

            wallArr[i] = Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity);
        }
    }

    public void DestroyWall()
    {
        for(int i=0; i< wallCount; i++)
        {
            Destroy(wallArr[i], 0.5f);
        }
    }
}
