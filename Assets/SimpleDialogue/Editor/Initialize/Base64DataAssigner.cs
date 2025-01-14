#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SimpleDialogueEditor
{
    [InitializeOnLoad]
    public static class Base64DataAssigner
    {
        static Base64DataAssigner()
        {
            AssignIconsFromAttributes();
            
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            AssignIconsFromAttributes();
        }

        private static void AssignIconsFromAttributes()
        {
            Assembly mainAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");

            if (mainAssembly == null)
            {
                return;
            }

            foreach (Type type in mainAssembly.GetTypes())
            {
                Base64DataAttribute iconAttribute = type.GetCustomAttribute<Base64DataAttribute>();
                if (iconAttribute != null)
                {
                    byte[] iconData = Convert.FromBase64String(iconAttribute.Base64Data);
                    Texture2D icon = new Texture2D(2, 2);
                    icon.filterMode = iconAttribute.FilterMode;

                    if (!icon.LoadImage(iconData))
                    {
                        continue;
                    }

                    MonoScript monoScript = GetMonoScriptForType(type); 
                    if (monoScript != null)
                    {
                        EditorGUIUtility.SetIconForObject(monoScript, icon);
                    }
                }
            }
        }

        private static MonoScript GetMonoScriptForType(Type type)
        {
            return Resources.FindObjectsOfTypeAll<MonoScript>()
                .FirstOrDefault(script => script.GetClass() == type);
        }
    }
}
#endif
