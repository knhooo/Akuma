using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite basicCursor;    // 기본 커서 스프라이트
    public Sprite gameCursor;     // 메인게임 커서 스프라이트
    public Sprite buttonCursor;   // 버튼 커서 스프라이트

    private Texture2D basicTexture;
    private Texture2D gameTexture;
    private Texture2D buttonTexture;

    private Texture2D currentCursor; // 현재 커서 추적
    private bool isOnButton = false; // 버튼 위 확인

    void Start()
    {
        // 스프라이트를 Texture2D로 변환
        basicTexture = SpriteToTexture(basicCursor);
        gameTexture = SpriteToTexture(gameCursor);
        buttonTexture = SpriteToTexture(buttonCursor);

        // 기본 커서로 설정
        SetCursor(basicTexture);
    }

    // 버튼 위로 마우스가 올라갔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnButton = true;
        SetCursor(buttonTexture);
    }

    // 버튼에서 마우스가 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        isOnButton = false;
        UpdateCursor();
    }

    void Update()
    {
        // 버튼 위에 있을 때는 씬 커서 변경 중지
        if (isOnButton) return;

        // 씬에 따라 커서 변경
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
        if (currentCursor == newCursor) return; // 중복 호출 방지
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

    // 스프라이트 → Texture2D 변환
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
