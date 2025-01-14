using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ScoreboardEntriesTable
{
    public ScoreboardEntriesTable(List<ScoreboardEntry> entries)
    {
        this.entries = entries; 
    }
    public List<ScoreboardEntry> entries = new List<ScoreboardEntry>();
}

public class Scoreboard : MonoBehaviour, ICommandTranslator
{
    [SerializeField] private ScoreboardView scoreboardView;
    [SerializeField] private int maxEntries;
    private List<ScoreboardEntry> entries = new List<ScoreboardEntry>();

    public event Action<ScoreboardEntry> OnEntryAdded;


    private void Start()
    {
        GameSession.Instance.AddCommandTranslator(this);
        string jsonScoreboardEntries = PlayerPrefs.GetString("ScoreboardEntriesTableTest"); //Binary file
        ScoreboardEntriesTable entriesTable = JsonUtility.FromJson<ScoreboardEntriesTable>(jsonScoreboardEntries);
        if (entriesTable == null)
            return;
        if (entriesTable.entries == null)
            return;
        for (int i = 0; i < entriesTable.entries.Count; i++)
        {
            entries.Add(entriesTable.entries[i]);
            OnEntryAdded?.Invoke(entriesTable.entries[i]);
        }
        
        // Sort scores descending then creates cards (invert this order cause issues) 
        entries.Sort((x, y) => y.Score.CompareTo(x.Score));
        scoreboardView.AddEntries(entries.GetRange(0, maxEntries));
    }

    public void AddScoreboardEntry(string entryName, int entryScore)
    {
        ScoreboardEntry entry = new ScoreboardEntry(entryName, entryScore);
        entries.Add(entry);
        OnEntryAdded?.Invoke(entry);
    }

    public void SortScoreboardEntriesByHighscore(List<ScoreboardEntry> entries)
    {
        entries.Sort((x,y) => y.Score.CompareTo(x.Score));
    }

    public void SortScoreboardCardsDatasByHighscore(List<PlayerScoreboardCardData> scoreboardCardDatas)
    {
        scoreboardCardDatas.Sort((x, y) => y.playerScore.CompareTo(x.playerScore));
    }

    public void AddScoreboardEntry(ScoreboardEntry entry)
    {
        entries.Add(entry);
        OnEntryAdded?.Invoke(entry);
        SaveScoreboardEntriesTable();
    }   

    public void SaveScoreboardEntriesTable()
    {
        ScoreboardEntriesTable scoreboardEntriesTable = new ScoreboardEntriesTable(entries);
        string jsonScoreboardEntries = JsonUtility.ToJson(scoreboardEntriesTable);
        PlayerPrefs.SetString("ScoreboardEntriesTableTest", jsonScoreboardEntries);
        PlayerPrefs.Save();
        
    }

    public void TranslateCommand(ECommand command, PressedState state)
    {
        switch (command)
        {
            case ECommand.OPEN_SCOREBOARD:
                if (state.IsPressed == true)
                    scoreboardView.Show(true);
                if (state.IsReleased == true)
                    scoreboardView.Show(false);
                break;
            default:
                scoreboardView.Show(false);
                break;
        }
    }
}
