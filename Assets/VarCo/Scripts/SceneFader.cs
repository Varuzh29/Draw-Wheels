using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


namespace VarCo
{
    public static class SceneFader
    {
        private const int SORTING_ORDER = 32767; // max 32767
        private static Image image;
        private static bool isBusy = false;

        private static Image CreateImage()
        {
            // Create canvas GameObject
            GameObject canvasObject = new GameObject($"{nameof(SceneFader)} Canvas");
            UnityEngine.Object.DontDestroyOnLoad(canvasObject);
            canvasObject.AddComponent<RectTransform>();
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            // Setup canvas
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.sortingOrder = SORTING_ORDER;

            // Create image
            GameObject imageObject = new GameObject($"{nameof(SceneFader)} Image");
            imageObject.SetActive(false);
            RectTransform imageTransform = imageObject.AddComponent<RectTransform>();
            imageObject.AddComponent<CanvasRenderer>();
            Image image = imageObject.AddComponent<Image>();

            // Setup Image
            imageTransform.SetParent(canvasObject.transform);
            imageTransform.anchorMax = Vector2.one;
            imageTransform.anchorMin = Vector2.zero;
            imageTransform.pivot.Set(0.5f, 0.5f);
            imageTransform.offsetMax = imageTransform.offsetMin = Vector2.zero;
            image.color = Color.black;

            return image;
        }

        public static void Initialize()
        {
            image = CreateImage();
        }

        private static void LoadSequence(int buildIndex, float duration)
        {
            isBusy = true;
            image.gameObject.SetActive(true);
            image.DOFade(0, 0).SetUpdate(true);
            Sequence sequence = DOTween.Sequence();
            sequence
                .SetUpdate(true)
                .Append(image.DOFade(1, duration))
                .AppendCallback(() => SceneManager.LoadScene(buildIndex))
                .Append(image.DOFade(0, duration))
                .AppendCallback(() => isBusy = false)
                .AppendCallback(() => image.gameObject.SetActive(false));
        }

        public static void LoadScene(int buildIndex, float fadeDuration = 0.5f)
        {
            if (isBusy)
            {
                Debug.LogWarning("SceneFader is busy now!");
                return;
            }
            if (image == null) Initialize();
            LoadSequence(buildIndex, fadeDuration);
        }

        public static void LoadScene(string sceneName, float fadeDuration = 0.5f)
        {
            if (isBusy)
            {
                Debug.LogWarning("SceneFader is busy now!");
                return;
            }
            if (image == null) Initialize();
            LoadSequence(SceneManager.GetSceneByName(sceneName).buildIndex, fadeDuration);
        }

        public static void LoadNextScene(float fadeDuration = 0.5f)
        {
            int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
            LoadScene(nextSceneBuildIndex, fadeDuration);
        }

        public static void LoadPreviousScene(float fadeDuration = 0.5f)
        {
            int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex - 1;
            LoadScene(nextSceneBuildIndex, fadeDuration);
        }

        public static void ReloadScene(float fadeDuration = 0.5f)
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex, fadeDuration);
        }
    }
}
