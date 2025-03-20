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
        SceneManager.LoadScene("CharacterSelect");//캐릭터 선택 씬 로드
    }

    public void ExitButton()
    {
        Application.Quit();//게임 종료
    }

    public void WarriorButton()//전사 선택
    {
        gameManager.GetComponent<GameManager>(). SetClass(0);
        SceneManager.LoadScene("MainGame");//캐릭터 선택 씬 로드
    }
    public void WizardButton()//마법사 선택
    {
        gameManager.GetComponent<GameManager>().SetClass(1);
        SceneManager.LoadScene("MainGame");//캐릭터 선택 씬 로드
    }
    public void ArcherButton()//궁수 선택
    {
        gameManager.GetComponent<GameManager>().SetClass(2);
        SceneManager.LoadScene("MainGame");//캐릭터 선택 씬 로드
    }
}
