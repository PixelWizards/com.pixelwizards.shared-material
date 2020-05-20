using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PixelWizards.Timeline
{

    [CustomEditor(typeof(SharedMaterialGlobalHelper))]
    public class SharedMaterialGlobalHelperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            SharedMaterialGlobalHelper materialReferences = (SharedMaterialGlobalHelper) target;
            GUILayout.Space(20f);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Update")) //8
            {
                materialReferences.UpdateRenderers();
            }
            if (GUILayout.Button("Reset")) //8
            {
                materialReferences.ResetRenderers();
            }
        
            GUILayout.EndHorizontal();
        }
    }
}