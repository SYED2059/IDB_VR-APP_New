using UnityEngine;
using UnityEngine.UI;

public class LoaderAnimation : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image targetImage;

    [Header("Frames")]
    [SerializeField] private Sprite[] frames;

    [Header("Debug (Optional)")]
    [Range(0f, 1f)]
    [SerializeField] private float progress = 0f;

    private int lastFrame = -1;

    void Update()
    {
        // Only for testing in Inspector
        UpdateFrame(progress);
    }

    /// <summary>
    /// Call this from your loading logic
    /// value should be between 0 and 1
    /// </summary>
    public void SetProgress(float value)
    {
        progress = Mathf.Clamp01(value);
        UpdateFrame(progress);
    }

    private void UpdateFrame(float value)
    {
        if (frames == null || frames.Length == 0 || targetImage == null)
            return;

        int frameIndex = Mathf.FloorToInt(value * (frames.Length - 1));

        // Prevent unnecessary sprite updates
        if (frameIndex == lastFrame)
            return;

        lastFrame = frameIndex;
        targetImage.sprite = frames[frameIndex];
    }
}