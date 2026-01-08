using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroVideoController : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;
    public Image fadeImage;
    public string nextSceneName = "MainMenu";

    [Header("Timing")]
    public float fadeDuration = 1.5f; // seconds

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // Fade from black to clear (fade-in)
        yield return StartCoroutine(Fade(1, 0));

        // Wait until the video ends
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        // Fade back to black
        yield return StartCoroutine(Fade(0, 1));

        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;
            yield return null;
        }
    }
}
