namespace CombatWordle
{
    public struct EditorDTO
    {
        public (int x, int y) Cell { get; set; } = (-1, -1);
        public Brush Color;

        public EditorDTO((int x, int y) cell, Brush color)
        {
            Cell = cell;
            Color = color;
        }
    }
}