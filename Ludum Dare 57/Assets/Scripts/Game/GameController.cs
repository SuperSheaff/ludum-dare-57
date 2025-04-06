using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    [Header("Timer UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text winText;
    private float timer         = 0f;
    private bool timerRunning   = false;

    [Header("Teleport Upgrade")]
    [SerializeField] private GameObject teleportUpgradePrefab;
    [SerializeField] private Transform teleportUpgradeSpawnPoint;
    private GameObject activeTeleportUpgrade;

    [Header("Text Group")]
    [SerializeField] private Transform textGroup;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    [Header("VFX")]
    public ParticleSystem winParticles;

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

        if (winText != null)
            winText.text = "NEARLY THERE, LETS GO!";

        CacheTextObjects();

        ResetGame();
    }

    private void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
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

    public void StartTimer()
    {
        timer = 0f;
        timerRunning = true;
        Debug.Log("Timer started");
    }

    public void StopTimer()
    {
        timerRunning = false;
        Debug.Log("Timer stopped at: " + timer.ToString("F2") + " seconds");
        // You can trigger an event here or call a win screen, etc.
    }

    public float GetElapsedTime()
    {
        return timer;
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int milliseconds = Mathf.FloorToInt((timer * 100f) % 100f); // Two-digit MS
        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    public void WinGame()
    {
        StopTimer();

        if (winText != null)
            winText.text = "YOU WON!\nPRESS R TO RESTART";

        winParticles.Play();
        PlayerController.instance?.EnterWinState();
    }

    public void ResetGame()
    {
        timer = 0f;
        UpdateTimerText();
        timerRunning = false;

        SoundManager.instance.PlaySound("restart", this.transform);

        if (winText != null)
            winText.text = "NEARLY THERE, LETS GO!";

        PlayerController.instance?.ResetToStart();

        // Respawn the upgrade
        if (teleportUpgradePrefab != null && teleportUpgradeSpawnPoint != null && activeTeleportUpgrade == null)
        {
            activeTeleportUpgrade = Instantiate(teleportUpgradePrefab, teleportUpgradeSpawnPoint.position, Quaternion.identity);
        }

        if (winParticles != null)
        {
            winParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void ClearTeleportUpgradeReference()
    {
        activeTeleportUpgrade = null;
    }

}
