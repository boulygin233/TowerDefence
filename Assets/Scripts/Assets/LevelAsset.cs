using System.Collections.Generic;
using EnemySpawn;
using NUnit.Framework;
using TurretSpawn;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Level Asset", fileName = "Level Asset")]
    public class LevelAsset : ScriptableObject
    {
        public SceneAsset SceneAsset;
        public SpawnWavesAsset SpawnWavesAsset;
        public TurretMarketAsset TurretMarketAsset;
    }
}