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
        static CalliopeLogManager()
        {
            RegisterCategories();
        }

        [RuntimeInitializeOnLoadMethod]
        public static void RegisterCategories()
        {
            List<LogEntry> logEntries = Calliope.CalliopeLogFileHelpers.LoadSettings()._logEntries;
            Calliope.LogRegistry.Clear();
            for (int i = 0; i < logEntries.Count; ++i)
            {
                if (logEntries[i]._enabled)
                {
                    Calliope.LogRegistry.LogEntryInfo newEntry = new LogRegistry.LogEntryInfo();
                    newEntry.categoryName = logEntries[i]._category.ToString();
                    newEntry.verbosity = (int)logEntries[i]._verbosity;
                    Calliope.LogRegistry.EnableLogEntry((int)logEntries[i]._category, newEntry);
                }
            }
        }
    }
}