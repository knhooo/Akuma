using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite basicCursor;    // �⺻ Ŀ�� ��������Ʈ
    public Sprite gameCursor;     // ���ΰ��� Ŀ�� ��������Ʈ
    public Sprite buttonCursor;   // ��ư Ŀ�� ��������Ʈ

    private Texture2D basicTexture;
    private Texture2D gameTexture;
    private Texture2D buttonTexture;

    private Texture2D currentCursor; // ���� Ŀ�� ���� ����
    private bool isOnButton = false; // ��ư ���� �ִ��� Ȯ��

    void Start()
    {
        // ��������Ʈ�� Texture2D�� ��ȯ
        basicTexture = SpriteToTexture(basicCursor);
        gameTexture = SpriteToTexture(gameCursor);
        buttonTexture = SpriteToTexture(buttonCursor);

        // �⺻ Ŀ���� ����
        SetCursor(basicTexture);
    }

    // ��ư ���� ���콺�� �ö󰡸� ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnButton = true; // ��ư �� ���� ���
        SetCursor(buttonTexture);
    }

    // ��ư���� ���콺�� ����� ����
    public void OnPointerExit(PointerEventData eventData)
    {
        isOnButton = false; // ��ư���� ���
        UpdateCursor();
    }

    void Update()
    {
        // ��ư ���� ���� ���� �� Ŀ�� ���� ����
        if (isOnButton) return;

        // ���� ���� Ŀ�� ����
        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            SetCursor(gameTexture); // ���� ���� Ŀ���� ����
        }
        else
        {
            SetCursor(basicTexture); // �⺻ Ŀ���� ����
        }
    }

    // Ŀ�� ���� (�ߺ� ȣ�� ����)
    private void SetCursor(Texture2D newCursor)
    {
        if (currentCursor == newCursor) return; // ������ Ŀ���� ���� �� ��
        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.Auto);
        currentCursor = newCursor; // ���� Ŀ�� ������Ʈ
    }

    // ���� ��Ȳ�� �´� Ŀ���� ������Ʈ
    private void UpdateCursor()
    {
        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            SetCursor(gameTexture);
        }
        else
        {
            SetCursor(basicTexture);
        }
    }

    // ��������Ʈ�� Texture2D�� ��ȯ�ϴ� �Լ�
    Texture2D SpriteToTexture(Sprite sprite)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        texture.SetPixels(sprite.texture.GetPixels(
            (int)sprite.rect.x, (int)sprite.rect.y,
            (int)sprite.rect.width, (int)sprite.rect.height));
        texture.Apply();
        return texture;
    }
}


