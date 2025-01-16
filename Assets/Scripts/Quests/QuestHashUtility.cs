using System.Collections.Generic;

namespace Quests
{
    public static class QuestHashUtility
    {
        private static Dictionary<string, int> _hashCache = new Dictionary<string, int>();

        /// <summary>
        /// Перетворює ім'я на хеш та кешує результат для швидшого доступу.
        /// </summary>
        /// <param name="questName">Ім'я квесту</param>
        /// <returns>Хеш-код для заданого імені</returns>
        public static int GetQuestHash(string questName)
        {
            if (string.IsNullOrEmpty(questName))
            {
                UnityEngine.Debug.LogWarning("Quest name is null or empty!");
                return 0;
            }

            if (_hashCache.TryGetValue(questName, out var cachedHash))
            {
                return cachedHash;
            }

            int hash = questName.GetHashCode();
            _hashCache[questName] = hash;
            return hash;
        }
    }

}