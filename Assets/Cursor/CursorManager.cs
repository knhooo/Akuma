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

    private Texture2D currentCursor; // ���� Ŀ�� ����
    private bool isOnButton = false; // ��ư �� Ȯ��

    void Start()
    {
        // ��������Ʈ�� Texture2D�� ��ȯ
        basicTexture = SpriteToTexture(basicCursor);
        gameTexture = SpriteToTexture(gameCursor);
        buttonTexture = SpriteToTexture(buttonCursor);

        // �⺻ Ŀ���� ����
        SetCursor(basicTexture);
    }

    // ��ư ���� ���콺�� �ö��� ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnButton = true;
        SetCursor(buttonTexture);
    }

    // ��ư���� ���콺�� ����� ��
    public void OnPointerExit(PointerEventData eventData)
    {
        isOnButton = false;
        UpdateCursor();
    }

    void Update()
    {
        // ��ư ���� ���� ���� �� Ŀ�� ���� ����
        if (isOnButton) return;

        // ���� ���� Ŀ�� ����
        if (SceneManager.GetActiveScene().name == "MainGame"&& GameManager.instance.isTimeStop == false)
        {
            SetCursor(gameTexture);
        }
        else if(GameManager.instance.isTimeStop == true|| SceneManager.GetActiveScene().name != "MainGame")
        {
            SetCursor(basicTexture);
        }
    }

    private void SetCursor(Texture2D newCursor)
    {
        if (currentCursor == newCursor) return; // �ߺ� ȣ�� ����
        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.Auto);
        currentCursor = newCursor;
    }

    private void UpdateCursor()
    {
        if (SceneManager.GetActiveScene().name == "MainGame"&& GameManager.instance.isTimeStop == false)
        {
            SetCursor(gameTexture);
        }
        else if (GameManager.instance.isTimeStop == true || SceneManager.GetActiveScene().name != "MainGame")
        {
            SetCursor(basicTexture);
        }
    }

    // ��������Ʈ �� Texture2D ��ȯ
    Texture2D SpriteToTexture(Sprite sprite)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y,
                                                  (int)sprite.rect.width, (int)sprite.rect.height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}
