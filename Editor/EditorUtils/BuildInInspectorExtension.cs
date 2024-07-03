//using System;
//using System.Reflection;
//using UnityEditor;
//using UnityEditor.TerrainTools;
//using UnityEngine;

//namespace BuildInInspectorExtension
//{
//    [CustomEditor(typeof(Transform),true), CanEditMultipleObjects]
//    public class TransformInspectorExtension : Editor
//    {
//        private Editor transformEditor;
//        public GUIStyle style = new GUIStyle();

//        private void OnEnable()
//        {
//            style.fontSize = 11;
//            style.normal.textColor = new Color(.5f, .5f, .5f);
//            transformEditor = Editor.CreateEditor(target, Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.TransformInspector"));
//        }

//        public override void OnInspectorGUI()
//        {
//            transformEditor.OnInspectorGUI();
//            Vector3 pos = Vector3.zero;
//            Vector3 rotation = Vector3.zero;
//            Vector3 scale = Vector3.zero;
//            foreach (Transform trans in targets)
//            {
//                pos = trans.position;
//                rotation = trans.eulerAngles;
//                scale = trans.lossyScale;
//            }
//            string message = $"World:\n Position: {pos} Rotation: {rotation} Scale: {scale}";
//            GUILayout.Label(message, style);
//        }
//    }
//}