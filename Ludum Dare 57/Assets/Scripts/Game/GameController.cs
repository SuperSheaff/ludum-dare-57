using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    [Header("Text Group")]
    [SerializeField] private Transform textGroup;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private List<TMP_Text> textList = new();
    private Dictionary<int, Coroutine> runningCoroutines = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        CacheTextObjects();
    }

    private void CacheTextObjects()
    {
        textList.Clear();
        foreach (Transform child in textGroup)
        {
            if (child.TryGetComponent(out TMP_Text tmp))
            {
                tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0f); // Start hidden
                textList.Add(tmp);
            }
        }
    }

    public void ShowText(int index)
    {
        if (IsValidIndex(index))
        {
            StartFade(index, 1f);
        }
    }

    public void HideText(int index)
    {
        if (IsValidIndex(index))
        {
            StartFade(index, 0f);
        }
    }

    private void StartFade(int index, float targetAlpha)
    {
        if (runningCoroutines.TryGetValue(index, out Coroutine running))
        {
            StopCoroutine(running);
        }

        runningCoroutines[index] = StartCoroutine(FadeText(textList[index], targetAlpha, index));
    }

    private IEnumerator FadeText(TMP_Text text, float targetAlpha, int index)
    {
        float startAlpha = text.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, targetAlpha);
        runningCoroutines.Remove(index);
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < textList.Count;
    }
}
