using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerClass;//�÷��̾� ���� 0:���� 1:������ 2:�ü�
    [SerializeField] GameObject[] playerPrefabs;//�÷��̾� ������ �迭 

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

    private void Start()
    {
        Debug.Log("1");
        if (SceneManager.GetActiveScene().name == "MainGame")//MainGame������
        {
            Debug.Log("2");
            //�÷��̾� ������ ����
            Instantiate(playerPrefabs[playerClass], transform.position, Quaternion.identity);
        }
    }

}
