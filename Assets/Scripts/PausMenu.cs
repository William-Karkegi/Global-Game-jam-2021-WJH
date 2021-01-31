using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject pauseMenuUI2;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused){
                Cursor.lockState = CursorLockMode.Locked;
                Resume();
            } else {
                Cursor.lockState = CursorLockMode.None;
                Pause();
            }
        }
    }
    public void Resume ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        pauseMenuUI2.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause ()
    {
         pauseMenuUI.SetActive(true);
         pauseMenuUI2.SetActive(false);
         Time.timeScale = 0f;
         GameIsPaused = true;
    }

    public void LoadMenu ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame ()
    {
        Debug.Log("Quitting menu...");
        Application.Quit();
    }
}
