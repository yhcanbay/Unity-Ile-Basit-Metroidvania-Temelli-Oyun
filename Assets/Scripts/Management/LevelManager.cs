using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField]
    private float fadeDuration;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeToTranparent());
    }

    public void RestartGame()
    {
        StartCoroutine(FadeToBlackInt(SceneManager.GetActiveScene().buildIndex));
    }

    public void ChangeLevel(string levelName)
    {
        StartCoroutine(FadeToBlackString(levelName));
    }

    private IEnumerator FadeToTranparent()
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 0;

    }

    private IEnumerator FadeToBlackInt(int buildIndex)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 1;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 1;
        SceneManager.LoadScene(buildIndex);
    }

    private IEnumerator FadeToBlackString(string levelName)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 1;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 1;
        SceneManager.LoadScene(levelName);
    }
}
