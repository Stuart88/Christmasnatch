using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Path = System.IO.Path;
using System.Windows.Threading;
using System.Reflection;

namespace Christmasnatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitGameData();
            InitializeComponent();
            InitPage();
        }

        private PageModel _pageModel;

        private void InitPage()
        {
            SetPage("0");
        }

        private void InitGameData()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly
                .GetManifestResourceNames()
                .Where(x => x.EndsWith(".txt"))
                .Select(x => x[21..])
                .ToArray();
            GameData.Instance.AllPages = resourceNames.Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
        }

        private TextBlock PageText;

        private void SetPage(string id)
        {
            _pageModel = new PageModel(id);

            GameState.Instance.VisitedPages.Add(id);

            if (!string.IsNullOrEmpty(_pageModel.ImageASCII))
            {
                ImageText.Text = _pageModel.ImageASCII;
                ImageRow.Height = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                ImageText.Text = string.Empty;
                //ImageRow.Height = new GridLength(0);
            }

            PageDetails.Children.Clear();
            animationTargetText = _pageModel.MainText; 
            currentIndex = 0;
            PageText = new TextBlock
            {
                Text = string.Empty,
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)FindResource("PageTextStyle")
            };
            PageDetails.Children.Add(PageText);

            foreach (var opt in _pageModel.Options)
            {
                var btn = new Button
                {
                    Content = opt.OptionText,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };

                btn.Style = (Style)FindResource("OptionButtonStyle");
                if (GameState.Instance.VisitedPages.Contains(opt.PageID) && opt.PageID != "0" && opt.PageID != "999")
                {
                    btn.ToolTip = new ToolTip { Content = GetTooltipText(opt.PageID) };
                    ToolTipService.SetShowOnDisabled(btn, true);
                    ToolTipService.SetInitialShowDelay(btn, 0);
                }

                btn.Click += (e, s) =>
                {
                    SetPage(opt.PageID);
                    GameState.Instance.CurrentPage = opt.PageID;
                };


                PageDetails.Children.Add(btn);
            }

            CompletedText.Text = GameState.Instance.CompletedPercent;
            AnimateText();
        }

        private string GetTooltipText(string pageId)
        {
            int visits = GameState.Instance.VisitedPages.Count(x => x == pageId);

            if (visits == 1)
                return "You have been this way, traveller";

            return $"You have been this way {visits} times, traveller";
        }

        private void AnimateText()
        {
            var animation = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };

            animation.Tick += Animation_Tick;
            animation.Start();
        }

        private int currentIndex;
        private string animationTargetText = string.Empty;

        private void Animation_Tick(object sender, EventArgs e)
        {
            if (currentIndex <= animationTargetText.Length)
            {
                PageText.Text = animationTargetText.Substring(0, currentIndex);
                currentIndex++;
            }
            else
            {
                // Animation completed, perform additional actions if needed
                ((DispatcherTimer)sender).Stop();
            }
        }
    }
}