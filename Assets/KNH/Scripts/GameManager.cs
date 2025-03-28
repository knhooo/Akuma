using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerClass;//�÷��̾� ���� 0:���� 1:������ 2:�ü�
    [SerializeField] GameObject[] playerPrefabs;//�÷��̾� ������ �迭 
    public static GameManager instance;
    public GameObject player;
    public GameObject boss;
    [SerializeField] GameObject areaPrefab;
    GameObject areaObject;
    //public CinemachineCamera virtualCamera; // �ó׸ӽ� ���� ī�޶�
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject spawner;
    public PoolManager pool;
    public bool isClear;//���� Ŭ���� ����
    public bool isTimeStop = false;//�ð� ���� ����
    public bool isBoss = false;//���� ���� ����
    [Header("��� ����")]
    [Range(0, 10)] public float gameSpeed = 1f;

    public GameObject Player { get { return player; } }
    
    [SerializeField]
    AudioSource audio;
    [SerializeField]
    private AudioClip bossClip;
    Spawner _spawner;
    bool isBGMChange = false;

    //�÷��̾��� ������ �����ϴ� �޼���
    public void SetClass(int classNo)
    {
        playerClass = classNo;
    }
    //�÷��̾� ������ ��ȯ�ϴ� �޼���
    public int GetClass()
    {
        return playerClass;
    }

    private void Awake()
    {
        instance = this;//�ʱ�ȭ   
        areaObject = Instantiate(areaPrefab) as GameObject;
        playerClass = PlayerPrefs.GetInt("classNo");
        gameSpeed = 1f;
        if (SceneManager.GetActiveScene().name == "MainGame")//MainGame������
        {
            //�÷��̾� ������ ����
            player = Instantiate(playerPrefabs[playerClass], transform.position, Quaternion.identity);
        }
        if (player != null)
        {
            areaObject.transform.SetParent(player.transform, false);
            mainCamera.transform.SetParent(player.transform, false);
            GameObject spawnerInstance = Instantiate(spawner, player.transform);
            spawnerInstance.transform.SetParent(player.transform, true);
        }

        _spawner = spawner.GetComponent<Spawner>();
    }


    private void Update()
    {
        if (!isTimeStop)
        {
            Time.timeScale = gameSpeed;//��� ����
        }
        else
        {
            Time.timeScale = 0f;//�ð�����
        }

        if (_spawner.timer > 300f && !isBGMChange)
        {
            isBGMChange = true;
            audio.clip = bossClip;
            audio.Play();
        }
    }
    public Vector3 GetPlayerPos()
    {
        return player.transform.position;
    }
}
