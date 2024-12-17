using UnityEngine;

namespace Obstacle
{
    /// <summary>
    /// Handles interactions when the player collides with an obstacle.
    /// Implements the ITouching interface to define touch behavior.
    /// </summary>
    public class ObstacleTouch : MonoBehaviour, ITouching
    {
        /// <summary>
        /// Invoked when the player touches the obstacle.
        /// Triggers the game end event with a "Loose" signal.
        /// </summary>
        /// <param name="eventBus">The EventBus used to invoke events.</param>
        public void Touch()
        {
           
        }
    }
}
