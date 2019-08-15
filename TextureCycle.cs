using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Leon Arndt. Can be used for both UI and World Object textures :)

public class TextureCycle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Image image;

    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    float framesPerSecond = 10;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        InvokeRepeating("Cycle", 0f, 1f / framesPerSecond);
    }

    // Iterate once
    private void Cycle()
    {
        int index = (int)(Time.time * framesPerSecond) % sprites.Length;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprites[index];
        }

        if (image != null)
        {
            image.sprite = sprites[index];
        }
    }

    public void SetSprites(Sprite[] newSprites)
    {
        sprites = newSprites;
    }
}
