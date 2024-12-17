using Characters;
using Audio;

namespace Characters.Character_Interfaces
{
    public interface IFootstepCharacterAudio
    {
        public IFootstepAudioHandler FootstepHandler { get; }
    }

    public interface IAttackCharacterAudio
    {
        public AttackAudioHandler AttackAudio { get; }
    }
}