using System.Collections.Generic;
using Runtime;

namespace Enemy
{
    public class EnemyDeathController : IController

    {
        private List<EnemyData> m_DiedEnemyDatas = new List<EnemyData>();

        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            foreach (EnemyData enemyData in Game.Player.EnemyDatas)
            {
                if (enemyData.IsDead)
                {
                    m_DiedEnemyDatas.Add(enemyData);
                    Game.Player.TurretMarket.GetReward(enemyData);
                    enemyData.Die();
                }
            }

            foreach (EnemyData enemyData in m_DiedEnemyDatas)
            {
                Game.Player.EnemyDied(enemyData);
            }

            m_DiedEnemyDatas.Clear();
        }
    }
}