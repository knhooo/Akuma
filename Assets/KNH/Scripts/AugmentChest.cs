using UnityEngine;

public class AugmentChest : MonoBehaviour
{
    private GameObject canvas;
    public void AugmentUI()
    {
        GameManager.instance.isTimeStop = true;//�ð� ����
        canvas = GameObject.Find("Canvas");
        canvas.GetComponent<UIManager>().AugmentChest();
        Destroy(gameObject, 0.8f);
    }
}
