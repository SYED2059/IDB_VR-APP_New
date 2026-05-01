using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;

    [Header("Transforms")]
    public Transform SpaceshipObj;
    public Transform PlayerCameraObj;
    public Transform SpaceshipEntryPoint;
    public Transform PlayerCameraEntryPoint;
    public Transform SpaceshipBeginPoint;
    public Transform PlayerCameraBeginPoint;
    public Transform SpaceshipRestartPoint;
    public Transform PlayerCameraRestartPoint;
    public Transform PlayerCameraStartingPoint;

    [Header("GameObjects")]
    public GameObject VfxObj;
    public GameObject locomotor;
    public GameObject EnterButton;
    public GameObject BeginButton;
    public GameObject TargetCanvasObj;
    public GameObject LogoCanvasObj;
    public GameObject LoopCanvasObj;
    public GameObject MainCanvas;
    public GameObject MainPanel;
    public GameObject targetImage;
    public GameObject MainButtonCanvas;
    public GameObject ContinueBtn;
    public GameObject ContinueDoubleBtn;
    public GameObject NormalSphere;
    public GameObject DoubleSphere;
    public GameObject DoublePanel;
    public GameObject DoubleSubPanel;
    public GameObject PodCanvas;
    public GameObject ExitButton;


    [Header("UI Card Animation")]
    public GameObject[] uiCards;
    public bool startUICardLoop = false;
    public float cardStartDistance = 50f;
    public float cardCenterDistance = 4f;
    public float cardEndDistance = -50f;
    public float enterDuration = 3f;
    public float stayDuration = 0.5f;
    public float exitDuration = 1f;
    public float delayBetweenCards = 0.1f;

    [Header("VideoPlayer")]
    public VideoPlayer videoPlayer;
    public VideoPlayer DoubleVideoPlayer_1;
    public VideoPlayer DoubleVideoPlayer_2;

    [Header("VideoClips")]
    public VideoClip Clip_1;
    public VideoClip Clip_2;
    public VideoClip Clip_3;
    public VideoClip Clip_4;
    public VideoClip Clip_5;
    public VideoClip Clip_6;
    public VideoClip Clip_7;
    public VideoClip Clip_8;
    public VideoClip Clip_9;

    [Header("ButtonActiveVariable")]
    public Button[] buttons;
    public Sprite activeSprite;
    public Sprite normalSprite;
    private Button currentActive;

    public VisualEffect Vfx;

    public Image ReferenceImage;
    public Image[] Images;



    void Start()
    {
        StartCoroutine(Align());
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    IEnumerator Align()
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 dir = (SpaceshipObj.position - PlayerCameraObj.position);
        dir.y = 0; 

        PlayerCameraObj.rotation = Quaternion.LookRotation(dir);
        PodCanvas.SetActive(true);
    }

    public void OnEnterClicked()
    {

        Debug.Log("Enter Button Clicked");
        audioManager.PlayEnterMusic();
        if (SpaceshipObj && SpaceshipEntryPoint)
        {
            SpaceshipObj.SetPositionAndRotation(
                SpaceshipEntryPoint.position,
                SpaceshipEntryPoint.rotation
            );
        }

        if (PlayerCameraObj && PlayerCameraEntryPoint)
        {
            PlayerCameraObj.SetPositionAndRotation(
                PlayerCameraEntryPoint.position,
                PlayerCameraEntryPoint.rotation
            );
        }
        StartCoroutine(EnableMovementAfterDelay());
        EnterButton.SetActive(false);
        PodCanvas.SetActive(false);

    }

    IEnumerator EnableMovementAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (locomotor != null)
        {
            locomotor.SetActive(false);
        }
    }

    public void OnBeginClicked()
    {
        audioManager.PlayClick();
        Debug.Log("Begin Button Clicked");
        locomotor.SetActive(true);
        VfxObj.SetActive(true);
        Vfx.Reinit();
        Vfx.SendEvent("OnPlay");
        if (SpaceshipObj && SpaceshipEntryPoint)
        {
            SpaceshipObj.SetPositionAndRotation(
                SpaceshipBeginPoint.position,
                SpaceshipBeginPoint.rotation
            );
        }

        if (PlayerCameraObj && PlayerCameraEntryPoint)
        {
            PlayerCameraObj.SetPositionAndRotation(
                PlayerCameraBeginPoint.position,
                PlayerCameraBeginPoint.rotation
            );
        }
        //StartCoroutine(EnableMovementAfterDelay());
        StartCoroutine(Wait5Seconds());
    }
    IEnumerator Wait5Seconds()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Done after 5 seconds!");
        LogoCanvasObj.SetActive(false);
        TargetCanvasObj.SetActive(true);
    }

    public void ActiveLoop()
    {
        audioManager.PlayClick();
        LogoCanvasObj.SetActive(false);
        TargetCanvasObj.SetActive(false);
        LoopCanvasObj.SetActive(true);
        startUICardLoop = true;
        StartCoroutine(ShowUICardsLoop());
        StartCoroutine(StopLoopAfterTime(5f));
    }

    IEnumerator ShowUICardsLoop()
    {
        float sideOffset = 80f;

        while (startUICardLoop)
        {
            for (int i = 0; i < uiCards.Length; i++)
            {
                GameObject currentCard = uiCards[i];

                if (currentCard == null)
                    continue;

                currentCard.SetActive(true);

                CanvasGroup cg = currentCard.GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = currentCard.AddComponent<CanvasGroup>();

                cg.alpha = 0f;

                bool goRight = (i % 2 == 0);

                Vector3 startLocalPos = new Vector3(0f, 0f, cardStartDistance);
                Vector3 centerLocalPos = new Vector3(0f, 0f, cardCenterDistance);

                Vector3 endLocalPos = goRight
                    ? new Vector3(sideOffset, 0f, cardEndDistance)
                    : new Vector3(-sideOffset, 0f, cardEndDistance);

                currentCard.transform.localPosition = startLocalPos;

                float enterTimer = 0f;
                while (enterTimer < enterDuration)
                {
                    enterTimer += Time.deltaTime;
                    float t = enterTimer / enterDuration;

                    currentCard.transform.localPosition = Vector3.Lerp(
                        startLocalPos,
                        centerLocalPos,
                        t
                    );

                    cg.alpha = Mathf.Lerp(0f, 1f, t);

                    yield return null;
                }

                currentCard.transform.localPosition = centerLocalPos;
                cg.alpha = 1f;

                yield return new WaitForSeconds(stayDuration);

                float exitTimer = 0f;
                while (exitTimer < exitDuration)
                {
                    exitTimer += Time.deltaTime;
                    float t = exitTimer / exitDuration;

                    float smoothT = Mathf.SmoothStep(0f, 1f, t);

                    currentCard.transform.localPosition = Vector3.Lerp(
                        centerLocalPos,
                        endLocalPos,
                        smoothT
                    );

                    cg.alpha = Mathf.Lerp(1f, 0f, t);

                    yield return null;
                }

                currentCard.SetActive(false);

                yield return new WaitForSeconds(delayBetweenCards);
            }
        }
    }

    IEnumerator StopLoopAfterTime(float time)
    {
        videoPlayer.isLooping = false;
        yield return new WaitForSeconds(time);

        startUICardLoop = false;

        yield return StartCoroutine(FadeVideo(1f, 0f, 0.5f));
        VfxObj.SetActive(false);
        NormalSphere.SetActive(true);
        LoopCanvasObj.SetActive(false);
        SpaceshipObj.gameObject.SetActive(false);
        TargetCanvasObj.SetActive(false);
        MainCanvas.SetActive(true);
        MainPanel.SetActive(true);

        videoPlayer.Stop();
        videoPlayer.clip = Clip_1;
        videoPlayer.Play();

        yield return new WaitUntil(() => videoPlayer.isPlaying);

        yield return StartCoroutine(FadeVideo(0f, 1f, 0.5f));
    }

    IEnumerator FadeVideo(float start, float end, float duration)
    {
        CanvasGroup cg = videoPlayer.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = videoPlayer.gameObject.AddComponent<CanvasGroup>();

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            cg.alpha = Mathf.Lerp(start, end, t);

            yield return null;
        }

        cg.alpha = end;
    }

    public void OnRestartClicked()
    {
        audioManager.PlayClick();
        Debug.Log("Restart Button Clicked");
        startUICardLoop = false;
        EnterButton.SetActive(true);
        BeginButton.SetActive(true);
        TargetCanvasObj.SetActive(false);
        LoopCanvasObj.SetActive(false);
        Vfx.Stop();
        VfxObj.SetActive(false);

        if (SpaceshipObj && SpaceshipEntryPoint)
        {
            SpaceshipObj.SetPositionAndRotation(
                SpaceshipRestartPoint.position,
                SpaceshipRestartPoint.rotation
            );
        }

        if (PlayerCameraObj && PlayerCameraEntryPoint)
        {
            PlayerCameraObj.SetPositionAndRotation(
                PlayerCameraRestartPoint.position,
                PlayerCameraRestartPoint.rotation
            );
        }

    }
    public void TogglePlayPause(Button clickedBtn)
    {
        audioManager.PlayClick();

        if (currentActive != null)
        {
            currentActive.GetComponent<Image>().sprite = normalSprite;
        }

        clickedBtn.GetComponent<Image>().sprite = activeSprite;
        currentActive = clickedBtn;
        foreach (var item in Images)
        {
            item.gameObject.SetActive(false);
        }
        //targetImage.SetActive(!targetImage.activeSelf);
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (vp.clip == Clip_1)
        {
            OnClip1Finished();
        }
        if (vp.clip == Clip_2)
        {
            OnClip2Finished();
        }
        if (vp.clip == Clip_3)
        {
            OnClip3Finished();
        }
        if (vp.clip == Clip_4)
        {
            OnClip4Finished();
        }
        if (vp.clip == Clip_5)
        {
            OnClip5Finished();
        }
        if (vp.clip == Clip_6)
        {
            OnClip6Finished();
        }
        if (vp.clip == Clip_7)
        {
            OnClip7Finished();
        }
        if (vp.clip == Clip_8)
        {
            OnClip8Finished();
        }
        if (vp.clip == Clip_9)
        {
            OnClip9Finished();
        }
    }

    void OnClip1Finished()
    {
        Debug.Log("Clip 1 Finished!");
        targetImage.SetActive(false);
        MainCanvas.SetActive(false);
        MainButtonCanvas.SetActive(true);
        foreach (var item in Images)
        {
            item.gameObject.SetActive(false);
        }
    }

    void OnClip2Finished()
    {
        Debug.Log("Clip 2 Finished!");
        ContinueBtn.SetActive(true);
    }

    void OnClip3Finished()
    {
        Debug.Log("Clip 3Finished!");
        ContinueBtn.SetActive(true);
    }

    void OnClip4Finished()
    {
        Debug.Log("Clip 4 Finished!");
        //ContinueBtn.SetActive(true);
        NormalSphere.SetActive(false);
        DoubleSphere.SetActive(true);
        videoPlayer.gameObject.SetActive(false);
        DoubleVideoPlayer_1.gameObject.SetActive(true);
        DoubleVideoPlayer_2.gameObject.SetActive(true);
        DoublePanel.SetActive(true);
    }

    void OnClip5Finished()
    {
        DoubleSubPanel.SetActive(true);
    }

    void OnClip6Finished()
    {
        DoubleSubPanel.SetActive(true);
    }

    void OnClip7Finished()
    {
        ContinueDoubleBtn.SetActive(true);
    }

    void OnClip8Finished()
    {
        ContinueDoubleBtn.SetActive(true);
    }

    void OnClip9Finished()
    {
        ContinueDoubleBtn.SetActive(false);
        DoubleSubPanel.SetActive(false);
        ContinueBtn.SetActive(false);
        NormalSphere.SetActive(false);
        DoubleSphere.SetActive(false);
        SpaceshipObj.gameObject.SetActive(true);
        OnBeginClicked();
        StartCoroutine(CallAfter2Sec());
    }

    public Sprite Biologics_ActiveSprite;
    public Sprite Biologics_InActiveSprite;

    public Sprite SmallMolecules_ActiveSprite;
    public Sprite SmallMolecules_InActiveSprite;

    public Sprite Continue_ActiveSprite;
    public Sprite Continue_InActiveSprite;


    public Sprite AntiTNFagents_ActiveSprite;
    public Sprite AntiTNFagents_InActiveSprite;

    public Sprite SmallMoleculesDouble_ActiveSprite;
    public Sprite SmallMoleculesDouble_InActiveSprite;

    public Sprite ContinueDouble_ActiveSprite;
    public Sprite ContinueDouble_InActiveSprite;



    public void BiologicsFN(Button Btn)
    {
        audioManager.PlayClick();
        Btn.interactable = false;
        Btn.image.sprite = Biologics_InActiveSprite;
        MainButtonCanvas.SetActive(false);
        videoPlayer.Stop();
        videoPlayer.clip = Clip_2;
        videoPlayer.Play();
        StartCoroutine(TriggerAtVideoTime(2f, Clip_2TargetObjects_1));
        StartCoroutine(TriggerAtVideoTime(4f, Clip_2TargetObjects_2));
        StartCoroutine(TriggerAtVideoTime(5f, Clip_2TargetObjects_3));
    }

    public void SmallMoleculesFN(Button Btn)
    {
        audioManager.PlayClick();
        Btn.interactable = false;
        Btn.image.sprite = SmallMolecules_InActiveSprite;
        MainButtonCanvas.SetActive(false);

        videoPlayer.Stop();
        videoPlayer.clip = Clip_3;
        videoPlayer.Play();

        StartCoroutine(TriggerAtVideoTime(2f, Clip_3TargetObjects_1));
        StartCoroutine(TriggerAtVideoTime(4f, Clip_3TargetObjects_2));
        //StartCoroutine(TriggerAtVideoTime(5f, Clip_3TargetObjects_3));
    }

    public void ContinueFN(Button Btn)
    {
        audioManager.PlayClick();
        Btn.interactable = false;
        Btn.image.sprite = Continue_InActiveSprite;
        MainButtonCanvas.SetActive(false);
        videoPlayer.Stop();
        videoPlayer.clip = Clip_4;
        videoPlayer.Play();
        StartCoroutine(TriggerAtVideoTime(2f, Clip_4TargetObjects_1));
        StartCoroutine(TriggerAtVideoTime(4f, Clip_4TargetObjects_2));
        //StartCoroutine(TriggerAtVideoTime(5f, Clip_4TargetObjects_3));
    }

    public GameObject TargetCanvas;
    public GameObject Clip_2TargetObjects_1;
    public GameObject Clip_2TargetObjects_2;
    public GameObject Clip_2TargetObjects_3;

    public GameObject Clip_3TargetObjects_1;
    public GameObject Clip_3TargetObjects_2;
    //public GameObject Clip_3TargetObjects_3;

    public GameObject Clip_4TargetObjects_1;
    public GameObject Clip_4TargetObjects_2;
    //public GameObject Clip_4TargetObjects_3;

    public GameObject Clip_5TargetObjects_1;
    //public GameObject Clip_5TargetObjects_2;
    //public GameObject Clip_5TargetObjects_3;


    public GameObject Clip_6TargetObjects_1;
    //public GameObject Clip_6TargetObjects_2;
    //public GameObject Clip_6TargetObjects_3;


    public GameObject Clip_7TargetObjects_1;
    //public GameObject Clip_7TargetObjects_2;
    //public GameObject Clip_7TargetObjects_3;


    public GameObject Clip_8TargetObjects_1;
    //public GameObject Clip_8TargetObjects_2;
    //public GameObject Clip_8TargetObjects_3;

    public GameObject Clip_9TargetObjects_1;
    //public GameObject Clip_9TargetObjects_2;
    //public GameObject Clip_9TargetObjects_3;




    IEnumerator TriggerAtVideoTime(double targetTime, GameObject targetObject)
    {
        // wait until video actually starts
        yield return new WaitUntil(() => videoPlayer.isPlaying);

        // wait until video reaches time
        while (videoPlayer.time < targetTime)
        {
            yield return null;
        }

        // show
        targetObject.SetActive(true);
        TargetCanvas.SetActive(true);

        // hide after 2 sec (real time, not video time)
        yield return new WaitForSeconds(1f);

        targetObject.SetActive(false);
        TargetCanvas.SetActive(false);
    }



    

    public void CrohnsDiseaseFn()
    {
        audioManager.PlayClick();
        NormalSphere.SetActive(true);
        DoubleSphere.SetActive(false);
        DoublePanel.SetActive(false);
        videoPlayer.gameObject.SetActive(true);
        DoubleVideoPlayer_1.gameObject.SetActive(false);
        DoubleVideoPlayer_2.gameObject.SetActive(false);
        videoPlayer.Stop();
        videoPlayer.clip = Clip_5;
        videoPlayer.Play();
        StartCoroutine(TriggerAtVideoTime(2f, Clip_5TargetObjects_1));
        //StartCoroutine(TriggerAtVideoTime(4f, Clip_5TargetObjects_2));
        //StartCoroutine(TriggerAtVideoTime(5f, Clip_5TargetObjects_3));
    }

    public void UlcerativeColitisFn()
    {
        audioManager.PlayClick();
        NormalSphere.SetActive(true);
        DoubleSphere.SetActive(false);
        DoublePanel.SetActive(false);
        videoPlayer.gameObject.SetActive(true);
        DoubleVideoPlayer_1.gameObject.SetActive(false);
        DoubleVideoPlayer_2.gameObject.SetActive(false);
        videoPlayer.Stop();
        videoPlayer.clip = Clip_6;
        videoPlayer.Play();
        StartCoroutine(TriggerAtVideoTime(2f, Clip_6TargetObjects_1));
        //StartCoroutine(TriggerAtVideoTime(4f, Clip_6TargetObjects_2));
        //StartCoroutine(TriggerAtVideoTime(5f, Clip_6TargetObjects_3));
    }

    public void ContinueBtnFN()
    {
        audioManager.PlayClick();
        ContinueBtn.SetActive(false);
        MainButtonCanvas.SetActive(true);
    }
    //======================================================================================================
    
    public void AntiTNFagentsFN(Button Btn)
    {
        audioManager.PlayClick();
        Btn.interactable = false;
        Btn.image.sprite = AntiTNFagents_InActiveSprite;
        DoubleSubPanel.SetActive(false);
        videoPlayer.Stop();
        videoPlayer.clip = Clip_7;
        videoPlayer.Play();

        StartCoroutine(TriggerAtVideoTime(2f, Clip_7TargetObjects_1));
        //StartCoroutine(TriggerAtVideoTime(4f, Clip_7TargetObjects_2));
        //StartCoroutine(TriggerAtVideoTime(5f, Clip_7TargetObjects_3));
    }

    public void SmallMoleculesDoubleFN(Button Btn)
    {
        audioManager.PlayClick();
        Btn.interactable = false;
        Btn.image.sprite = SmallMoleculesDouble_InActiveSprite;
        DoubleSubPanel.SetActive(false);
        videoPlayer.Stop();
        videoPlayer.clip = Clip_8;
        videoPlayer.Play();
        StartCoroutine(TriggerAtVideoTime(2f, Clip_8TargetObjects_1));
        //StartCoroutine(TriggerAtVideoTime(4f, Clip_8TargetObjects_2));
        //StartCoroutine(TriggerAtVideoTime(5f, Clip_8TargetObjects_3));
    }

    public void ContinueDoubleFN(Button Btn)
    {
        audioManager.PlayClick();
        Btn.interactable = false;
        Btn.image.sprite = ContinueDouble_InActiveSprite;
        DoubleSubPanel.SetActive(false);
        videoPlayer.Stop();
        videoPlayer.clip = Clip_9;
        videoPlayer.Play();

        StartCoroutine(TriggerAtVideoTime(2f, Clip_9TargetObjects_1));
        //StartCoroutine(TriggerAtVideoTime(4f, Clip_9TargetObjects_2));
        //StartCoroutine(TriggerAtVideoTime(5f, Clip_9TargetObjects_3));
    }

    public void ContinueDoubleBtnFN()
    {
        audioManager.PlayClick();
        ContinueDoubleBtn.SetActive(false);
        DoubleSubPanel.SetActive(true);
    }


    IEnumerator CallAfter2Sec()
    {
        yield return new WaitForSeconds(2f);
        PodCanvas.SetActive(true);
        OnRestartClicked();
        EnterButton.SetActive(false);
        ExitButton.SetActive(true);
    }

    public void ExitBtnFN()
    {
        audioManager.PlayClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //======================================================================================================

    public void RestartFN()
    {
        audioManager.PlayClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RefereneFN()
    {
        audioManager.PlayClick();
        Time.timeScale = 0f;

        ReferenceImage.gameObject.SetActive(true);

        Vfx.Stop();

        videoPlayer.Pause();
        DoubleVideoPlayer_1.Pause();
        DoubleVideoPlayer_2.Pause();
    }

    public void ExitReference()
    {
        audioManager.PlayClick();
        Time.timeScale = 1f;

        ReferenceImage.gameObject.SetActive(false);

        Vfx.Play();

        videoPlayer.Play();
        DoubleVideoPlayer_1.Play();
        DoubleVideoPlayer_2.Play();
    }

}