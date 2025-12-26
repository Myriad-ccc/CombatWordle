namespace CombatWordle
{
    public class EntityData
    {
        public Entity Entity { get; private set; }
        public bool Visible;
        //int chunkID

        public Rect Rect => Entity.Rect;
        public double X => Rect.X;
        public double Y => Rect.Y;

        public EntityData(Entity entity)
        {
            Entity = entity;
        }
    }
}