using System;
using Characters.Player;
using Commands;
using UnityEngine;

public class BattleController : MonoBehaviour
{
   private CommandPlayerFactory _commandPlayerFactory;
   private PlayerController _playerController;
   
   public void Inject(DependencyContainer container)
   {
      _commandPlayerFactory = container.Resolve<CommandPlayerFactory>();
      _playerController = container.Resolve<PlayerController>();
   }
   public void StartBattle()
   {
      _commandPlayerFactory.CreateCombatCommand(_playerController);
   }

   public void EndBattle()
   {
         _commandPlayerFactory.CreateRegularCommand(_playerController);
   }
}
