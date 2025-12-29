namespace CombatWordle
{
    public class Player : Entity
    {
        public double Speed { get; private set; } = 500;
        public double DX { get; set; } = 0;
        public double DY { get; set; } = 0;

        public Player() : base() { }

        public override void SetAttributes()
        {
            CanCollide = true;
            CollisionType = CollisionType.Live;

            Color = Brushes.CornflowerBlue;
            BorderColor = Brushes.RoyalBlue;
        }
    }
}