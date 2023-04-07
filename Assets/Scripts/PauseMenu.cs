using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu, gameOverMenu, crosshair, gameClearedMenu, gameControlsMenu; 
    public static bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverMenu.activeSelf)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame(false);
            }
        }
    }

    public void PauseGame(bool flag)
    {
        AudioListener.pause = true;
        isPaused = true;
        crosshair.SetActive(false);
        if (GameObject.FindWithTag("cleared") && flag)
            gameClearedMenu.SetActive(true);
        else if (GameObject.Find("Player").GetComponent<ObjectHealth>().GetHealth() > 0)
            pauseMenu.SetActive(true);
        else
            gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;
        isPaused = false;
        crosshair.SetActive(true);
        pauseMenu.SetActive(false);
        if (gameControlsMenu.activeSelf)
            gameControlsMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}
