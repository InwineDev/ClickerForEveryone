using UnityEngine;
using Mirror;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

public class RadioManager : NetworkBehaviour
{
    private AudioSource audioSource;
    
    [SyncVar(hook = nameof(OnTrackIndexChanged))]
    private int currentTrackIndex = 0;
    
    [SyncVar(hook = nameof(OnIsPlayingChanged))]
    private bool isPlaying = false;
    
    private static AudioClip[] musicTracks;
    private static string musicFolderPath;
    
    [SyncVar(hook = nameof(OnTrackNameChanged))]
    private string currentTrackName = "";

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        
        // Определяем путь к папке Music
        musicFolderPath = Path.Combine(Application.dataPath, "..", "Music");
        
        // Только сервер загружает музыку из папки
        if (isServer)
        {
            StartCoroutine(LoadMusicFromFolder());
        }
    }

    // Серверная корутина для загрузки музыки
    private IEnumerator LoadMusicFromFolder()
    {
        Debug.Log($"Loading music from: {musicFolderPath}");
        
        if (!Directory.Exists(musicFolderPath))
        {
            Debug.LogWarning($"Music directory not found: {musicFolderPath}");
            yield break;
        }

        string[] supportedFormats = { "*.wav", "*.mp3", "*.ogg", "*.aiff" };
        ArrayList audioFiles = new ArrayList();

        foreach (string format in supportedFormats)
        {
            string[] files = Directory.GetFiles(musicFolderPath, format);
            audioFiles.AddRange(files);
        }

        if (audioFiles.Count == 0)
        {
            Debug.LogWarning("No music files found in Music folder");
            yield break;
        }

        musicTracks = new AudioClip[audioFiles.Count];
        
        for (int i = 0; i < audioFiles.Count; i++)
        {
            string filePath = (string)audioFiles[i];
            string fileName = Path.GetFileName(filePath);
            
            Debug.Log($"Loading music file: {fileName}");
            
            // Используем UnityWebRequest для загрузки аудиофайлов
            string url = "file://" + filePath;
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, GetAudioType(filePath)))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    clip.name = fileName;
                    musicTracks[i] = clip;
                    Debug.Log($"Successfully loaded: {fileName}");
                }
                else
                {
                    Debug.LogError($"Failed to load {fileName}: {www.error}");
                }
            }
        }
        
        Debug.Log($"Loaded {musicTracks.Length} music tracks");
        
        // Уведомляем клиентов о загрузке
        RpcMusicLoaded(musicTracks.Length);
    }

    // Определяем тип аудио по расширению файла
    private AudioType GetAudioType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".wav": return AudioType.WAV;
            case ".mp3": return AudioType.MPEG;
            case ".ogg": return AudioType.OGGVORBIS;
            case ".aiff": return AudioType.AIFF;
            default: return AudioType.UNKNOWN;
        }
    }

    [ClientRpc]
    private void RpcMusicLoaded(int trackCount)
    {
        Debug.Log($"Server loaded {trackCount} music tracks");
    }

    [Command(requiresAuthority = false)]
    public void CmdToggleRadio()
    {
        if (isPlaying)
        {
            isPlaying = false;
            RpcStopMusic();
        }
        else
        {
            PlayNextTrack();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdNextTrack()
    {
        PlayNextTrack();
    }

    [Command(requiresAuthority = false)]
    public void CmdStopMusic()
    {
        isPlaying = false;
        RpcStopMusic();
    }

    private void PlayNextTrack()
    {
        if (musicTracks == null || musicTracks.Length == 0) 
        {
            Debug.LogWarning("No music tracks available");
            return;
        }

        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        currentTrackName = musicTracks[currentTrackIndex]?.name ?? "Unknown";
        isPlaying = true;
        
        RpcPlayMusic(currentTrackIndex);
    }

    [ClientRpc]
    private void RpcPlayMusic(int trackIndex)
    {
        if (musicTracks == null || musicTracks.Length == 0) 
        {
            Debug.LogWarning("No music tracks available on client");
            return;
        }

        if (trackIndex < 0 || trackIndex >= musicTracks.Length)
        {
            Debug.LogError($"Invalid track index: {trackIndex}");
            return;
        }

        audioSource.clip = musicTracks[trackIndex];
        audioSource.Play();
        isPlaying = true;
        
        Debug.Log($"Now playing: {musicTracks[trackIndex].name}");
    }

    [ClientRpc]
    private void RpcStopMusic()
    {
        audioSource.Stop();
        isPlaying = false;
        Debug.Log("Music stopped");
    }

    // Хуки для синхронизации
    private void OnTrackIndexChanged(int oldIndex, int newIndex)
    {
        if (musicTracks != null && newIndex >= 0 && newIndex < musicTracks.Length)
        {
            audioSource.clip = musicTracks[newIndex];
            if (isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnIsPlayingChanged(bool oldValue, bool newValue)
    {
        isPlaying = newValue;
        if (!newValue && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnTrackNameChanged(string oldName, string newName)
    {
        currentTrackName = newName;
        Debug.Log($"Track changed to: {newName}");
    }

    void OnMouseDown()
    {
        CmdToggleRadio();
    }

    // Метод для UI (можно вызвать из кнопок)
    public void PlayNext()
    {
        CmdNextTrack();
    }

    public void StopMusic()
    {
        CmdStopMusic();
    }

    // Для отображения текущего трека в UI
    public string GetCurrentTrackName()
    {
        return currentTrackName;
    }

    public bool IsMusicPlaying()
    {
        return isPlaying;
    }
}