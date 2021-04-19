using System;
using UnityEngine;

namespace Utils.Pulling
{
    public class PooledMonoBehaviour : MonoBehaviour
    {
        private int m_PrefabID;
        public int PrefabID => m_PrefabID;

        public virtual void AwakePooled() 
        {
        }

        public void SetPrefabID(int id)
        {
            m_PrefabID = id;
        }
    }
}
















