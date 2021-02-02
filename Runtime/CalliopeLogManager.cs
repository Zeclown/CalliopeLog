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

    [ExecuteInEditMode]
    public class CalliopeLogManager : MonoBehaviour
    {
        private static CalliopeLogManager _instance = null;
        public static CalliopeLogManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (CalliopeLogManager)FindObjectOfType(typeof(CalliopeLogManager));
                    if (_instance == null)
                        _instance = (new GameObject("CalliopeLogManager")).AddComponent<CalliopeLogManager>();
                }
                return _instance;
            }
        }

        public List<LogEntry> _logEntries = new List<LogEntry>();

        void Awake()
        {
            RegisterCategories();
        }

        public void RegisterCategories()
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