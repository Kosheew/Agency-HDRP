using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog_System;

public class GameSaveManager : MonoBehaviour
{
    public GameData gameData { get; set; }
    
    private BinarySaveSystem _saveSystem;
    
    [SerializeField] private bool _clearSave;
    
    public void Inject(DependencyContainer container)
    {
        _saveSystem = container.Resolve<BinarySaveSystem>();
        
        if(_clearSave) _saveSystem.ClearSaveData();
        
        LoadGame();
    }
    
    
    public void SaveNPCProgress(NPCDialogueManager[] npcDialogues)
    {
        var npcProgressData = new List<NPCDialogProgress>(10);
        
        foreach (NPCDialogueManager npcDialogue in npcDialogues)
        {
            var dialogueProgress = npcDialogue.GetComponent<NPCDialogProgress>();
            
            npcProgressData.Add(dialogueProgress);
        }
        
        var dialogProgressData = new NPCDialogProgressData(npcProgressData);
        
        gameData.npcDialogProgressData = dialogProgressData;
    }
    
    public void SaveGame()
    {
        _saveSystem.Save(gameData);
    }

    public void LoadGame()
    {
        gameData = _saveSystem.CheckFileExists() ? _saveSystem.Load<GameData>() : new GameData();
    }
    
    public void Save<T>(T data) where T : ISaveable
    {
        data.SaveTo(gameData);
        SaveGame();
    }

    public void Load<T>(T data) where T : ISaveable
    {
        data.LoadFrom(gameData);
    }
}
