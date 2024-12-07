using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PageData
{
    public GameObject page; // Page GameObject
    public float pageWaitTime = 1f; // Time to wait before starting to display images on the page
    public float imageWaitTime = 0.5f; // Time to wait between each image
    public float pageDuration = 5f; // Total time the page should last after all images are shown
}

public class ComicController : MonoBehaviour
{
    public List<PageData> pages; // List of pages with their data
    public float fadeDuration = 1f; // Duration for fade-in effects
    public string nextSceneName; // Name of the next scene

    public int currentPage = 0;

    void Start()
    {
        StartCoroutine(ShowComic());
    }

    private IEnumerator ShowComic()
    {
        while (currentPage < pages.Count)
        {
            PageData pageData = pages[currentPage];

            // Wait for the pageWaitTime before starting
            yield return new WaitForSeconds(pageData.pageWaitTime);

            // Display the page
            yield return DisplayPage(pageData);

            // Wait for the page duration after all images are shown
            yield return new WaitForSeconds(pageData.pageDuration);

            // Disable the page
            pageData.page.SetActive(false);

            currentPage++;
        }

        // Transition to the next scene after all pages
        LoadNextScene();
    }

    private IEnumerator DisplayPage(PageData pageData)
    {
        // Activate the page
        GameObject page = pageData.page;
        page.SetActive(true);

        // Display images one by one
        Transform imagesParent = page.transform;
        for (int i = 0; i < imagesParent.childCount; i++)
        {
            GameObject image = imagesParent.GetChild(i).gameObject;

            // Fade in the image
            yield return FadeInImage(image);

            // Wait before showing the next image
            if (i < imagesParent.childCount - 1) // Skip wait for the last image
            {
                yield return new WaitForSeconds(pageData.imageWaitTime);
            }
        }
    }

    private IEnumerator FadeInImage(GameObject image)
    {
        CanvasGroup canvasGroup = image.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = image.AddComponent<CanvasGroup>();
        }

        image.SetActive(true);
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
