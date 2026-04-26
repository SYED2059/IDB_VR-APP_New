using UnityEngine;
using UnityEngine.Video;

public class VideoSwitcher : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] videos;

    private int currentIndex = 0;

    void Start()
    {
        if (videos.Length > 0)
        {
            currentIndex = 0;
            videoPlayer.clip = videos[currentIndex];
            videoPlayer.Play();
        }
    }

    public void ChangeVideo()
    {
        currentIndex++;

        if (currentIndex >= videos.Length)
            currentIndex = 0;

        videoPlayer.Stop();
        videoPlayer.clip = videos[currentIndex];
        videoPlayer.Play();
    }
}