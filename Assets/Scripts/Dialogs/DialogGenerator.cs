using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _dialogPanel;
    [SerializeField] private RectTransform _parentObject;
    [SerializeField] private Transform _noParentObject;
    [SerializeField] private float _spaceBetween = 10f;
    [SerializeField] private float _stretchAmount = 400f;

    private Image _personPhoto;
    private Text _personName;
    private TextMeshProUGUI _text;
    private List<Button> _answers;

    public void GenerateDialog(DialogParams dialog)
    {
        //Transform[] children = new Transform[_parentObject.childCount];
        //Vector3[] localPositions = new Vector3[_parentObject.childCount];

        //for (int i = 0; i < _parentObject.childCount; i++)
        //{
        //    children[i] = _parentObject.GetChild(i);
        //    localPositions[i] = children[i].localPosition;
        //}

        //_parentObject.offsetMin = new Vector2(_parentObject.offsetMin.x, _parentObject.offsetMin.y - _stretchAmount);

        //for (int i = 0; i < children.Length; i++)
        //    children[i].localPosition = localPositions[i];

        GameObject panel = Instantiate(_dialogPanel);
        panel.transform.SetParent(_parentObject, false);

        float panelHeight = panel.GetComponent<RectTransform>().rect.height;
        int panelCount = _parentObject.childCount;
        Vector3 newPosition = new Vector3(0, (-panelHeight + -_spaceBetween) * (panelCount - 1), 0);
        panel.GetComponent<RectTransform>().anchoredPosition = newPosition;

        panel.transform.localScale = Vector3.one;

        _personPhoto = panel.transform.Find("PersonPhoto").GetComponent<Image>();
        _personName = panel.GetComponentInChildren<Text>();
        _text = panel.GetComponentInChildren<TextMeshProUGUI>();
        _answers = panel.GetComponentsInChildren<Button>().ToList();

        _personPhoto.sprite = dialog.PersonPhoto;
        _personName.text = dialog.PersonName;
        _text.text = dialog.Text;

        for (int i = dialog.Options.Length; i < _answers.Count;)
        {
            Destroy(_answers[i].gameObject);
            _answers.RemoveAt(i);
        }

        for (int i = 0; i < dialog.Options.Length; i++)
        {
            int index = i;

            _answers[index].GetComponentInChildren<Text>().text = dialog.OptionsName[index];

            _answers[index].onClick.AddListener(() =>
            {
                DisableButtons(_answers);
                GenerateDialog(dialog.Options[index]);
            });
        }
    }

    private void DisableButtons(List<Button> buttons)
    {
        foreach (Button button in buttons)
            button.interactable = false;
    }
}
