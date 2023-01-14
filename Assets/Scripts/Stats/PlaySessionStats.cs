using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObject/Stats")]
public class PlaySessionStats : ScriptableObject
{
    public StatsType stats;
    PlaySessionStats data;

    [Header("Kicks")]
    public int kicksDone;

    [Header("Goals")]
    public int goals;

    [Header("Kicks by Distance")]
    public int longKicks;
    public int mediumKicks;
    public int shortKicks;

    [Header("Goals by Distance")]
    public int longGoals;
    public int mediumGoals;
    public int shortGoals;

    [Header("Kicks by Side")]
    public int kicksOnLeft;
    public int kicksOnRight;
    public int kicksOnCenter;

    [Header("Goals by Side")]
    public int goalsOnLeft;
    public int goalsOnRight;
    public int goalsOnCenter;

    public enum StatsType
    {
        General,
        Session,
        ByPlay
    }

    public void ResetStats()
    {
        switch (stats)
        {
            case StatsType.Session:
                data = Resources.Load<PlaySessionStats>("Stats/SessionStats");
                break;
            case StatsType.General:
                data = Resources.Load<PlaySessionStats>("Stats/GeneralStats");
                break;
        }

        kicksDone = 0;
        goals = 0;

        longKicks = 0;
        mediumKicks = 0;
        shortKicks = 0;

        longGoals = 0;
        mediumGoals = 0;
        shortGoals = 0;

        kicksOnLeft = 0;
        kicksOnRight = 0;
        kicksOnCenter = 0;

        goalsOnLeft = 0;
        goalsOnRight = 0;
        goalsOnCenter = 0;
    }

    public void SendStats(PlaySessionStats _stats)
    {
        _stats.kicksDone += this.kicksDone;
        _stats.goals += this.goals;

        _stats.longKicks += this.longKicks;
        _stats.mediumKicks += this.mediumKicks;
        _stats.shortKicks += this.shortKicks;

        _stats.longGoals += this.longGoals;
        _stats.mediumGoals += this.mediumGoals;
        _stats.shortGoals += this.shortGoals;

        _stats.kicksOnLeft += this.kicksOnLeft;
        _stats.kicksOnRight += this.kicksOnRight;
        _stats.kicksOnCenter += this.kicksOnCenter;

        _stats.goalsOnLeft += this.goalsOnLeft;
        _stats.goalsOnRight += this.goalsOnRight;
        _stats.goalsOnCenter += this.goalsOnCenter;
    }
}