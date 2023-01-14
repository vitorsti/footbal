using System;
using UnityEngine;

public class DailyTask : MonoBehaviour
{
    [SerializeField] PlaySessionStats taskStats;

    [Header("Task Props")]
    public SpawnPositionsProps.SpawnPosSide Side;
    public SpawnPositionsProps.SpawnPosDistance Distance;
    public scoreType scoredType;
    public posType positionType;
    public difficult taskLevel;
    public bool completed;

    [Header("Type/Qtd Need")]
    int kicks;
    int goals;

    [Header("Constants")]
    [SerializeField] int easyKicksQtd = 5;
    [SerializeField] int mediumKicksQtd = 10;
    [SerializeField] int hardKicksQtd = 15;

    [SerializeField] int easyGoalsQtd = 3;
    [SerializeField] int mediumGoalsQtd = 5;
    [SerializeField] int hardGoalsQtd = 10;

    [Header("Constants Coins")]
    [SerializeField] int easyTaskReward = 50;
    [SerializeField] int normalTaskReward = 100;
    [SerializeField] int hardTaskReward = 150;

    [Header("Task Text")]
    public string description;


    public enum scoreType
    {
        Goals,
        Kicks
    }

    public enum posType
    {
        Distance,
        Side
    }

    public enum difficult
    {
        Easy,
        Normal,
        Hard
    }

    void SelectDifficult()
    {
        int rng = UnityEngine.Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                kicks = easyKicksQtd;
                goals = easyGoalsQtd;
                taskLevel = difficult.Easy;
                break;

            case 1:
                kicks = mediumKicksQtd;
                goals = mediumGoalsQtd;
                taskLevel = difficult.Normal;
                break;

            case 2:
                kicks = hardKicksQtd;
                goals = hardGoalsQtd;
                taskLevel = difficult.Hard;
                break;
        }
    }

    void Randomize()
    {
        var scoreT = (scoreType[])Enum.GetValues(typeof(scoreType));
        var posT = (posType[])Enum.GetValues(typeof(posType));
        var sides = (SpawnPositionsProps.SpawnPosSide[])Enum.GetValues(typeof(SpawnPositionsProps.SpawnPosSide));
        var dist = (SpawnPositionsProps.SpawnPosDistance[])Enum.GetValues(typeof(SpawnPositionsProps.SpawnPosDistance));

        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        int rng = UnityEngine.Random.Range(0, sides.Length);
        Side = sides[rng];

        rng = UnityEngine.Random.Range(0, dist.Length);
        Distance = dist[rng];

        rng = UnityEngine.Random.Range(0, scoreT.Length);
        scoredType = scoreT[rng];

        rng = UnityEngine.Random.Range(0, posT.Length);
        positionType = posT[rng];
    }

    // Start is called before the first frame update
    void Start()
    {
        taskStats = Resources.Load<PlaySessionStats>("Stats/TaskStats");
        SelectDifficult();
        Randomize();
        CreateText();
    }

    void CreateText()
    {
        string dist;
        int qtd = 0;
        
        switch (taskLevel)
        {
            case difficult.Easy:
                if (scoredType == scoreType.Goals)
                    qtd = easyGoalsQtd;
                else
                    qtd = easyKicksQtd;
                break;

            case difficult.Normal:
                if (scoredType == scoreType.Goals)
                    qtd = mediumGoalsQtd;
                else
                    qtd = mediumKicksQtd;
                break;

            case difficult.Hard:
                if (scoredType == scoreType.Goals)
                    qtd = hardGoalsQtd;
                else
                    qtd = hardKicksQtd;
                break;
        }

        if (Distance == SpawnPositionsProps.SpawnPosDistance.Long || Distance == SpawnPositionsProps.SpawnPosDistance.SemiLong)
            dist = "far from the goal";
        else if (Distance == SpawnPositionsProps.SpawnPosDistance.Medium)
            dist = "from regular distance";
        else
            dist = "near the goal";

        description = $"Take {qtd.ToString()} {scoredType.ToString()} {dist}";
    }

    public void VerifyCompleteTask()
    {
        if (!completed)
        {
            //Goals
            if (scoredType == scoreType.Goals)
            {
                //By Distance
                if (positionType == posType.Distance)
                {
                    switch (Distance)
                    {
                        case SpawnPositionsProps.SpawnPosDistance.Short:
                            if (taskStats.shortGoals >= goals)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosDistance.SemiShort:
                            if (taskStats.shortGoals >= goals)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosDistance.Medium:
                            if (taskStats.mediumGoals >= goals)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosDistance.Long:
                            if (taskStats.longGoals >= goals)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosDistance.SemiLong:
                            if (taskStats.longGoals >= goals)
                                completed = true;
                            return;
                    }
                }
                //By Side
                else
                {
                    switch (Side)
                    {
                        case SpawnPositionsProps.SpawnPosSide.Left:
                            if (taskStats.goalsOnLeft >= goals)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosSide.Right:
                            if (taskStats.goalsOnRight >= goals)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosSide.Center:
                            if (taskStats.goalsOnCenter >= goals)
                                completed = true;
                            return;
                    }
                }
            }

            // Kicks
            else
            {
                //By Distance
                if (positionType == posType.Distance)
                {
                    switch (Distance)
                    {
                        case SpawnPositionsProps.SpawnPosDistance.Short:
                            if (taskStats.shortKicks >= kicks)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosDistance.SemiShort:
                            if (taskStats.shortKicks >= kicks)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosDistance.Medium:
                            if (taskStats.mediumKicks >= kicks)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosDistance.Long:
                            if (taskStats.longKicks >= kicks)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosDistance.SemiLong:
                            if (taskStats.longKicks >= kicks)
                                completed = true;
                            return;
                    }
                }
                //By Side
                else
                {
                    switch (Side)
                    {
                        case SpawnPositionsProps.SpawnPosSide.Left:
                            if (taskStats.kicksOnLeft >= kicks)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosSide.Right:
                            if (taskStats.kicksOnRight >= kicks)
                                completed = true;
                            return;

                        case SpawnPositionsProps.SpawnPosSide.Center:
                            if (taskStats.kicksOnCenter >= kicks)
                                completed = true;
                            return;
                    }
                }               
            }
        }
        else
        {
            taskStats.ResetStats();
            completed = false;
        }

        //se completar seta timer pra nova daily
    }
}
