using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FlashManager : MonoBehaviour
{
    public VideoPlayer FlashvideoPlayer;
    public VideoClip FlashClip;

    void Start()
    {
        FlashvideoPlayer.waitForFirstFrame = true;
        FlashvideoPlayer.loopPointReached += OnFlashVideoFinished;
    }

    void OnFlashVideoFinished(VideoPlayer vp)
    {
        if (vp.clip == FlashClip)
        {
            Debug.Log("FlashVideoFinished");
            SceneManager.LoadScene("GameScene");
        }
    }
}
