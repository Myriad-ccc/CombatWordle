namespace CombatWordle
{
    public class Culler
    {
        private readonly List<EntityData> Entities;

        public Culler(List<EntityData> entities)
        {
            Entities = entities;
        }

        public void Cull(Rect viewport, List<EntityData> visible, List<EntityData> hidden)
        {
            viewport.Inflate(-200, -200);
            foreach (var e in Entities)
            {
                if (viewport.IntersectsWith(e.Rect))
                {
                    if (!e.Visible)
                    {
                        e.Visible = true;
                        visible.Add(e);
                    }
                }
                else
                {
                    if (e.Visible)
                    {
                        e.Visible = false;
                        hidden.Add(e);
                    }
                }
            }
        }
    }
}
