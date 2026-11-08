public interface IExperienceManager
{
    int Level { get; }
    long Experience { get; }
    //long NextLevelExperience { get; }

    void Increase(long xpPointsToAdd);
    //void LevelUp();
}

