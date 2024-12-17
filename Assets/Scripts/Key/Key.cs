using Characters;
using Wallet;
using UnityEngine;

namespace Keys 
{
    public class Key : MonoBehaviour, ITouching
    {
        [SerializeField] private AudioClip _pickUpSound;
        private AudioManager _audioManager;
        private WalletModel _wallet;
        
        public void Inject(DependencyContainer container)
        {
            _wallet = container.Resolve<WalletModel>();
            _audioManager = container.Resolve<AudioManager>();
        }
        
        public void Touch()
        {           
            _wallet.AddKey();
            _audioManager.PlaySound(_pickUpSound);
            gameObject.SetActive(false); 
        }
    }
}
