using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public Sprite basicCursor;    // �⺻ Ŀ�� ��������Ʈ
    public Sprite gameCursor;  // ���ΰ��� Ŀ�� ��������Ʈ
    //public Sprite itemCursor;     // ������ �� Ŀ�� ��������Ʈ
    public Sprite buttonCursor;     // ��ư Ŀ�� ��������Ʈ

    private Texture2D basicTexture;
    private Texture2D gameTexture;
    //private Texture2D itemTexture;
    private Texture2D buttonTexture;

    void Start()
    {
        // ��������Ʈ�� Texture2D�� ��ȯ
        basicTexture = SpriteToTexture(basicCursor);
        gameTexture = SpriteToTexture(gameCursor);
        //itemTexture = SpriteToTexture(itemCursor);
        buttonTexture = SpriteToTexture(buttonCursor);

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
            //if (hit.collider.CompareTag("Monster"))
            //{
            //    Cursor.SetCursor(monsterTexture, Vector2.zero, CursorMode.Auto);
            //    return;  // Ŀ�� ���� �� ��������
            //}

            //if (hit.collider.CompareTag("Item"))
            //{
            //    Cursor.SetCursor(itemTexture, Vector2.zero, CursorMode.Auto);
            //    return;  // Ŀ�� ���� �� ��������
            //}

            if (hit.collider.CompareTag("Button"))
            {
                Cursor.SetCursor(buttonTexture, Vector2.zero, CursorMode.Auto);
                return;  // Ŀ�� ���� �� ��������
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "MainGame") //���� ���� Ŀ��
            {
                Cursor.SetCursor(gameTexture, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                // �⺻ Ŀ��
                Cursor.SetCursor(basicTexture, Vector2.zero, CursorMode.Auto);
            }
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

