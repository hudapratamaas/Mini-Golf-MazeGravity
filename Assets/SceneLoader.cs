using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{

    private static string sceneToLoad;

    public static string SceneToLoad { get => sceneToLoad;}

    //Load
    public static void Load( string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    //Progres Load
    public static void ProgressLoad( string sceneName)
    {
        sceneToLoad = sceneName;
        //Debug.Log("LoadingProgress sceneName");
        SceneManager.LoadScene("LoadingProgress");
    }

    //Reload Level
    public static void ReloadLevel()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        ProgressLoad(currentScene);
    }

    //Load Next Level
    public static void LoadNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int nextLevel =  int.Parse(currentScene.Split("Level")[1]) + 1;
        string nextSceneName = "level" + nextLevel;

        if(SceneUtility.GetBuildIndexByScenePath(nextSceneName) == - 1)
        {
            Debug.LogError(nextSceneName+" does not exits");
            return;
        }

        ProgressLoad(nextSceneName);
    }
}
