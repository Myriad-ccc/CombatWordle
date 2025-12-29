namespace CombatWordle
{
    public class SceneManager
    {
        private readonly Canvas Canvas;

        public SceneManager(Canvas canvas)
        {
            Canvas = canvas;
        }

        public void Update(Rect Viewport, List<EntityData> entities)
        {
            var viewport = Viewport;
            viewport.Inflate(150, 150);
            foreach (var data in entities)
            {
                var e = data.Entity;
                bool onScreen = viewport.IntersectsWith(data.Rect);
                bool onCanvas = e.Visual != null;
                if (onScreen)
                {
                    if (!onCanvas)
                    {
                        e.Visual = new()
                        {
                            Width = e.Width,
                            Height = e.Height,
                            BorderThickness = new(e.Area / (5 * e.Parameter)),
                            Background = e.DefaultColor,
                            BorderBrush = e.DefaultBorderColor
                        };
                        Canvas.Children.Add(e.Visual);
                    }
                    Canvas.SetLeft(data.Entity.Visual, data.X);
                    Canvas.SetTop(data.Entity.Visual, data.Y);
                }
                else
                {
                    if (onCanvas) continue;
                    Canvas.Children.Remove(e.Visual);
                    e.Visual = null;
                }
            }
        }
    }
}