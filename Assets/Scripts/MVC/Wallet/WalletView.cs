using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wallet
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text keyText;
        [SerializeField] private Slider keySlider; 
        
        public void UpdateKeyText(int keyScore, int maxKeys)
        {
            keySlider.maxValue = maxKeys;
            keySlider.value = keyScore;
            keyText.SetText($"{keyScore}");
        }
    }
}
