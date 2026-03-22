using UnityEngine;

public class animationVisage : MonoBehaviour
{
    public Renderer targetRenderer;

    public Texture[] frames;   // <-- TES IMAGES ICI
    public float frameRate = 10f;

    private int index = 0;

    void Start()
    {
        InvokeRepeating(nameof(ChangeFrame), 0f, 1f / frameRate);
    }

    void ChangeFrame()
    {
        targetRenderer.material.SetTexture("_BaseMap", frames[index]);

        index++;
        if (index >= frames.Length)
            index = 0;
    }
}