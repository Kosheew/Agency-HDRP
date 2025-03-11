using System.Collections.Generic;
using System.Linq;
using Dialog_System;
using UnityEngine;

public class DialogProgressManager: MonoBehaviour
{
    [SerializeField] private DialogueSettings[] dialogueSettings;
    [SerializeField] private DialogueOptionSettings[] optionSettings;
    
    private GameSaveManager _gameSaveManager;
    
    private List<NPCDialogProgress> _npcProgressData;
    
    private GameData _gameData;
    
    public void Inject(DependencyContainer container)
    {
        _gameSaveManager = container.Resolve<GameSaveManager>();
        
        _npcProgressData = new List<NPCDialogProgress>(10);
    }

    private void LoadProgressDialogue()
    {
        if (_gameSaveManager.gameData.npcDialogProgressData.dialoguesProgress.Count > 0)
        {
            _npcProgressData = _gameSaveManager.gameData.npcDialogProgressData.dialoguesProgress;
        }
        else
        {
            _npcProgressData = new List<NPCDialogProgress>(10);
        }
    }
    
    public Dictionary<DialogueSettings, DialogueOptionSettings> GetNPCProgress(ushort npcId)
    {
        var _currentNpc = _npcProgressData.FirstOrDefault(npc => npc.NpcId == npcId);

        var _passedDialogues = new Dictionary<DialogueSettings, DialogueOptionSettings>(10);
        
        if (_currentNpc != null)
        {
            for (int i = 0; i < _currentNpc.CompletedDialogues.Count; i++)
            {
                ushort dialogId = _currentNpc.CompletedDialogues[i];
                var dialogueSetting = dialogueSettings.FirstOrDefault(d => d.UniqueID == dialogId);
                if (dialogueSetting == null) continue;

                DialogueOptionSettings option = null;

                if (i < _currentNpc.ChooseDialoguesOptions.Count)
                {
                    ushort optionId = _currentNpc.ChooseDialoguesOptions[i];
                    option = optionSettings.FirstOrDefault(o => o.UniqueID == optionId);
                }

                _passedDialogues[dialogueSetting] = option;
            }
        }

        return _passedDialogues;
    }
    
}