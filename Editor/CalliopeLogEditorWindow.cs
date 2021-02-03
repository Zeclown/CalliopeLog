using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Calliope
{
    public class CalliopeLogEditorWindow : EditorWindow
    {
        CalliopeLogFileHelpers.SavedSettings _savedSettings = new CalliopeLogFileHelpers.SavedSettings();


        [MenuItem("Tools/Calliope/Calliope Log Manager")]
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
            bool hasDefine = Calliope.PreprocessorDefines.HasDefine();
            bool shouldBeEnabled = GUILayout.Toggle(hasDefine, "Logs Compiled");
            if (shouldBeEnabled && !hasDefine)
            {
                Calliope.PreprocessorDefines.AddDefineSymbols();
            }
            else if(!shouldBeEnabled && hasDefine)
            {
                Calliope.PreprocessorDefines.RemoveDefineSymbols();
            }

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
        }

        void ReadSettingsFile()
        {
            _savedSettings = CalliopeLogFileHelpers.LoadSettings();
        }

        void SaveSettingsFile()
        {
            CalliopeLogFileHelpers.SaveSettings(_savedSettings);
            CalliopeLogManager.RegisterCategories();
        }
    }
}
