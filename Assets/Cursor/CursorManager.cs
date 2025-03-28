using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite basicCursor;      // �⺻ Ŀ�� ��������Ʈ
    public Sprite gameCursor;       // ���� Ŀ�� ��������Ʈ
    public Sprite buttonCursor;     // ��ư Ŀ�� ��������Ʈ

    private Texture2D basicTexture;
    private Texture2D gameTexture;
    private Texture2D buttonTexture;

    private Texture2D currentCursor;
    private bool isOnButton = false; // ��ư ���� �ִ��� Ȯ���ϴ� �÷���

    void Start()
    {
        // ��������Ʈ�� Texture2D�� ��ȯ
        basicTexture = SpriteToTexture(basicCursor);
        gameTexture = SpriteToTexture(gameCursor);
        buttonTexture = SpriteToTexture(buttonCursor);

        // �⺻ Ŀ���� �ʱ�ȭ
        SetCursor(basicTexture);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnButton = true;
        SetCursor(buttonTexture); // ��ư�� �����ϸ� ��ư Ŀ���� ����
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnButton = false;
        UpdateCursor(); // ��ư���� ������ ��Ȳ�� �´� Ŀ���� ����
    }

    void Update()
    {
        // ��ư ���� ������ �ٸ� ���� ����
        if (isOnButton) return;

        // ���� ���� ���� ���ο� ���� Ŀ�� ����
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        // MainGame ������ ������ ���� ���̸� gameCursor ���
        if (SceneManager.GetActiveScene().name == "MainGame" && Time.timeScale == 1f)
        {
            SetCursor(gameTexture);
        }
        else
        {
            SetCursor(basicTexture); // �� �ܿ��� �⺻ Ŀ�� ���
        }
    }

    private void SetCursor(Texture2D newCursor)
    {
        // ������ Ŀ������ �ߺ� ���� ����
        if (currentCursor == newCursor) return;

        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.Auto);
        currentCursor = newCursor; // ���� Ŀ�� ������Ʈ
    }

    // ��������Ʈ�� Texture2D�� ��ȯ�ϴ� ��ƿ��Ƽ �Լ�
    private Texture2D SpriteToTexture(Sprite sprite)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y,
                                                  (int)sprite.rect.width, (int)sprite.rect.height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}
