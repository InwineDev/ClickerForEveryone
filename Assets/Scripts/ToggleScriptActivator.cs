using UnityEngine;
using UnityEngine.UI;

public class ToggleScriptActivator : MonoBehaviour
{
    // Массив для хранения Toggles
    public Toggle[] toggles;

    // Массив для хранения индексов Toggles, которые будут активировать соответствующие скрипты
    public int[] toggleIndices;

    // Массив для хранения скриптов, которые нужно активировать/деактивировать
    public MonoBehaviour[] scripts;

    private void Start()
    {
        // Убедимся, что массивы имеют одинаковую длину
        if (toggleIndices.Length != scripts.Length)
        {
            Debug.LogError("Длина массивов toggleIndices и scripts должна быть одинаковой!");
            return;
        }

        // Подписываемся на событие изменения состояния каждого Toggle
        for (int i = 0; i < toggleIndices.Length; i++)
        {
            int index = i; // Локальная переменная для захвата индекса в замыкании
            if (toggleIndices[index] < toggles.Length)
            {
                toggles[toggleIndices[index]].onValueChanged.AddListener(delegate { ToggleChanged(index); });
                ToggleChanged(index); // Устанавливаем начальное состояние скриптов
            }
            else
            {
                Debug.LogError($"Индекс {toggleIndices[index]} выходит за пределы массива toggles!");
            }
        }
    }

    // Метод, вызываемый при изменении состояния Toggle
    private void ToggleChanged(int index)
    {
        if (index < scripts.Length)
        {
            scripts[index].enabled = toggles[toggleIndices[index]].isOn; // Активируем или деактивируем скрипт в зависимости от состояния соответствующего Toggle
        }
    }
}
