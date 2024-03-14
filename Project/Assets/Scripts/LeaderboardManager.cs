using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int score;
    }

    public List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    public int maxEntries = 10; 
    private string filePath;
    public TMP_Text LeaderboardDataText;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.json"); //C:\Users\user\AppData\LocalLow\DefaultCompany\Raindrops
        LoadLeaderboard();
    }

    
    public void SaveEntry()
    {
        AddEntry(Player.Instance.nickname, Player.Instance.score);
    }

    
    public void AddEntry(string playerName, int score)
    {
        
        if (leaderboardEntries.Count < maxEntries || score > leaderboardEntries[leaderboardEntries.Count - 1].score)
        {
            LeaderboardEntry newEntry = new LeaderboardEntry { playerName = playerName, score = score };
            leaderboardEntries.Add(newEntry);
            leaderboardEntries.Sort((a, b) => b.score.CompareTo(a.score));

            if (leaderboardEntries.Count > maxEntries)
            {
                leaderboardEntries.RemoveAt(leaderboardEntries.Count - 1);
            }

            SaveLeaderboard();
        }
    }

    
    public void SaveLeaderboard()
    {
        LeaderboardData data = new LeaderboardData(leaderboardEntries);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    
    public void LoadLeaderboard()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            leaderboardEntries = data.entries;
        }
        else
        {
            SaveLeaderboard();
        }
    }

    public List<LeaderboardEntry> GetTopEntries(int count)
    {
        return leaderboardEntries.OrderByDescending(entry => entry.score).Take(count).ToList();
    }
    public void UpdateLeaderboardText()
    {
        List<LeaderboardEntry> topEntries = GetTopEntries(maxEntries);
        string leaderboardString = "Leaderboard:\n";

        foreach (LeaderboardEntry entry in topEntries)
        {
            leaderboardString += $"{entry.playerName}: {entry.score}\n";
        }

        LeaderboardDataText.text = leaderboardString;
    }

    [System.Serializable]
    private class LeaderboardData
    {
        public List<LeaderboardEntry> entries;

        public LeaderboardData(List<LeaderboardEntry> entries)
        {
            this.entries = entries;
        }
    }
}
