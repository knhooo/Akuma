using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    private GameObject gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
    }
    public void StartButton()
    {
        SceneManager.LoadScene("CharacterSelect");//ĳ���� ���� �� �ε�
    }

    public void ExitButton()
    {
        Application.Quit();//���� ����
    }

    public void WarriorButton()//���� ����
    {
        gameManager.GetComponent<GameManager>(). SetClass(0);
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
    public void WizardButton()//������ ����
    {
        gameManager.GetComponent<GameManager>().SetClass(1);
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
    public void ArcherButton()//�ü� ����
    {
        gameManager.GetComponent<GameManager>().SetClass(2);
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
}
