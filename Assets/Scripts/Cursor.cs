using Mirror;
using UnityEngine;

public class CursorController : NetworkBehaviour
{
    public Sprite remoteCursorSprite; // Спрайт для остальных игроков

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Проверка на наличие SpriteRenderer
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден на объекте курсора!");
            return; // Выходим из метода, если компонента нет
        }

        // Если игрок не локальный, меняем спрайт на спрайт для других игроков
        if (!isLocalPlayer)
        {
            spriteRenderer.sprite = remoteCursorSprite;
        }
        else
        {
            // Для локального игрока делаем объект невидимым, чтобы использовать системный курсор
            spriteRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            // Управляем позицией курсора для локального игрока
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0; // Убираем смещение по Z, чтобы курсор был в плоскости камеры
            transform.position = cursorPosition;
        }
    }
}
