using System;
using Characters.Player;
using Commands;
using UnityEngine;

public class BatleZone : MonoBehaviour
{
   private CommandPlayerFactory _commandPlayerFactory;
   
   public void Inject(DependencyContainer container)
   {
      _commandPlayerFactory = container.Resolve<CommandPlayerFactory>();
   }
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.TryGetComponent(out PlayerController player))
      {
         _commandPlayerFactory.CreateCombatCommand(player);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.TryGetComponent(out PlayerController player))
      {
         _commandPlayerFactory.CreateRegularCommand(player);
      }
   }
}
