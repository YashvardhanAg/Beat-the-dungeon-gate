using Unity.VisualScripting;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;
    private AudioSource menuMusic;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        menuMusic = GetComponent<AudioSource>();
    }

    public void StopMusic()
    {
        if (menuMusic != null && menuMusic.isPlaying)
        {
            menuMusic.Stop();
        }
    }

    public void PlayMusic()
    {
        if (menuMusic != null && !menuMusic.isPlaying) 
        {
            menuMusic.Play();
        }
    }
}
