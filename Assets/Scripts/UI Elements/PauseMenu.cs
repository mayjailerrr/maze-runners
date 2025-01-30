using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Asegúrate de incluir esto

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Asigna el Panel del menú de pausa aquí
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);

        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(Exit);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
}
