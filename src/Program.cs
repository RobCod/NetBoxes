using System;

namespace NetBoxes
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Main game = new Main())
            {
                Context.Game = game;
                game.Run();
            }
        }
    }
}
