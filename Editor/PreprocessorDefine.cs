using System.Collections.Generic;
using UnityEditor;

namespace Calliope
{
    static class PreprocessorDefines
    {
        private static string DEFINE_STRING = "CALLIOPE_ENABLED";

        public static bool HasDefine()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Contains(DEFINE_STRING);
        }

        public static void AddDefineSymbols()
        {
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            HashSet<string> defines = new HashSet<string>(currentDefines.Split(';'))
            {
                DEFINE_STRING
            };

            string newDefines = string.Join(";", defines);
            if (newDefines != currentDefines)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
            }
        }

        public static void RemoveDefineSymbols()
        {
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            string newDefines = currentDefines.Replace(DEFINE_STRING, "");
            if (newDefines != currentDefines)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
            }
        }
    }
}
