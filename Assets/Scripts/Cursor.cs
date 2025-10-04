using Mirror;
using UnityEngine;

public class CursorController : NetworkBehaviour
{
    public Sprite remoteCursorSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!isLocalPlayer)
        {
            spriteRenderer.sprite = remoteCursorSprite;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;
            transform.position = cursorPosition;
        }
    }
}
