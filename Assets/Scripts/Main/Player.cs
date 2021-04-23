using System;
using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using Turret;
using TurretSpawn;
using UnityEngine;
using Grid = Field.Grid;
using Object = UnityEngine.Object;

namespace Main
{
    public class Player
    {
        private List<EnemyData> m_EnemyDatas = new List<EnemyData>();
        public IReadOnlyList<EnemyData> EnemyDatas => m_EnemyDatas;

        private List<TurretData> m_TurretDatas = new List<TurretData>();

        public List<TurretData> TurretDatas => m_TurretDatas;

        public readonly GridHolder GridHolder;
        public readonly Grid Grid;
        public readonly TurretMarket TurretMarket;

        private bool m_AllWavesAreSpawned = false;
        private int m_Health;

        public int Health => m_Health;
        public event Action<int> HealthChanged;

        public Player()
        {
            GridHolder = Object.FindObjectOfType<GridHolder>();
            GridHolder.CreateGrid();
            Grid = GridHolder.Grid;

            TurretMarket = new TurretMarket(Game.CurrentLevel.TurretMarketAsset);
            m_Health = Game.CurrentLevel.StartHealth;
        }

        public void EnemySpawned(EnemyData data)
        {
            m_EnemyDatas.Add(data);
        }

        public void EnemyDied(EnemyData data)
        {
            m_EnemyDatas.Remove(data);
        }

        public void EnemyReachedTarget(EnemyData data)
        {
            m_EnemyDatas.Remove(data);
        }

        public void ApplyDamage(int damage)
        {
            m_Health -= damage;
            HealthChanged?.Invoke(m_Health);
        }

        public void TurretSpawn(TurretData data)
        {
            m_TurretDatas.Add(data);
        }

        public void LastWaveSpawned()
        {
            m_AllWavesAreSpawned = true;
        }

        public void CheckForWin()
        {
            if (m_AllWavesAreSpawned && m_EnemyDatas.Count == 0)
            {
                GameWon();
            }
        }

        private void GameWon()
        {
            Debug.Log("Win!");
            Game.StopPlayer();
        }

        public void CheckForLose()
        {
            if (m_Health <= 0)
            {
                GameLost();
            }
        }

        private void GameLost()
        {
            Debug.Log("Lose!");
            Game.StopPlayer();
        }
    }
}