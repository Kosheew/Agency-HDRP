using UserController;
using UnityEngine;
using CharacterSettings;
using Characters;
using Audio;

namespace Player.State
{
    public class MovementState : IPlayerState
    {
        private Camera _camera;

        private CharacterController _characterController;
        private CharacterAudioSettings _characterAudioSettings;
        private PlayerSetting _playerSetting;

        private IFootstepAudioHandler _footstepHandler;
        private IUserInputs _userInputs;
        private IMovement _movement;
        
        public void EnterState(IPlayer player)
        {
            player.Alive = true;
            
            _playerSetting = player.PlayerSetting;
            _characterController = player.Controller; 
            _userInputs = player.UserInputs;
            _footstepHandler = player.FootstepHandler;
            _camera = player.CameraMain;
            
            _movement = new CharacterMovement(
                _playerSetting.MaxClamp,
                _playerSetting.MoveSpeed,
                _playerSetting.AccelerationRate,
                _playerSetting.TurnSpeed);
        }

        public void UpdateState(IPlayer player)
        {
         //   var lookX = _userInputs.GetLookDirectionX();
           // var lookY = _userInputs.GetLookDirectionY();
           // var moveVertical = _userInputs.GetMoveDirectionVertical();
           // var moveHorizontal = _userInputs.GetMoveDirectionHorizontal();
            
            //_movement.RotateCharacter(lookX, lookY, player.TransformMain, _camera.transform);
            //var velocity = _movement.MoveCharacter(moveHorizontal, moveVertical, player.TransformMain);
            //var gravityVelocity = _movement.AddGravity();

            //_characterController.Move(_characterController.isGrounded ? velocity : (Vector3)gravityVelocity);

           // if (IsMoving(velocity))
           // {
                _footstepHandler.PlayFootstepSound();
            //}
        }

        public void ExitState(IPlayer player)
        {
            
        }

        private bool IsMoving(Vector3 velocity)
        {
            var velocityMagnitude = new Vector3(velocity.x, 0, velocity.z).magnitude;
            return velocityMagnitude > 0.01f;
        }
    }
}
