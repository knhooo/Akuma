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

    public void WarriorButton()//���� ����
    {
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
    public void WizardButton()//������ ����
    {
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
    public void ArcherButton()//�ü� ����
    {
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
}
