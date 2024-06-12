using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneManagerAsync : MonoBehaviour
{
    [SerializeField] public string _sceneToLoad;
    [SerializeField] Slider slider;
    [SerializeField] GameObject _menuUI, _loadingUI;
    [SerializeField] TextMeshProUGUI _progressText;

    public void LoadSelectedScene(string sceneToLoad)
    {
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    IEnumerator LoadSceneAsync( string sceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        _menuUI.SetActive(false);
        _loadingUI.SetActive(true);
        float progress = 0;
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);

            Debug.Log($"Operation progress is: {progress}");
            slider.value = progress;
            _progressText.text = ($"{progress * 100}%");
            yield return null;
        }
    }
}
