namespace Wallet
{
    public class WalletController
    {
        private readonly WalletModel _wallet;
        private readonly WalletView _walletView;

        public WalletController(WalletModel wallet, WalletView walletView)
        {
            _wallet = wallet;
            _walletView = walletView;

            _wallet.OnKeyCountChanged += UpdateView;
            UpdateView(_wallet.CurrentKey, _wallet.MaxKeys);
        }

        private void UpdateView(int keyScore, int maxKeys)
        {
            _walletView.UpdateKeyText(keyScore, maxKeys);
        }
    }
}