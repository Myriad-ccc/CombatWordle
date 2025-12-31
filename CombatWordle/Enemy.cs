namespace CombatWordle
{
    public class Enemy : Entity, ILive
    {
        public EnemyState State { get; set; } = EnemyState.Idle;
        public double AgroRange { get; set; } = 80;

        public double Speed { get; set; } = 200;
        public double DX { get; set; } = 0;
        public double DY { get; set; } = 0;

        public Enemy() : base() { }
        public Enemy(Point pos) : base(pos) { }
        public Enemy(Size size) : base(size) { }
        public Enemy(Point pos, Size size) : base(pos, size) { }

        public override void SetAttributes()
        {
            CanCollide = true;
            CollisionType = CollisionType.Live;

            Color = Brushes.IndianRed;
            BorderColor = Brushes.DarkRed;

            DetectionRange = 450;
        }
    }

    public enum EnemyState
    {
        Idle,
        Chasing,
        Attacking
    }
}