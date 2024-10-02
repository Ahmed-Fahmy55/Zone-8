using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour
{
    public Text text;
    public static SceneController instance;
    public List<string> sceneName = new List<string>();
    public int currentLevelNumb;
    public string currentSceneName;
    public static event Action LevelStart;
    public static event Action LevelEnd;


    private void Start()
    {

        instance = this;
        currentSceneName = sceneName[0];
        currentLevelNumb = 0;
        LevelAdvancePanel.FadeInToEndLevel(LoadLevel);

    }
    private void Update()
    {
        text.text = "Level " + (currentLevelNumb + 1);
    }

    void LoadLevel()
    {
        LoadLevel(-1);
    }
    void LoadLevel(int Numb)
    {
        if (Numb == -1)
        {
            Numb = currentLevelNumb;
        }
        currentLevelNumb = Numb;
        currentSceneName = sceneName[currentLevelNumb];

        StartCoroutine(LoadScene(currentSceneName));
    }

    IEnumerator LoadScene(string name)
    {
        if (SceneManager.sceneCount > 1)
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        InteractingPlayer.SetPosition(new Vector3(3000f, 3000f, 3000f), Quaternion.identity);

        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        Scene newLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newLoadedScene);
        LevelAdvancePanel.FadeOutToBeginLevel(StartLevel);

    }

    public void StartLevel()
    {
        AlertModeManager.SwitchToAlertMode(false);
        GameObject obj = GameObject.Find("PlayerStart");
        if (obj != null)
        {
            InteractingPlayer.SetPosition(obj.transform.position, obj.transform.rotation);

            StealthPlayerCamera.ResetToFarPosition();
        }
        LevelStart?.Invoke();
    }

    public void EndLEvel()
    {

        currentLevelNumb++;
        LevelEnd?.Invoke();
        LevelAdvancePanel.FadeInToEndLevel(LoadLevel);
    }
    public void ReloadLevel()
    {
        currentLevelNumb--;
        EndLEvel();
    }
}
