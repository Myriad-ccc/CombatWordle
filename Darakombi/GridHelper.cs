using System.Windows.Shapes;

namespace Darakombi
{
    public class GridHelper : Grid
    {
        public int CellSize { get; set; } = 128;
        public Brush Color { get; set; } = Brushes.White;
        public double Thickness { get; set; } = 1.0;

        public GridHelper(int cellSize, int width, int height, Brush color, double opacity)
        {
            CellSize = cellSize;
            Width = width;
            Height = height;
            Color = color;
            Opacity = opacity;
            Create();
        }

        public Grid Create()
        {
            var grid = new Grid()
            {
                IsHitTestVisible = false,
                Opacity = Opacity,
            };
            Canvas.SetLeft(grid, 0);
            Canvas.SetTop(grid, 0);

            Update();
            return grid;
        }

        public void Update()
        {
            for (int x = 0; x <= Width; x += CellSize)
            {
                var line = new Line()
                {
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = Height,
                    Stroke = Color,
                    StrokeThickness = Thickness
                };
                Children.Add(line);
            }

            for (int y = 0; y <= Height; y += CellSize)
            {
                var line = new Line()
                {
                    X1 = 0,
                    X2 = Width,
                    Y1 = y,
                    Y2 = y,
                    Stroke = Color,
                    StrokeThickness = Thickness
                };
                Children.Add(line);
            }
        }
    }
}