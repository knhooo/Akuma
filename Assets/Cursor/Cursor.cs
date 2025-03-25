using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Sprite basicCursor;    // �⺻ Ŀ�� ��������Ʈ
    public Sprite monsterCursor;  // ���� �� Ŀ�� ��������Ʈ
    public Sprite itemCursor;     // ������ �� Ŀ�� ��������Ʈ

    private Texture2D basicTexture;
    private Texture2D monsterTexture;
    private Texture2D itemTexture;

    void Start()
    {
        // ��������Ʈ�� Texture2D�� ��ȯ
        basicTexture = SpriteToTexture(basicCursor);
        monsterTexture = SpriteToTexture(monsterCursor);
        itemTexture = SpriteToTexture(itemCursor);

        // �⺻ Ŀ���� ����
        Cursor.SetCursor(basicTexture, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        // ���콺 ��ġ���� ���� ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            // �±׿� ���� Ŀ�� ����
            if (hit.collider.CompareTag("Monster"))
            {
                Cursor.SetCursor(monsterTexture, Vector2.zero, CursorMode.Auto);
                return;  // Ŀ�� ���� �� ��������
            }

            if (hit.collider.CompareTag("Item"))
            {
                Cursor.SetCursor(itemTexture, Vector2.zero, CursorMode.Auto);
                return;  // Ŀ�� ���� �� ��������
            }
        }

        // �ƹ��͵� �������� ������ �⺻ Ŀ���� ����
        Cursor.SetCursor(basicTexture, Vector2.zero, CursorMode.Auto);
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

