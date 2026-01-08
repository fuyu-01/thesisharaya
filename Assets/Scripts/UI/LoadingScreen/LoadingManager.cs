using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    public CanvasGroup fadePanel;
    public float fadeTime = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeLoad(sceneName));
    }

    IEnumerator FadeLoad(string sceneName)
    {
        // Fade in
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }

        // Actually load the scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade out
        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }
    }
}
