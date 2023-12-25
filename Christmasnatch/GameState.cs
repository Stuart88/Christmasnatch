using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christmasnatch
{
    public sealed class GameState
    {
        private static readonly Lazy<GameState> lazy = new Lazy<GameState>(() => new GameState());

        public static GameState Instance { get { return lazy.Value; } }

        private GameState()
        {
            GameName = string.Empty;
            VisitedPages = new();
        }

        public string GameName { get; set; }
        public List<string> VisitedPages { get; set; }
        public string CurrentPage { get; set; }
        public string CompletedPercent => GetCompletedPercent();

        private string GetCompletedPercent()
        {
            if (VisitedPages.Count == 0)
                return "Completed 0%";

            int visitsCount = VisitedPages.Distinct().Count();

            return $"{Math.Round(100 * (double)visitsCount / (double)GameData.Instance.AllPages.Count, 0)}% Complete";
        }
    }
}
