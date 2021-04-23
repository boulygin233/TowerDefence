using Runtime;

namespace Main
{
    public class LoseController : IController
    {
        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            Game.Player.CheckForLose();
        }
    }
}