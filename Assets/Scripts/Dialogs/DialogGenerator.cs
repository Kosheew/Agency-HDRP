using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogGenerator : MonoBehaviour
{
    public GameObject _dialogPanel;
    private Image _personPhoto;
    private Text _personName;
    private TextMeshProUGUI _text;
    private Button[] _answers;

    public void GenerateDialog(DialogParams dialog)
    {
        GameObject panel = Instantiate(_dialogPanel, transform.position, transform.rotation);

        _personPhoto = panel.transform.Find("PersonPhoto").GetComponent<Image>();
        _personName = panel.GetComponentInChildren<Text>();
        _text = panel.GetComponentInChildren<TextMeshProUGUI>();
        _answers = panel.GetComponentsInChildren<Button>();

        _personPhoto.sprite = dialog.PersonPhoto;
        _personName.text = dialog.PersonName;
        _text.text = dialog.Text;

        for (int i = 0; i < dialog.Options.Length; i++)
        {
            _answers[i].onClick.RemoveAllListeners();
            _answers[i].onClick.AddListener(() => { GenerateDialog(dialog.Options[i]); });
            _answers[i].GetComponentInChildren<Text>().text = dialog.OptionsName[i];
        }

        for (int i = dialog.Options.Length; i < _answers.Length; i++)
            Destroy(_answers[i].gameObject); 
    }
}
