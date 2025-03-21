using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerClass;//플레이어 직업 0:전사 1:마법사 2:궁수
    [SerializeField] GameObject[] playerPrefabs;//플레이어 프리팹 배열 
    public static GameManager instance;
    public GameObject player;
    [SerializeField] GameObject areaPrefab;
    GameObject areaObject;
    //public CinemachineCamera virtualCamera; // 시네머신 가상 카메라
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject spawner;
    public PoolManager pool;

    //플레이어의 직업을 설정하는 메서드
    public void SetClass(int classNo)
    {
        playerClass = classNo;
    }
    //플레이어 직업을 반환하는 메서드
    public int GetClass()
    {
        return playerClass;
    }


    

    private void Awake()
    {
        instance = this;//초기화   
        areaObject = Instantiate(areaPrefab) as GameObject;
        playerClass = PlayerPrefs.GetInt("classNo");

        if (SceneManager.GetActiveScene().name == "MainGame")//MainGame씬에서
        {
            //플레이어 프리팹 생성
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
