using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite basicCursor;      // 기본 커서 스프라이트
    public Sprite gameCursor;       // 게임 커서 스프라이트
    public Sprite buttonCursor;     // 버튼 커서 스프라이트

    private Texture2D basicTexture;
    private Texture2D gameTexture;
    private Texture2D buttonTexture;

    private Texture2D currentCursor;
    private bool isOnButton = false; // 버튼 위에 있는지 확인하는 플래그

    void Start()
    {
        // 스프라이트를 Texture2D로 변환
        basicTexture = SpriteToTexture(basicCursor);
        gameTexture = SpriteToTexture(gameCursor);
        buttonTexture = SpriteToTexture(buttonCursor);

        // 기본 커서로 초기화
        SetCursor(basicTexture);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnButton = true;
        SetCursor(buttonTexture); // 버튼에 진입하면 버튼 커서로 변경
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnButton = false;
        UpdateCursor(); // 버튼에서 나가면 상황에 맞는 커서로 변경
    }

    void Update()
    {
        // 버튼 위에 있으면 다른 로직 무시
        if (isOnButton) return;

        // 씬과 게임 정지 여부에 따라 커서 변경
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        // MainGame 씬에서 게임이 진행 중이면 gameCursor 사용
        if (SceneManager.GetActiveScene().name == "MainGame" && Time.timeScale == 1f)
        {
            SetCursor(gameTexture);
        }
        else
        {
            SetCursor(basicTexture); // 그 외에는 기본 커서 사용
        }
    }

    private void SetCursor(Texture2D newCursor)
    {
        // 동일한 커서로의 중복 변경 방지
        if (currentCursor == newCursor) return;

        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.Auto);
        currentCursor = newCursor; // 현재 커서 업데이트
    }

    // 스프라이트를 Texture2D로 변환하는 유틸리티 함수
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
