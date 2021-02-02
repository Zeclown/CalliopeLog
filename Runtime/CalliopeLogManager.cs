using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calliope
{
    public enum LogCategory
    {
        None = 0,
        General,
        Player,
        Network,
        Editor,
        GameManager,
        AI,
    }
    public enum LogVerbosity
    {
        Critical,
        Important,
        All,
    }

    [System.Serializable]
    public class LogEntry
    {
        public LogCategory _category;
        public LogVerbosity _verbosity;
        public bool _enabled;
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public static class CalliopeLogManager
    {

        public static List<LogEntry> _logEntries = new List<LogEntry>();

        static CalliopeLogManager()
        {
            RegisterCategories();
        }

        [RuntimeInitializeOnLoadMethod]
        public static void RegisterCategories()
        {
            Calliope.LogRegistry.Clear();
            for (int i = 0; i < _logEntries.Count; ++i)
            {
                if (_logEntries[i]._enabled)
                {
                    Calliope.LogRegistry.LogEntryInfo newEntry = new LogRegistry.LogEntryInfo();
                    newEntry.categoryName = _logEntries[i]._category.ToString();
                    newEntry.verbosity = (int)_logEntries[i]._verbosity;
                    Calliope.LogRegistry.EnableLogEntry((int)_logEntries[i]._category, newEntry);
                }
            }
        }
    }
}