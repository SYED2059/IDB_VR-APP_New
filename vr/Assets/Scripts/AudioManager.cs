using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip buttonClick;
    public AudioClip enterSound;
    public AudioClip ambientMusic;
    public AudioClip LogoSound;
    public AudioClip TargetSound;
    public AudioClip TargetLoopSound;

    private Coroutine musicCoroutine;

    void Start()
    {
        PlayMusic();
    }

    public void PlayClick()
    {
        if (buttonClick != null)
            sfxSource.PlayOneShot(buttonClick);
    }

    public void PlayEnter()
    {
        if (enterSound != null)
            sfxSource.PlayOneShot(enterSound);
    }

    public void PlayMusic()
    {
        if (musicSource == null || ambientMusic == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = ambientMusic;
        musicSource.loop = false;
        musicSource.Play();

        musicCoroutine = StartCoroutine(WaitForMusicEnd());
    }

    IEnumerator WaitForMusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("Music finished");

        if (gameManager != null)
        {
            gameManager.EnterButton.SetActive(true);
        }
    }

    public void PlayEnterMusic()
    {
        if (musicSource == null || enterSound == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = enterSound;
        musicSource.loop = false;
        musicSource.Play();

        musicCoroutine = StartCoroutine(WaitForEnterMusicEnd());
    }

    IEnumerator WaitForEnterMusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");

        if (gameManager != null)
        {
            gameManager.BeginButton.SetActive(true);
        }
    }


    public void PlayLogoMusic()
    {
        if (musicSource == null || LogoSound == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = LogoSound;
        musicSource.loop = false;
        musicSource.Play();

        musicCoroutine = StartCoroutine(WaitForLogoMusicEnd());
    }

    IEnumerator WaitForLogoMusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");

        if (gameManager != null)
        {
            gameManager.LogoCanvasObj.SetActive(false);
            gameManager.StartCoroutine(gameManager.Wait1Seconds());
        }
    }

    public void PlayTargetMusic()
    {
        if (musicSource == null || TargetSound == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = TargetSound;
        musicSource.loop = false;
        musicSource.Play();
        musicCoroutine = StartCoroutine(WaitForTargetMusicEnd());
    }

    IEnumerator WaitForTargetMusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");
    }

    public void PlayTargetLoopMusic()
    {
        if (musicSource == null || TargetLoopSound == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = TargetLoopSound;
        musicSource.loop = false;
        musicSource.Play();

        musicCoroutine = StartCoroutine(WaitForTargetLoopMusicEnd());
    }

    IEnumerator WaitForTargetLoopMusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");
        gameManager.StartCoroutine(gameManager.StopLoopAfterTime(1f));
    }
}