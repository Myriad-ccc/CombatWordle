global using System.Diagnostics;
global using System.Text;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Input;
global using System.Windows.Media;

namespace CombatWordle
{
    public partial class MainWindow : Window
    {
        private readonly Stopwatch Uptime = Stopwatch.StartNew();
        private double lastTime;

        private bool WindowDragging = false;
        private Point DragOffset;
        private readonly HashSet<Key> PressedKeys = [];

        private GameState game;
        private Culler culler;
        private Renderer renderer;

        private List<EntityData> visible = [];
        private List<EntityData> hidden = [];

        private Map map => game.Map;
        private Player player => game.Player;

        private StringBuilder debugInfo = new();

        private void Form_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                WindowDragging = true;

                var mouseLoc = PointToScreen(e.GetPosition(this));
                DragOffset = new Point(mouseLoc.X - Left, mouseLoc.Y - Top);
                Mouse.Capture((UIElement)sender);
            }
        }
        private void Form_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                WindowDragging = false;
                Mouse.Capture(null);
            }
        }
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowDragging)
                {
                    var screenLoc = PointToScreen(e.GetPosition(this));

                    Left = screenLoc.X - DragOffset.X;
                    Top = screenLoc.Y - DragOffset.Y;
                }
            }
        }
        private void TitleText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                if (sender is TextBlock tb)
                    tb.Foreground = QOL.RandomColor();
            }
            else if (e.ChangedButton == MouseButton.Left)
                Form_MouseDown(sender, e);
        }
        private void ClosingButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void PlayButton_Click(object sender, RoutedEventArgs e) => StartGame();
        private void DebugText_MouseUp(object sender, MouseButtonEventArgs e) => DebugText.Visibility = e.ChangedButton == MouseButton.Right ? Visibility.Hidden : DebugText.Visibility;
        private void Window_KeyDown(object sender, KeyEventArgs e) => PressedKeys.Add(e.Key);
        private void Window_KeyUp(object sender, KeyEventArgs e) => PressedKeys.Remove(e.Key);

        public MainWindow()
        {
            InitializeComponent();

            lastTime = Uptime.Elapsed.TotalSeconds;

            TitleText.Foreground = QOL.RandomColor();
            TitleTextShadow.Foreground = QOL.RandomColor();

            game = new GameState();
            culler = new(game.AllEntityData);
            renderer = new(GameCanvas);

            GameCanvas.Children.Add(map);
            Canvas.SetLeft(map, 0);
            Canvas.SetTop(map, 0);

            StartGame();
        }

        private void PlayerMovement(double dt)
        {
            double dx = 0;
            double dy = 0;

            if (PressedKeys.Contains(Key.W)) dy -= 1;
            if (PressedKeys.Contains(Key.A)) dx -= 1;
            if (PressedKeys.Contains(Key.S)) dy += 1;
            if (PressedKeys.Contains(Key.D)) dx += 1;

            if (dx != 0 || dy != 0)
            {
                double totalVectorLength = Math.Sqrt(dx * dx + dy * dy);

                dx /= totalVectorLength;
                dy /= totalVectorLength;
            }

            dx *= player.Speed * dt;
            dy *= player.Speed * dt;

            Point pos = player.WorldPos;
            Size size = player.Size;
            Rect newRect;

            double leftEdge = map.Thickness;
            double topEdge = map.Thickness;
            double rightEdge = map.Width - map.Thickness - player.Width;
            double bottomEdge = map.Height - map.Thickness - player.Height;

            pos.X += dx;
            newRect = new Rect(pos, size);
            foreach (Entity collider in game.Colliders
                .Where(c => c.CollisionType != CollisionType.Live
                && Math.Abs(c.WorldPos.X - pos.X) < player.Area / 15))
            {
                if (newRect.IntersectsWith(collider.Rect))
                {
                    if (dx > 0)
                        pos.X = collider.WorldPos.X - player.Width - 0.1;
                    else if (dx < 0)
                        pos.X = collider.WorldPos.X + collider.Width + 0.1;
                    newRect = new Rect(pos, size);
                }
            }

            pos.Y += dy;
            newRect = new Rect(pos, size);
            foreach (Entity collider in game.Colliders
                .Where(c => c.CollisionType != CollisionType.Live
                && Math.Abs(c.WorldPos.Y - pos.Y) < player.Area / 15))
            {
                var colliderRect = new Rect(collider.WorldPos, collider.Size);
                if (newRect.IntersectsWith(colliderRect))
                {
                    if (dy > 0)
                        pos.Y = collider.WorldPos.Y - player.Height - 0.1;
                    else if (dy < 0)
                        pos.Y = collider.WorldPos.Y + collider.Height + 0.1;
                    newRect = new Rect(pos, size);
                }
            }

            pos.X = Math.Max(leftEdge, Math.Min(pos.X, rightEdge));
            pos.Y = Math.Max(topEdge, Math.Min(pos.Y, bottomEdge));

            player.WorldPos = pos;

            //debug
            debugInfo.Clear();
            debugInfo.Append($"dx: {dx:F1}\ndy: {dy:F1}\n");
        }
        private void CameraMovement()
        {
            double px = player.WorldPos.X + player.Width / 2;
            double py = player.WorldPos.Y + player.Height / 2;

            double screenCenterX = ActualWidth / 2;
            double screenCenterY = ActualHeight / 2;

            double offsetX = screenCenterX - px;
            double offsetY = screenCenterY - py;

            offsetX = Math.Min(0, Math.Max(offsetX, ActualWidth - game.Map.Width));
            offsetY = Math.Min(0, Math.Max(offsetY, ActualHeight - game.Map.Height));

            CameraTransform.X = offsetX;
            CameraTransform.Y = offsetY;

            debugInfo.Append($"px: {px:F1}\npy: {py:F1}\n");
        }
        public void Move(double dt)
        {
            PlayerMovement(dt);
            CameraMovement();
        }
        private void HandleHotKeys()
        {
            if (PressedKeys.Remove(Key.R))
                game.AddTestRock();
            if (PressedKeys.Remove(Key.G))
                game.PopulateMap<Rock>(2000);
        }
        private void OnRender(object sender, EventArgs e)
        {
            double now = Uptime.Elapsed.TotalSeconds;
            double dt = Math.Min(now - lastTime, 0.05);
            lastTime = now;

            Update(dt);
        }

        private void Update(double dt)
        {
            HandleHotKeys();
            Move(dt);

            Rect viewport = new(
                -CameraTransform.X,
                -CameraTransform.Y,
                ActualWidth,
                ActualHeight
                );

            visible.Clear();
            hidden.Clear();

            culler.Cull(viewport, visible, hidden);
            renderer.RenderEntities(visible, hidden);

            debugInfo.Append($"dt: {dt:F3}\n");
            DebugText.Text = debugInfo.ToString();
        }

        private async void StartGame()
        {
            StartMenu.Visibility = Visibility.Hidden;
            GameCanvas.Visibility = Visibility.Visible;

            CompositionTarget.Rendering += OnRender;
        }
    }
}