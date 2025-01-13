using DG.Tweening;
using UnityEngine;

public class WinEffect : MonoBehaviour
{
    private void OnEnable ()
    {

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        Color color = sprite.color;

        // Set the alpha to 0
        color.a = 0f;

        // Apply the modified color back to the sprite
        sprite.color = color;

        // Fade the sprite's alpha to 0 over 1 second
        sprite.DOFade(1f , 1f).SetEase(Ease.InOutQuad);
    }
}


