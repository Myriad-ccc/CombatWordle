namespace CombatWordle
{
    public class Rock : Entity
    {
        private static readonly Random random = new();

        public Rock()
        {
            UpdateDimensions(GetRandomSize());
            SetAttributes();
        }

        public Rock(Point pos) : base(pos)
        {
            UpdateDimensions(GetRandomSize());
            SetAttributes();
        }

        public Rock(Point pos, Size size) : base(pos, size)
        {
            SetAttributes();
        }

        private void SetAttributes()
        {
            CanCollide = true;
            CollisionType = CollisionType.Enviornment;

            DefaultColor = Brushes.Gray;
            DefaultBorderColor = Brushes.LightGray;

            Visual.Background = DefaultColor;
            Visual.BorderBrush = DefaultBorderColor;
        }

        private static Size GetRandomSize()
        {
            int w = 0, h = 0;
            while (w * h < 1000)
            {
                w = random.Next(20, 200);
                h = random.Next(5, 120);
            }
            return new(w, h);
        }
    }
}
