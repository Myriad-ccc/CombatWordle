namespace CombatWordle
{
    public class SceneManager
    {
        private readonly Canvas Canvas;

        private readonly HashSet<EntityData> Rendered = [];
        private readonly HashSet<EntityData> Visible = [];
        private readonly List<EntityData> ToRemove = [];

        public SceneManager(Canvas canvas)
        {
            Canvas = canvas;
        }

        public void Update(Rect viewport, List<EntityData> allEntities, IEnumerable<EntityData> viewPortEntities = null)
        {
            Visible.Clear();
            ToRemove.Clear();

            var viewportEntities = viewPortEntities ?? EntitiesInArea(viewport, allEntities);

            foreach (var data in EntitiesInArea(viewport, allEntities))
            {
                Visible.Add(data);
                if (Rendered.Add(data))
                    Add(data);
                Canvas.SetLeft(data.Entity.Visual, data.X);
                Canvas.SetTop(data.Entity.Visual, data.Y);
            }

            foreach (var data in Rendered.Where(e => !Visible.Contains(e)))
                ToRemove.Add(data);
            foreach (var data in ToRemove)
            {
                Remove(data);
                Rendered.Remove(data);
            }
        }

        public void Add(EntityData data)
        {
            var e = data.Entity;
            e.Visual ??= new()
            {
                Width = e.Width,
                Height = e.Height,
                BorderThickness = new(e.Area / (5 * e.Parameter)),
                Background = e.Color,
                BorderBrush = e.BorderColor
            };
            Canvas.Children.Add(e.Visual);
            data.Visible = true;
        }

        public void Remove(EntityData data)
        {
            var e = data.Entity;
            if (e.Visual != null)
            {
                Canvas.Children.Remove(e.Visual);
                data.Visible = false;
            }
        }

        public IEnumerable<EntityData> EntitiesInArea(Rect area, List<EntityData> allEntities)
        { // use spatial grid instead if possible
            foreach (var entity in allEntities)
            {
                if (entity.Rect.IntersectsWith(area))
                    yield return entity;
            }
        }
    }
}