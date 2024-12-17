using System;

namespace Wallet
{
    public class WalletModel
    {
        private int _countKey;
        private int _maxKeys;

        public event Action<int, int> OnKeyCountChanged;

        public WalletModel(int maxKeys)
        {
            _maxKeys = maxKeys;
        }

        public void AddKey()
        {
            _countKey++;
            OnKeyCountChanged?.Invoke(_countKey, _maxKeys);
        }

        public int CurrentKey => _countKey;

        public int MaxKeys => _maxKeys;
    }
}