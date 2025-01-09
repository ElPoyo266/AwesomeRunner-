using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(WorldCurver))]  
public class GameSession : MonoBehaviour,IResettable
{
    [SerializeField] private Player currentPlayer;
    [SerializeField] private Scoreboard scoreboard;
    public static GameSession Instance { get; private set; } 
    public WorldCurver Curver { get; private set; }
    private IInputTranslator inputTranslator;

    private bool isSessionPaused = false;
    private bool isInputAlreadyRestricted = false;

    private void Awake()
    {
        Instance = this;
        Init();
        Curver = GetComponent<WorldCurver>();
    }

    private void Start()
    {
        if (ApplicationUtil.platform == RuntimePlatform.Android || ApplicationUtil.platform == RuntimePlatform.IPhonePlayer)
        {
            //Application.targetFrameRate = 60;
        }
    }
    private void Update()
    {
       inputTranslator.Tick();
       Curver.Tick();
        // curver.SinCurveX();
        // Curver.SinCurveY();
       //Curver.TurnWorldToLeft();
    }

    private void Init()
    {
        if (ApplicationUtil.platform == RuntimePlatform.Android || ApplicationUtil.platform == RuntimePlatform.IPhonePlayer)
        {
            IBindingHolder<TouchBinding> touchHolder = new TouchBindingHolder();
            inputTranslator = new InputTranslator<TouchBinding>(touchHolder);
        }
        else
        {
            IBindingHolder<KeyBinding> keyHolder = new KeyBindingHolder();
            inputTranslator = new InputTranslator<KeyBinding>(keyHolder);
        }
    }

    public void AddCommandTranslator(ICommandTranslator translator)
    {
        inputTranslator.AddCommandTranslator(translator);
    }

  
    public void PauseSession(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        if (!isSessionPaused && inputTranslator.IsTranslationResticted(InputConstants.InGameCommands))
        {
            isInputAlreadyRestricted = true;
            isSessionPaused = isPaused;
            return;
        }
        if (!inputTranslator.IsTranslationResticted(InputConstants.InGameCommands))
        {
            isInputAlreadyRestricted = false;
        }
        isSessionPaused = isPaused;
        if (isInputAlreadyRestricted)
        {
            return;
        }   
        RestrictInputs(InputConstants.InGameCommands,isRestricted: isPaused);
    }

    public void RestrictInputs(List<ECommand> commands,bool isRestricted)
    {
        inputTranslator.RestictTranslation(commands, isRestricted);
    }

    public void UpdateScoreboard(ScoreboardEntry entry)
    {
        scoreboard.AddScoreboardEntry(entry);
    }

    public void GoToGameScene()
    {
        StartCoroutine(LoadGameSceneAsync());
    }

    private IEnumerator LoadGameSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            // Indiquer la progression de chargement (0 à 0.9)
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log($"Loading progress: {progress * 100}%");

            // Activer la scène lorsqu'elle est prête
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
        ResetToDefault();
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        ResetToDefault();
    }

    public void ResetToDefault()
    {
        PauseSession(false);
        if(currentPlayer !=null)
            currentPlayer.ResetToDefault();
    }
}