using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CombatWordle
{
    public class Renderer
    {
        private readonly Canvas Canvas;

        private HashSet<EntityData> CurrentlyVisible = [];

        public Renderer(Canvas canvas)
        {
            Canvas = canvas;
        }

        public void RenderEntities(List<EntityData> visible, List<EntityData> hidden)
        {
            foreach (var data in visible)
            {
                CurrentlyVisible.Add(data);
                if (!Canvas.Children.Contains(data.Entity.Visual))
                    Canvas.Children.Add(data.Entity.Visual);
            }
            foreach (var data in hidden)
            {
                CurrentlyVisible.Remove(data);
                Canvas.Children.Remove(data.Entity.Visual);
            }
            foreach (var data in CurrentlyVisible)
            {
                Canvas.SetLeft(data.Entity.Visual, data.X);
                Canvas.SetTop(data.Entity.Visual, data.Y);
            }
        }
    }
}
