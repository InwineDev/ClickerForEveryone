using UnityEngine;
using Mirror;

public class RadioManager : NetworkBehaviour
{
    [SerializeField] private AudioClip[] musicTracks; // Массив музыкальных треков
    private AudioSource audioSource; // Компонент AudioSource для воспроизведения музыки

    [SyncVar(hook = nameof(OnTrackIndexChanged))]
    private int currentTrackIndex = 0; // Индекс текущего трека (синхронизированный)

    private bool isPlaying = false; // Флаг, указывающий, играет ли музыка

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Не зацикливать треки
    }

    [Command(requiresAuthority = false)]
    public void CmdToggleRadio()
    {
        if (isPlaying)
        {
            RpcStopMusic();
        }
        else
        {
            PlayNextTrack(); // Вызов серверного метода для переключения трека
        }
    }

    private void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return;

        // Устанавливаем следующий трек на сервере
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        RpcPlayMusic(); // Запускаем воспроизведение на клиентах
    }

    [ClientRpc]
    private void RpcPlayMusic()
    {
        if (musicTracks.Length == 0) return;

        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
        isPlaying = true;
    }

    [ClientRpc]
    private void RpcStopMusic()
    {
        audioSource.Stop();
        isPlaying = false;
    }

    // Хук для синхронизации изменения текущего трека
    private void OnTrackIndexChanged(int oldIndex, int newIndex)
    {
        if (musicTracks.Length == 0) return;

        audioSource.clip = musicTracks[newIndex];

        if (isPlaying) // Если музыка должна играть, начинаем воспроизведение
        {
            audioSource.Play();
        }
    }

    void OnMouseDown() // Метод для обработки нажатия на объект
    {
        CmdToggleRadio(); // Любой игрок может переключать трек
    }
}
