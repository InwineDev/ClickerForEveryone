using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class ModifiedAnimation : NetworkBehaviour
{
    [SerializeField] private Animator animator;             // Ссылка на компонент Animator
    [SerializeField] private AnimationClip[] animations;    // Массив анимаций
    [SyncVar(hook = nameof(OnModChanged))] private int mod; // Синхронизируемая переменная для хранения значения модификатора
    [SerializeField] private Button[] buttons;              // Массив кнопок для изменения значения mod

    private void Start()
    {
        if (isLocalPlayer)
        {
            AssignButtonListeners(); // Привязываем события к кнопкам только для локального игрока
        }
    }

    // Привязка обработчиков кнопок
    private void AssignButtonListeners()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Локальная переменная для сохранения индекса
            buttons[i].onClick.AddListener(() => CmdSetMod(index));
        }
    }

    // Команда для изменения модификатора на сервере
    [Command]
    private void CmdSetMod(int newMod)
    {
        if (IsValidMod(newMod))
        {
            mod = newMod; // Устанавливаем новое значение mod
        }
    }

    // Проверка допустимости значения модификатора
    private bool IsValidMod(int newMod) => newMod >= 0 && newMod < animations.Length;

    // Обработчик изменения значения mod
    private void OnModChanged(int oldMod, int newMod)
    {
        if (IsValidMod(newMod))
        {
            PlayAnimation(newMod);
        }
    }

    // Проигрывание анимации по индексу
    private void PlayAnimation(int animationIndex)
    {
        animator.Play(animations[animationIndex].name);
    }
}
