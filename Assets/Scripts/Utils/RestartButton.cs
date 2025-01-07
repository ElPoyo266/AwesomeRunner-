using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    [SerializeField] public Button restartButton;

    public void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogError("RestartButton is not assigned in the inspector!");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
