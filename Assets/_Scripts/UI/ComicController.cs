using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management

public class ComicController : MonoBehaviour
{
    public GameObject[] pages; // Array of page GameObjects
    public List<float> pageDisplayTimes; // List of display times for each page
    public float fadeDuration = 1f; // Duration for the fade effect
    public float fadeOutDuration = 0.5f; // Duration for the fade-out effect
    public string nextSceneName; // Name of the next scene
    private int currentPage = 0;
    private Coroutine autoPaginationCoroutine;

    void Start()
    {
        ShowPage(0); // Show the first page with fade-in
        StartAutoPagination(); // Start the auto-pagination process
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage(true); // Stop auto-pagination if manually navigating
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
    }

    public void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }

        StartCoroutine(FadeInPage(pages[index]));
    }

    public void NextPage(bool stopAuto = false)
    {
        if (stopAuto && autoPaginationCoroutine != null)
        {
            StopCoroutine(autoPaginationCoroutine);
            autoPaginationCoroutine = null;
        }

        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
        else
        {
            StartCoroutine(FadeOutLastPage());
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    private void StartAutoPagination()
    {
        if (autoPaginationCoroutine != null)
        {
            StopCoroutine(autoPaginationCoroutine);
        }
        autoPaginationCoroutine = StartCoroutine(AutoPaginate());
    }

    private IEnumerator AutoPaginate()
    {
        while (currentPage < pages.Length)
        {
            yield return new WaitForSeconds(pageDisplayTimes[currentPage]);
            NextPage();
        }
    }

    private IEnumerator FadeInPage(GameObject page)
    {
        CanvasGroup canvasGroup = page.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = page.AddComponent<CanvasGroup>();
        }

        page.SetActive(true);
        canvasGroup.alpha = 0;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = elapsedTime / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutLastPage()
    {
        GameObject lastPage = pages[pages.Length - 1];
        CanvasGroup canvasGroup = lastPage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = lastPage.AddComponent<CanvasGroup>();
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - (elapsedTime / fadeOutDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        lastPage.SetActive(false);

        LoadNextScene(); // Transition to the next scene
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set!");
        }
    }
}
