
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using GameLogging;

[Serializable]
public class PlayerStats {

    public Dictionary<string, Dictionary<string, Stat>> Stats { get; private set; }
    public float OSTVolume { get; set; } = .8f;
    public float SFXVolume { get; set; } = .8f;

    public PlayerStats() {
        try {
            Stats = PlayerIO.LoadData().Stats;
            OSTVolume = PlayerIO.LoadData().OSTVolume;
            SFXVolume = PlayerIO.LoadData().SFXVolume;
            Debug.Log("Player history detected");
        } catch (NoSaveFileException) {
            MiniGameLister MGL = Zombie.MiniGameList;
            Stats = new Dictionary<string, Dictionary<string, Stat>>();
            foreach(string Category in MGL.GetListofCategories()) {
                Dictionary<string, Stat> DataStruct = new Dictionary<string, Stat>();
                foreach(string MiniGame in MGL.GetSceneNamesFromCategory(Category)) {
                    DataStruct.Add(MiniGame, new Stat());
                }
                Stats.Add(Category, DataStruct);
            }        
            PlayerIO.SaveData(this);
            Debug.Log("New Player Record Created");
        }
    }

}

[Serializable]
public class Stat {

    public float BestTime { get; set; } = 0;
    public float BestScore { get; set; } = 0;
    public int GamesPlayed { get; set; } = 0;
    public int GamesWon { get; set; } = 0;
    public int GamesLost { get; set; } = 0;
    public int TimesPracticed { get; set; } = 0;
    public int TimesPlayed { get; set; } = 0;

    /// <summary>A Log of Games Played</summary>
    public GameLogs GameLog { get; } = new GameLogs();

    public Stat() {
        GameLog.ListChanged += (sender, e) => {
            if(e.TimeTaken < BestTime) { BestTime = e.TimeTaken; }
            if(e.Score > BestScore) { BestScore = e.Score; }
            if (e.Win && !e.IsPractice) { GamesWon++; } else { GamesLost++; }
            if (e.IsPractice) { TimesPracticed++; } else { TimesPlayed++; }
            GamesPlayed++;
        };
    }
    
}

namespace GameLogging {

    [Serializable]
    public class GameLogs: List<Log> {

        public event EventHandler<Log> ListChanged;

        public new void Add(Log item) {
            if(ListChanged != null && item != null) { ListChanged.Invoke(this, item); }
            base.Add(item);
        }

    }

    [Serializable]
    public class Log {

        public DateTime Date { get; set; }
        public int TurnsUsed { get; set; } = -1;
        public int ErrorsMade { get; set; } = 0;
        public float Score { get; set; } = -1;
        public float TimeTaken { get; set; } = -1;
        public bool IsTeamGame { get; set; } = false;
        public bool IsPractice { get; set; } = false;
        public Difficulty Difficulty { get; set; } = Difficulty.Normal;
        public bool Win { get; set; } = false;
    }
}

[Serializable]
public enum Difficulty { Easy, Normal, Hard, Expert }

public static class PlayerIO {

    private static readonly string Path = Application.persistentDataPath + "/History.survive";
    private static readonly BinaryFormatter Serializer = new BinaryFormatter();
    private static readonly string PathExport = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Super Data Structure Adventure Stats.csv";


    public static void SaveData(PlayerStats player) {
        using FileStream file = new FileStream(Path, FileMode.Create);
        Serializer.Serialize(file, player);
    }

    public static PlayerStats LoadData() {
        if (File.Exists(Path)) {
            try {
                using FileStream file = new FileStream(Path, FileMode.Open);
                return Serializer.Deserialize(file) as PlayerStats;
            } catch {
                throw new NoSaveFileException();
            }
        } else {
            Debug.Log("File was not found " + Path);
            throw new NoSaveFileException();
        }
    }

    public static void ExportData(PlayerStats Player) {
        if (File.Exists(PathExport)) { File.SetAttributes(PathExport, File.GetAttributes(PathExport) & ~FileAttributes.ReadOnly); }
        using FileStream file = new FileStream(PathExport, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        string ser = "Key\n";
        ser += "Category Name\n";
        ser += "Game Name\n";
        ser += "Times Played,Games Won,Games Lost,Best Time,Best Score,Times Practiced\n";
        ser += "{Individual Logs}\n";
        ser += "Practice Game,Online Game,Win,Score,Time Taken, Turns Used, Errors Made, Difficulty\n\n";
        foreach (string s in Player.Stats.Keys) {
            ser += $"\n{s}\n";
            foreach (string g in Player.Stats[s].Keys) {
                ser += $"\n{g}\n{Player.Stats[s][g].TimesPlayed},{Player.Stats[s][g].GamesWon},{Player.Stats[s][g].GamesLost},{Player.Stats[s][g].BestTime}," +
                    $"{Player.Stats[s][g].BestScore},{Player.Stats[s][g].TimesPracticed}\n{{Individual Logs}}\n";
                foreach(Log log in Player.Stats[s][g].GameLog) {
                    ser += $"{log.IsPractice},{log.IsTeamGame},{log.Win},{log.Score},{log.TimeTaken},{log.TurnsUsed},{log.ErrorsMade},{log.Difficulty}\n";
                }
            }
        }
        byte[] data = new System.Text.UTF8Encoding(true).GetBytes(ser);
        file.Write(data, 0, data.Length);
        File.SetAttributes(PathExport, FileAttributes.ReadOnly);
    }
}
class NoSaveFileException : Exception { }