using System.Reflection;
using System.Windows.Shapes;

namespace CombatWordle
{
    public static class QOL
    {
        public static SolidColorBrush RandomColor() => new(Color.FromArgb(255, (byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256)));
        public static SolidColorBrush RGB(int r) => new(Color.FromArgb(255, (byte)r, (byte)r, (byte)r));

        public static void WriteOut(object obj) => MessageBox.Show($"{obj}");

        public static Point CenterOfRect(Rect rect) => new(
            rect.X + rect.Width / 2,
            rect.Y + rect.Height / 2
            );

        private static double AverageFPS;
        public static double GetAverageFPS(double dt)
        {
            double perFrame = 1.0 / dt;
            double alpha = 1 - Math.Exp(-dt / 0.5);
            AverageFPS = (AverageFPS == 0) ? perFrame : AverageFPS + alpha * (perFrame - AverageFPS);
            return AverageFPS;
        }

        public static double NextDoubleInRange(double min, double max)
        {
            if (min >= max) return min;
            return Random.Shared.NextDouble() * (max - min) + min;
        }

        public static Size GetRandomSize(double wMin, double wMax, double hMin, double hMax)
        {
            if (wMin <= 0 || hMin <= 0 || wMin > wMax || hMin > hMax)
                return Size.Empty;
            double w = NextDoubleInRange(wMin, wMax);
            double h = NextDoubleInRange(hMin, hMax);
            return new(w, h);
        }

        public static Size GetRandomSize(double[] arg)
        {
            if (arg.Length != 4) return Size.Empty;
            double wMin = arg[0], wMax = arg[1], hMin = arg[2], hMax = arg[3];
            if (wMin <= 0 || hMin <= 0 || wMin > wMax || hMin > hMax)
                return Size.Empty;
            double w = NextDoubleInRange(wMin, wMax);
            double h = NextDoubleInRange(hMin, hMax);
            return new(w, h);
        }

        public static void DrawGrid(Canvas canvas, int width, int height, int cellSize, Brush? color = null, double strokeThickness = 1, double opacity = 0.5)
        {
            var grid = new Grid()
            {
                IsHitTestVisible = false,
                Opacity = opacity
            };
            canvas.Children.Add(grid);

            color ??= Brushes.Green;

            for (int x = 0; x <= width; x += cellSize)
            {
                var line = new Line()
                {
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = height,
                    Stroke = color,
                    StrokeThickness = strokeThickness
                };
                grid.Children.Add(line);
            }

            for (int y = 0; y <= height; y += cellSize)
            {
                var line = new Line()
                {
                    X1 = 0,
                    X2 = width,
                    Y1 = y,
                    Y2 = y,
                    Stroke = color,
                    StrokeThickness = strokeThickness
                };
                grid.Children.Add(line);
            }
        }

        public static Type[] GetDerivedTypes<T>() =>
            Assembly
            .GetAssembly(typeof(T))!
            .GetTypes()
            .Where(t => typeof(T).IsAssignableFrom(t) && t != typeof(T) && !t.IsAbstract)
            .ToArray();
    }
}