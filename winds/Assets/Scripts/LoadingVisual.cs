using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingVisual : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (!image)
            image = GetComponent<Image>();
        SetAlpha(1f);
    }
    public IEnumerator FadeOut()
    {
        yield return Fade(1f, 0f);
        gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        gameObject.SetActive(true);
        yield return Fade(0f, 1f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = image.color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(from, to, t / fadeDuration);
            image.color = c;
            yield return null;
        }

        c.a = to;
        image.color = c;
    }
    public void SetAlpha(float a)
    {
        var c = image.color;
        c.a = Mathf.Clamp01(a);
        image.color = c;
    }

    public void ShowInstant()
    {
        gameObject.SetActive(true);
        SetAlpha(1f);
    }

    public void HideInstant()
    {
        SetAlpha(0f);
        gameObject.SetActive(false);
    }



}
