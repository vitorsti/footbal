public static class GameStateManager
{
    public static StateOfTheGame GameState;

    public enum StateOfTheGame
    {
        Formation,
        Quarterback,
        Runner,
        LoadingRunner,
        FieldGoal,
        RunningBack,
        KickingBack,
        ExtraGoal,
        ExtraTouchDown,
        Touchdown,
        LoadingQB,
        LoadingFormation,
        LoadingFieldGoal,
        ChosingLastDownAction,
        ChosingExtraPoint,
        LogginEnemyDowns,
        GameOver
    }
}