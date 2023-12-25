using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christmasnatch
{
    public sealed class GameData
    {
        private static readonly Lazy<GameData> lazy = new Lazy<GameData>(() => new GameData());

        public static GameData Instance { get { return lazy.Value; } }

        private GameData()
        {
            AllPages = new();
        }

        public List<string> AllPages { get; set; }

    }
}
