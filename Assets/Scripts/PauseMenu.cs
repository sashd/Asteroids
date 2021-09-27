using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{   

    static bool firstLaunch = true;

    [SerializeField] private GameObject panel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Text changeInputText;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameManager gameManager;

    private  bool isPaused = true;
    
    private void Start()
    {
        if (firstLaunch)
        {
            Pause();
            resumeButton.interactable = false;
            firstLaunch = false;
        }
        else
        {
            Resume();
        }
        ChangeText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void NewGame()
    {
        gameManager.Restart();
        Resume();
    }

    public void ChangeInput()
    {
        playerInput.ChangeInput();
        ChangeText();

    }

    public void ChangeText()
    {
        changeInputText.text = playerInput.UseMouse? "Mouse + Keyboard" : "Keyboard only";
    }

    public void Quit()
    {
        Application.Quit();
    }
}
