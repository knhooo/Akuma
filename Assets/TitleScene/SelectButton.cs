using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{

    public void StartButton()
    {
        SceneManager.LoadScene("CharacterSelect");//ĳ���� ���� �� �ε�
    }

    public void ExitButton()
    {
        Application.Quit();//���� ����
    }
}
