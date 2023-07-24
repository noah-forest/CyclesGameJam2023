using UnityEngine;

public class SpriteSheetAnimator : MonoBehaviour
{
    public Sprite[] sprites; // Assign the sprites from the sprite sheet in the Inspector
    public float framesPerSecond = 10f; // Speed of animation

    private SpriteRenderer spriteRenderer;
    private int currentFrameIndex = 0;

    private void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the animation loop
        InvokeRepeating("PlayAnimation", 0f, 1f / framesPerSecond);
    }

    private void PlayAnimation()
    {
        // Set the current frame of the animation
        spriteRenderer.sprite = sprites[currentFrameIndex];

        // Move to the next frame
        currentFrameIndex = (currentFrameIndex + 1) % sprites.Length;
    }
}