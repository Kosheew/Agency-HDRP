using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPCFileView : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Transform hintsContainer;
    [SerializeField] private GameObject hintPrefab;
    
    private CluesController _cluesController;

    public void Inject(CluesController cluesController)
    {
        _cluesController = cluesController;
    }

    public void ShowNPCFile(NPCFileData npcFile)
    {
        portrait.sprite = npcFile.SpritePerson;
        descriptionText.text = npcFile.Description;

        // Очищуємо старі зачіпки
        foreach (Transform child in hintsContainer)
        {
            Destroy(child.gameObject);
        }

        // Отримуємо лише відомі зачіпки
        List<CluesData> knownHints = npcFile.GetKnownHints(_cluesController);

        foreach (var hint in knownHints)
        {
            var hintGO = Instantiate(hintPrefab, hintsContainer);
            hintGO.GetComponentInChildren<Text>().text = hint.Description; // Припустимо, що CluesData має HintText
        }
    }
}