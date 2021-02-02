using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Calliope
{
    public class CalliopeLogEditorWindow : EditorWindow
    {
        const string JSON_SETTINGS_FILE_PATH = "/CalliopeLog/Settings.json";
        SavedSettings _savedSettings;

        [Serializable]
        private class SavedSettings
        {
            public List<LogEntry> _logEntries = new List<LogEntry>();
        }

        [MenuItem("Calliope/Log Manager")]
        public static void ShowWindow() 
        {
            EditorWindow.GetWindow(typeof(CalliopeLogEditorWindow));
        }

        public void Awake()
        {
            ReadSettingsFile();
        }
        Vector2 _scrollPos;
        void OnGUI()
        {
            GUILayoutOption[] categoryGroupOptions = { GUILayout.MinWidth(10.0f)};
            GUILayoutOption[] categoryVerticalGroupOptions = { GUILayout.Height(10.0f) };
            GUILayout.BeginVertical();
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            GUILayout.Label("Log Categories", EditorStyles.largeLabel);
            foreach (LogEntry entry in _savedSettings._logEntries)
            {
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox, categoryVerticalGroupOptions);
                    {
                        GUILayout.BeginHorizontal(categoryGroupOptions);
                        entry._enabled = EditorGUILayout.Toggle(entry._enabled);
                        EditorGUILayout.LabelField(entry._category.ToString(), EditorStyles.boldLabel);
                        GUILayout.EndHorizontal();
                    }
                    {
                        GUILayout.BeginHorizontal(categoryGroupOptions);
                        EditorGUILayout.PrefixLabel("Verbosity");
                        entry._verbosity = (LogVerbosity)EditorGUILayout.EnumPopup(entry._verbosity, GUILayout.Width(100.0f));
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
            }
            if (GUILayout.Button("Save Settings"))
            {
                SaveSettingsFile();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            UpdateCalliopeCategories();
        }

        void ReadSettingsFile()
        {
            try
            {
                string categoryJson = System.IO.File.ReadAllText(Application.dataPath + JSON_SETTINGS_FILE_PATH);
                _savedSettings = JsonUtility.FromJson(categoryJson, typeof(SavedSettings)) as SavedSettings;
            }
            catch(Exception e)
            { 
                if(e is System.IO.FileNotFoundException notFoundException)
                {
                    Debug.LogFormat("[CalliopeLog] - Could not find settings file at {0}, creating a new one.", Application.dataPath + JSON_SETTINGS_FILE_PATH);
                }
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Application.dataPath + JSON_SETTINGS_FILE_PATH));
            }

            if(_savedSettings == null || _savedSettings._logEntries.Count < Enum.GetValues(typeof(LogCategory)).Length)
            {
                _savedSettings = new SavedSettings();
                foreach (var category in Enum.GetValues(typeof(LogCategory)))
                {
                    LogEntry newEntry = new LogEntry();
                    newEntry._category = (LogCategory)category;
                    newEntry._enabled = true;
                    newEntry._verbosity = 0;
                    _savedSettings._logEntries.Add(newEntry);
                }
                SaveSettingsFile();
            }
        }

        void SaveSettingsFile()
        {
            string categoryJson = JsonUtility.ToJson(_savedSettings, true);
            System.IO.File.WriteAllText(Application.dataPath + JSON_SETTINGS_FILE_PATH, categoryJson);
            AssetDatabase.Refresh();
        }

        void UpdateCalliopeCategories()
        {
            CalliopeLogManager.Instance._logEntries = _savedSettings._logEntries;
            CalliopeLogManager.Instance.RegisterCategories();
        }
    }
}
