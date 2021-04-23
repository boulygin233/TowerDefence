using Runtime;

namespace Main
{
    public class WinController : IController
    {
        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            Game.Player.CheckForWin();
        }
    }
}