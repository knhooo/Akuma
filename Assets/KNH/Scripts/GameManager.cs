using Unity.Cinemachine;
using UnityEngine;
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
    AudioSource audioSource;
    [SerializeField]
    private AudioClip bossClip;
    float timer = 0f;

    [SerializeField] CinemachineCamera cinemachineCamera; // �ó׸ӽ� ī�޶�

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
            cinemachineCamera.Follow = player.transform;
            cinemachineCamera.LookAt = player.transform;

            //mainCamera.transform.SetParent(player.transform, false);
            GameObject spawnerInstance = Instantiate(spawner, player.transform);
            spawnerInstance.transform.SetParent(player.transform, true);
        }

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

        timer += Time.deltaTime;
        if (timer > 300f && audioSource.clip != bossClip)
        {
            audioSource.Stop();
            audioSource.clip = bossClip;
            audioSource.Play();
        }
    }
    public Vector3 GetPlayerPos()
    {
        return player.transform.position;
    }
}
