using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenuScript : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;
  


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isGamePaused = false;
        Cursor.visible = false;
    }
    public void Pause()
    {
        
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        isGamePaused = true;
    }
    public void LoadMainMenu()
    {
        isGamePaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
