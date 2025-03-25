using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Sprite basicCursor;    // 기본 커서 스프라이트
    public Sprite monsterCursor;  // 몬스터 위 커서 스프라이트
    public Sprite itemCursor;     // 아이템 위 커서 스프라이트

    private Texture2D basicTexture;
    private Texture2D monsterTexture;
    private Texture2D itemTexture;

    void Start()
    {
        // 스프라이트를 Texture2D로 변환
        basicTexture = SpriteToTexture(basicCursor);
        monsterTexture = SpriteToTexture(monsterCursor);
        itemTexture = SpriteToTexture(itemCursor);

        // 기본 커서로 설정
        Cursor.SetCursor(basicTexture, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        // 마우스 위치에서 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            // 태그에 따라 커서 변경
            if (hit.collider.CompareTag("Monster"))
            {
                Cursor.SetCursor(monsterTexture, Vector2.zero, CursorMode.Auto);
                return;  // 커서 변경 후 빠져나감
            }

            if (hit.collider.CompareTag("Item"))
            {
                Cursor.SetCursor(itemTexture, Vector2.zero, CursorMode.Auto);
                return;  // 커서 변경 후 빠져나감
            }
        }

        // 아무것도 감지되지 않으면 기본 커서로 복귀
        Cursor.SetCursor(basicTexture, Vector2.zero, CursorMode.Auto);
    }

    // 스프라이트를 Texture2D로 변환하는 함수
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

