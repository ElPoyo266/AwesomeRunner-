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
    private IInputTranslator _player1InputTranslator;
    private IInputTranslator _player2InputTranslator;

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
       _player1InputTranslator.Tick();
       _player2InputTranslator.Tick();

       Curver.Tick();
        // curver.SinCurveX();
        // Curver.SinCurveY();
       //Curver.TurnWorldToLeft();
    }

    private void Init()
    {
        var player1Holder = new KeyBindingHolder();
        player1Holder.Init(false);
        _player1InputTranslator = new InputTranslator<KeyBinding>(player1Holder, false);

        var player2Holder = new KeyBindingHolder();
        player2Holder.Init(true);
        _player2InputTranslator = new InputTranslator<KeyBinding>(player2Holder, true);
    }

    public void AddCommandTranslator(ICommandTranslator translator, bool isPlayer2)
    {
        if (isPlayer2)
            _player2InputTranslator.AddCommandTranslator(translator);
        else
            _player1InputTranslator.AddCommandTranslator(translator);
    }

  
    public void PauseSession(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        if (!isSessionPaused && _player1InputTranslator.IsTranslationResticted(InputConstants.InGameCommands) && _player2InputTranslator.IsTranslationResticted(InputConstants.InGameCommands) )
        {
            isInputAlreadyRestricted = true;
            isSessionPaused = isPaused;
            return;
        }
        if (!_player1InputTranslator.IsTranslationResticted(InputConstants.InGameCommands) && !_player2InputTranslator.IsTranslationResticted(InputConstants.InGameCommands))
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
        _player1InputTranslator.RestictTranslation(commands, isRestricted);
        _player2InputTranslator.RestictTranslation(commands, isRestricted);

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