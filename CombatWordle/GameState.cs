namespace CombatWordle
{
    public class GameState
    {
        public Map Map { get; private set; }

        public bool GameOver { get; private set; } = false;

        public Player Player { get; private set; }

        public List<Rock> Colliders { get; set; } = [];
        public List<Rock> Rocks { get; set; } = [];

        public double MapCenterX => Map.Width / 2 - Player.Width / 2;
        public double MapCenterY => Map.Height / 2 - Player.Height / 2;
        public Point MapCenter => new(MapCenterX, MapCenterY);

        public GameState(int mapWidth = 10000, int mapHeight = 10000)
        {
            Map = new(mapWidth, mapHeight);
            AddPlayer();
        }

        private void AddPlayer()
        {
            Player = new(80, 80);
            Player.WorldPos = MapCenter;
        }

        public Rock rock;
        public void AddRock()
        {
            rock = new();
            rock.WorldPos = new(Player.X, Player.Y - Player.Height - rock.Height);
            Rocks.Add(rock);
            Colliders.Add(rock);
        }

        public void PopulateMap(Entity entity, decimal percentage)
        {

        }
    }
}