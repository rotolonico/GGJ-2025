using UnityEngine;

public static class DiffucltyManager
{
    public enum Difficulty
    {
        Normal,
        Crazy
    }
    
    public static Difficulty CurrentDifficulty { get; private set; } = Difficulty.Normal;
    
    public static void SetDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
    }
    
    public static string GetCurrentDifficulty()
    {
        switch (CurrentDifficulty)
        {
            case Difficulty.Normal:
                return "Bubble (Normal)";
            case Difficulty.Crazy:
                return "Foam (Crazy)";
            default:
                return "Unknown";
        }
    }
}
