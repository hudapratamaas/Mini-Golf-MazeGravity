using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text gameOvertext;
    [SerializeField] Hole hole;
    [SerializeField] PlayerController player;

    private void Start()
    {
        //gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if(hole.Entered && gameOverPanel.activeInHierarchy == false)
        {
            gameOverPanel.SetActive(true);
           // gameOvertext.text = "Finished!" + player.ShootCount; 
            gameOvertext.text = "Finished!!!";
        }
    }
    public void BackToMainMenu()
    {
        SceneLoader.Load("MainMenu");
    }
    public void Replay()
    {
        SceneLoader.ReloadLevel();
    }
    public void PlayNext()
    {
        SceneLoader.LoadNextLevel();
    }
}
