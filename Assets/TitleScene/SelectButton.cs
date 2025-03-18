using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{

    public void StartButton()
    {
        SceneManager.LoadScene("CharacterSelect");//캐릭터 선택 씬 로드
    }

    public void ExitButton()
    {
        Application.Quit();//게임 종료
    }
}
