using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(WorldCurver))]  
public class GameSession : MonoBehaviour,IResettable
{
    [SerializeField] private Player currentPlayer;
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private Boolean isPlayer2;

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
        IBindingHolder<KeyBinding> keyHolder = new KeyBindingHolder();
        inputTranslator = new InputTranslator<KeyBinding>(keyHolder, isPlayer2);
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

    public void GoToSoloMode()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        ResetToDefault();
    }
    
    public void GoToDuoMode()
    {
        SceneManager.LoadScene("DuoGameScene", LoadSceneMode.Single);
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
        currentPlayer.ResetToDefault();
    }
}