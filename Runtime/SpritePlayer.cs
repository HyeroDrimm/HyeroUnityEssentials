using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpritePlayer : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float interval = 1;
    [SerializeField] private bool useUnscaledTime;

    private float timeCounter = 0;
    private int spriteCounter = 0;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (sprites.Length == 0)
        {
            Debug.LogError("No sprites", this);
        }
    }

    private void Update()
    {
        if (useUnscaledTime)
            timeCounter += Time.unscaledDeltaTime;
        else
            timeCounter += Time.deltaTime;

        if (timeCounter >= interval)
        {
            timeCounter = 0;
            spriteCounter = (spriteCounter + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[spriteCounter];
        }
    }

}
