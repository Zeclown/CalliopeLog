using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Calliope;

namespace Calliope
{
    public static class CalliopeLogFileHelpers
    {
        const string JSON_SETTINGS_FILE_PATH = "/CalliopeLog/Settings.json";
        [Serializable]
        public class SavedSettings
        {
            public List<LogEntry> _logEntries = new List<LogEntry>();
        }

        public static SavedSettings LoadSettings()
        {
            SavedSettings savedSettings = null;
            try
            {
                string categoryJson = System.IO.File.ReadAllText(Application.dataPath + JSON_SETTINGS_FILE_PATH);
                savedSettings = JsonUtility.FromJson(categoryJson, typeof(SavedSettings)) as SavedSettings;
            }
            catch (Exception e)
            {
                if (e is System.IO.FileNotFoundException notFoundException)
                {
                    Debug.LogFormat("[CalliopeLog] - Could not find settings file at {0}, creating a new one.", Application.dataPath + JSON_SETTINGS_FILE_PATH);
                }
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Application.dataPath + JSON_SETTINGS_FILE_PATH));
            }

            if (savedSettings == null || savedSettings._logEntries.Count < Enum.GetValues(typeof(LogCategory)).Length)
            {
                savedSettings = new SavedSettings();
                foreach (var category in Enum.GetValues(typeof(LogCategory)))
                {
                    LogEntry newEntry = new LogEntry();
                    newEntry._category = (LogCategory)category;
                    newEntry._enabled = true;
                    newEntry._verbosity = 0;
                    savedSettings._logEntries.Add(newEntry);
                }
            }
            return savedSettings;
        }

#if UNITY_EDITOR
        public static void SaveSettings(SavedSettings settings)
        {
            string categoryJson = JsonUtility.ToJson(settings, true);
            System.IO.File.WriteAllText(Application.dataPath + JSON_SETTINGS_FILE_PATH, categoryJson);
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}
