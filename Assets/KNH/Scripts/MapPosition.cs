using UnityEngine;

public class MapPosition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
            {
                case "Ground":
                    if (diffX > diffY)
                    {
                        transform.Translate(Vector3.right * dirX * 80);
                    }
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 80);
                    }
                else
                {
                    transform.Translate(Vector3.right * dirX * 80);
                    transform.Translate(Vector3.up * dirY * 80);
                }
                break;
            }
    }
}
