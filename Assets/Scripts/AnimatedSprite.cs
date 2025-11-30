using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationSpeed = 10f;
    private int frame;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        if(sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    private void OnEnable()
    {

        Invoke(nameof(Animate), 0f);
    }

    private void OnDisable()
    {
       CancelInvoke(nameof(Animate)); 
    }

    public void StartAnimation()
    {
        frame = 0;
        if(sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }
        InvokeRepeating(nameof(Animate), 1f / animationSpeed, 1f / animationSpeed);
        
    }

    public void StopAnimation()
    {
        CancelInvoke(nameof(Animate));
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
    }

}
