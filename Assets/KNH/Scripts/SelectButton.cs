using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, IPointerEnterHandler
{
    private GameObject gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
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
        PlayerPrefs.SetInt("classNo", 0);
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
    public void WizardButton()//������ ����
    {
        PlayerPrefs.SetInt("classNo", 1);
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
    public void ArcherButton()//�ü� ����
    {
        PlayerPrefs.SetInt("classNo", 2);
        SceneManager.LoadScene("MainGame");//ĳ���� ���� �� �ε�
    }
    public void Finsh()//���� ����
    {
        SceneManager.LoadScene("TitleMenu");//Ÿ��Ʋ�� �ε�
    }
}
