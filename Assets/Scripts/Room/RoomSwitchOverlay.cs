using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomSwitchOverlay : MonoBehaviour
{
    // The black screen covering the game upon transition.
    public Image overlay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // Fade-out method (hides the game)
    public void FadeOut(float duration = 1.0f)
    {
        StartCoroutine(FadeOverlay(false, duration));
    }


    // Fade-in method (shows the game)
    public void FadeIn(float duration = 1.0f)
    {
        StartCoroutine(FadeOverlay(true, duration));
    }


    // Fades the overlay (shows it or hides it depending on fadeOut parameter)
    private IEnumerator FadeOverlay(bool fadeOut, float duration)
    {
        if (!overlay)
        {
            yield break;
        }

        // Initializes a timer
        float timer = 0.0f;
        // Gets the initial color of the overlay
        Color color = overlay.color;
        float initialAlpha = color.a;
        // The alpha value to reach at the end of the fade
        float finalAlpha = (fadeOut) ? 0.0f : 1.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(initialAlpha, finalAlpha, timer / duration);
            overlay.color = color;

            yield return null;
        }

        color.a = finalAlpha;
        overlay.color = color;
    }
}
