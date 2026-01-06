namespace Darakombi
{
    public class Player : Entity, ILive
    {
        public double Speed { get; set; } = 500;
        public double DX { get; set; } = 0;
        public double DY { get; set; } = 0;

        public Player() : base() { }
        public Player(Point pos) : base(pos) { }
        public Player(Size size) : base(size) { }
        public Player(Point pos, Size size) : base(pos, size) { }

        public override void SetAttributes()
        {
            CanCollide = true;
            CollisionType = CollisionType.Live;

            Color = Brushes.CornflowerBlue;
            BorderColor = Brushes.RoyalBlue;

            DetectionRange = 200;
        }
    }
}