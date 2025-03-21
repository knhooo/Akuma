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
    [SerializeField] GameObject spawner;
    public PoolManager pool;

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
    }

}
