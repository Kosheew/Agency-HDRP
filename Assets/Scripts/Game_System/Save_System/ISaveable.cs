public interface ISaveable
{
    void SaveTo(GameData gameData);
    void LoadFrom(GameData gameData);
}