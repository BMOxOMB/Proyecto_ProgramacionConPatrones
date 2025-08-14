using UnityEngine;

namespace RTSGame.AI
{
    /// <summary>
    /// Basic team identifiers. Extend as needed.
    /// </summary>
    public enum Team
    {
        Player,
        Enemy
    }

    public static class TeamHelper
    {
        public static bool IsEnemy(Team a, Team b) => a != b;
    }
}
