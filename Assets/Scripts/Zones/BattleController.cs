using System;
using Characters.Player;
using Commands;
using UnityEngine;

public class BattleController : MonoBehaviour
{
   private CommandPlayerFactory _commandPlayerFactory;
   private PlayerContext _playerContext;
   
   public void Inject(DependencyContainer container)
   {
      _commandPlayerFactory = container.Resolve<CommandPlayerFactory>();
      _playerContext = container.Resolve<PlayerContext>();
   }
   public void StartBattle()
   {
      _commandPlayerFactory.CreateCombatCommand(_playerContext);
   }

   public void EndBattle()
   {
      _commandPlayerFactory.CreateRegularCommand(_playerContext);
   }
}
