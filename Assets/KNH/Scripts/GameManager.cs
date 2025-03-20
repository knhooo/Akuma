using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerClass;//�÷��̾� ���� 0:���� 1:������ 2:�ü�
    [SerializeField] GameObject[] playerPrefabs;//�÷��̾� ������ �迭 
    public static GameManager instance;
    public GameObject player;
    [SerializeField] GameObject areaPrefab;
    GameObject areaObject;
    //public CinemachineCamera virtualCamera; // �ó׸ӽ� ���� ī�޶�
    [SerializeField] Camera mainCamera;

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
    }

    

    private void Start()
    {
        areaObject = Instantiate(areaPrefab) as GameObject;
        
        if (SceneManager.GetActiveScene().name == "MainGame")//MainGame������
        {
            //�÷��̾� ������ ����
            player = Instantiate(playerPrefabs[playerClass], transform.position, Quaternion.identity);
        }
        if (player != null)
        {
            areaObject.transform.SetParent(player.transform, false);
            mainCamera.transform.SetParent(player.transform, false);
        }
    }

}
