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

    public AudioClip Card1;
    public AudioClip Card2;
    public AudioClip Card3;
    public AudioClip Card4;
    public AudioClip Card5;
    public AudioClip Card6;



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


    //=================

    public void Card1Music()
    {
        gameManager.Clip_7TargetObjects_1.SetActive(false);
        gameManager.TargetCanvas.SetActive(true);
        gameManager.MainPanel.SetActive(true);

        if (musicSource == null || Card1 == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = Card1;
        musicSource.loop = false;
        musicSource.Play();
        gameManager.Clip_7TargetObjects_Card1.SetActive(true);
        musicCoroutine = StartCoroutine(WaitForCard1MusicEnd());
    }

    IEnumerator WaitForCard1MusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");
        gameManager.Clip_7TargetObjects_Card1.SetActive(false);
        Card2Music();
    }

    public void Card2Music()
    {
        if (musicSource == null || Card2 == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = Card2;
        musicSource.loop = false;
        musicSource.Play();
        gameManager.Clip_7TargetObjects_Card2.SetActive(true);


        musicCoroutine = StartCoroutine(WaitForCard2MusicEnd());
    }

    IEnumerator WaitForCard2MusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");
        gameManager.Clip_7TargetObjects_Card2.SetActive(false);
        Card3Music();

    }
    public void Card3Music()
    {
        if (musicSource == null || Card3 == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = Card3;
        musicSource.loop = false;
        musicSource.Play();
        gameManager.Clip_7TargetObjects_Card3.SetActive(true);

        musicCoroutine = StartCoroutine(WaitForCard3MusicEnd());
    }

    IEnumerator WaitForCard3MusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");
        gameManager.Clip_7TargetObjects_Card3.SetActive(false);

        Card4Music();
    }
    public void Card4Music()
    {
        if (musicSource == null || Card4 == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = Card4;
        musicSource.loop = false;
        musicSource.Play();
        gameManager.Clip_7TargetObjects_Card4.SetActive(true);




        musicCoroutine = StartCoroutine(WaitForCard4MusicEnd());
    }

    IEnumerator WaitForCard4MusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");
        gameManager.Clip_7TargetObjects_Card4.SetActive(false);

        Card5Music();

    }
    public void Card5Music()
    {
        if (musicSource == null || Card5 == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = Card5;
        musicSource.loop = false;
        musicSource.Play();
        gameManager.Clip_7TargetObjects_Card5.SetActive(true);



        musicCoroutine = StartCoroutine(WaitForCard5MusicEnd());
    }

    IEnumerator WaitForCard5MusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");
        gameManager.Clip_7TargetObjects_Card5.SetActive(false);

        Card6Music();
    }

    public void Card6Music()
    {
        if (musicSource == null || Card6 == null)
        {
            Debug.LogError("Music source or clip missing");
            return;
        }

        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);

        musicSource.clip = Card6;
        musicSource.loop = false;
        musicSource.Play();
        gameManager.Clip_7TargetObjects_Card6.SetActive(true);



        musicCoroutine = StartCoroutine(WaitForCard6MusicEnd());
    }

    IEnumerator WaitForCard6MusicEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);

        Debug.Log("EnterMusic finished");
        musicSource.Stop();
        gameManager.TargetCanvas.SetActive(false);
        gameManager.Clip_7TargetObjects_Card6.SetActive(false);
        gameManager.NewExitFN();
    }
    //public void StartCardSequence()
    //{
    //    if (musicCoroutine != null)
    //        StopCoroutine(musicCoroutine);

    //    musicCoroutine = StartCoroutine(PlaySequence());
    //}

    //IEnumerator PlaySequence()
    //{
    //    gameManager.Clip_7TargetObjects_1.SetActive(false);
    //    gameManager.TargetCanvas.SetActive(true);

    //    for (int i = 0; i < cards.Length; i++)
    //    {
    //        if (cards[i] == null)
    //        {
    //            Debug.LogError($"Card {i} missing");
    //            continue;
    //        }

    //        musicSource.clip = cards[i];
    //        musicSource.loop = false;
    //        musicSource.Play();

    //        if (cardObjects[i] != null)
    //            cardObjects[i].SetActive(true);

    //        // ✅ Better than WaitForSeconds
    //        yield return new WaitUntil(() => !musicSource.isPlaying);

    //        if (cardObjects[i] != null)
    //            cardObjects[i].SetActive(false);
    //    }

    //    musicSource.Stop();
    //    gameManager.TargetCanvas.SetActive(false);
    //    gameManager.NewExitFN();
    //}

    //public AudioClip[] cards;
    //public GameObject[] cardObjects;
}