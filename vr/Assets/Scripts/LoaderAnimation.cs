using UnityEngine;
using UnityEngine.UI;

public class LoaderAnimation : MonoBehaviour
{
    public Image targetImage;
    public Sprite[] frames;
    private float fps = 60f;

    private int currentFrame;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / fps)
        {
            timer = 0f;

            currentFrame = (currentFrame + 1) % frames.Length;
            targetImage.sprite = frames[currentFrame];
        }
    }
}
