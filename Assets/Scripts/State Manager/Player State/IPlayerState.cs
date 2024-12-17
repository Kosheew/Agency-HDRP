namespace Characters
{
    public interface IPlayerState
    {
        void EnterState(IPlayer player);
        void UpdateState(IPlayer player);
        void ExitState(IPlayer player);
    }
}