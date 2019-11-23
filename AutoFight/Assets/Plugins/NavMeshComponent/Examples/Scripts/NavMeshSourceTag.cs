using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

// Tagging component for use with the LocalNavMeshBuilder
// Supports mesh-filter and terrain - can be extended to physics and/or primitives
[DefaultExecutionOrder(-200)]
public class NavMeshSourceTag : MonoBehaviour
{
    // Global containers for all active mesh/terrain tags

    public static List<GameObject> m_GameObjects = new List<GameObject>();
    
    void OnEnable()
    {
        m_GameObjects.Add(gameObject);
    }

    void OnDisable()
    {
        m_GameObjects.Remove(gameObject);
    }

    // Collect all the navmesh build sources for enabled objects tagged by this component
    public static void Collect(ref List<NavMeshBuildSource> sources, string targetTag)
    {
        sources.Clear();

        List<MeshFilter> mMeshes = new List<MeshFilter>();
        List<Terrain> mTerrains = new List<Terrain>();
        
        for (int i = 0; i < m_GameObjects.Count; i++)
        {
            string[] tag = targetTag.Split(';');
            bool tagCompared = false;
            for (int j = 0; j < tag.Length; j++)
            {
                if (m_GameObjects[i].CompareTag(tag[j]) || m_GameObjects[i].CompareTag("Ground") )
                    tagCompared = true;
            }

            if (!tagCompared)
            {
                continue;
            }
            
            var m = m_GameObjects[i].GetComponent<MeshFilter>();
            if (m != null)
            {
                mMeshes.Add(m);
            }

            var t = m_GameObjects[i].GetComponent<Terrain>();
            if (t != null)
            {
                mTerrains.Add(t);
            }
        }
        
        for (var i = 0; i < mMeshes.Count; ++i)
        {
            var mf = mMeshes[i];
            if (mf == null) continue;
            
            var m = mf.sharedMesh;
            if (m == null) continue;

            var s = new NavMeshBuildSource();
            s.shape = NavMeshBuildSourceShape.Mesh;
            s.sourceObject = m;
            s.transform = mf.transform.localToWorldMatrix;
            s.area = 0;
            sources.Add(s);
        }

        for (var i = 0; i < mTerrains.Count; ++i)
        {
            var t = mTerrains[i];
            if (t == null) continue;

            var s = new NavMeshBuildSource();
            s.shape = NavMeshBuildSourceShape.Terrain;
            s.sourceObject = t.terrainData;
            // Terrain system only supports translation - so we pass translation only to back-end
            s.transform = Matrix4x4.TRS(t.transform.position, Quaternion.identity, Vector3.one);
            s.area = 0;
            sources.Add(s);
        }
    }
}
