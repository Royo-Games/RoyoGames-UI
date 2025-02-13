using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIPanel))]
public class SceneLoader : Singleton<SceneLoader>
{
    [Space]
    [SerializeField] UnityEvent<float> onProggressChangedEvent;
    public UIPanel Panel { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Panel = GetComponent<UIPanel>();
    }
    public void LoadAndAutoPanelOpenClose(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadAndAutoPanelOpenCloseCoroutine(sceneName, loadMode));
    }
    public void LoadAndAutoPanelOpenClose(int sceneIndex, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadAndAutoPanelOpenCloseCoroutine(sceneIndex, loadMode));
    }
    public IEnumerator LoadAndAutoPanelOpenCloseCoroutine(int sceneIndex, LoadSceneMode loadMode)
    {
        yield return Panel.ShowCoroutine();
        yield return LoadCoroutine(sceneIndex, LoadSceneMode.Single);
        yield return new WaitForSeconds(0.5f);
        yield return Panel.HideCoroutine();
    }
    public IEnumerator LoadAndAutoPanelOpenCloseCoroutine(string sceneName, LoadSceneMode loadMode)
    {
        yield return Panel.ShowCoroutine();
        yield return LoadCoroutine(sceneName, LoadSceneMode.Single);
        yield return new WaitForSeconds(0.5f);
        yield return Panel.HideCoroutine();
    }
    public void Load(int sceneIndex, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadCoroutine(sceneIndex, loadMode));
    }
    public void Load(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadCoroutine(sceneName, loadMode));
    }
    public IEnumerator LoadCoroutine(int sceneIndex, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        yield return ILoad(SceneManager.LoadSceneAsync(sceneIndex, loadMode));
    }
    public IEnumerator LoadCoroutine(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        yield return ILoad(SceneManager.LoadSceneAsync(sceneName, loadMode));
    }
    private IEnumerator ILoad(AsyncOperation asyncOperation)
    {
        SetProgress(0);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            SetProgress(progress);
            yield return null;
        }
    }
    public void SetProgress(float progress)
    {
        onProggressChangedEvent.Invoke(progress);
    }
}
