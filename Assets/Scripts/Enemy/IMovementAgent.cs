using Field;

namespace Enemy
{
    public interface IMovementAgent
    {
        void TickMovement();
        void Die();
        
        Node GetCurrentNode();
    }
}