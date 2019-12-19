using UnityEngine;
using UnityEngine.AI;

namespace UnityEditor.AI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LocalNavMeshBuilder))]
    public class LocalNavMeshBuilderEditor: Editor
    {
        SerializedProperty m_AgentTypeID;
        SerializedProperty m_Size;
        SerializedProperty m_SourceTag;


        void OnEnable()
        {
            m_AgentTypeID = serializedObject.FindProperty("m_AgentTypeID");
            m_Size = serializedObject.FindProperty("m_Size");
            m_SourceTag = serializedObject.FindProperty("m_SourceTag");
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();
            
            var bs = NavMesh.GetSettingsByID(m_AgentTypeID.intValue);

            if (bs.agentTypeID != -1)
            {
                // Draw image
                const float diagramHeight = 80.0f;
                Rect agentDiagramRect = EditorGUILayout.GetControlRect(false, diagramHeight);
                NavMeshEditorHelpers.DrawAgentDiagram(agentDiagramRect, bs.agentRadius, bs.agentHeight, bs.agentClimb, bs.agentSlope);
            }
            NavMeshComponentsGUIUtility.AgentTypePopup("Agent Type", m_AgentTypeID);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_Size);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_SourceTag);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}