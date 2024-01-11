using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class FadeTransition
{
    public static void FadeScreen(Color color, float fromAlpha, float toAlpha, float fadeTimeSeconds, Action endAction = null)
    {
        Image image = CreateFadeImage();
        image.StartCoroutine(FadeCoroutine(image, color, fromAlpha, toAlpha, fadeTimeSeconds, endAction));
    }


    private static Image CreateFadeImage()
    {
        GameObject canvasGo = new GameObject("CanvasEffect");
        Canvas canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        GameObject imageGo = new GameObject("FadeImage");
        imageGo.transform.SetParent(canvasGo.transform);

        Image image = imageGo.AddComponent<Image>();
        image.rectTransform.anchoredPosition = Vector2.zero;
        image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        return image;
    }

    private static IEnumerator FadeCoroutine(Image image, Color color, float fromAlpha, float toAlpha, float fadeTimeSeconds, Action action)
    {
        color.a = fromAlpha;
        image.color = color;

        float time = 1f / fadeTimeSeconds;
        float progress = 0;

        while (progress < 1)
        {
            progress += time * Time.deltaTime;
            color.a = Mathf.Lerp(fromAlpha, toAlpha, progress);
            image.color = color;
            yield return null;
        }

        if (action != null) action.Invoke();

        GameObject.Destroy(image.transform.root.gameObject, 1);
    }
}