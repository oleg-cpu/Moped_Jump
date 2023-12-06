using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;
    private int frame;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnEnable()
    {

        Invoke(nameof(Animate), 0f);
    }

    private void Animate()
    {

        frame++;

        if (frame >= sprites.Length)
        {
            frame = 0;
        }

        if (frame >= 0 && frame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[frame];
        }

        Invoke(nameof(Animate), 1f / GameManager.Instance.gameSpeed);
    }

}
