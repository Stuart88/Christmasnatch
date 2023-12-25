using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Christmasnatch
{
    public class PageModel
    {
        public PageModel(string pageNum)
        {
            this.ID = pageNum;

            string[] pageLines = GetPageData(pageNum);

            StringBuilder sb = new StringBuilder();
            bool onMainText = true;

            foreach (string line in pageLines)
            {
                if (line.StartsWith("IMAGE"))
                {
                    this.ImageASCII = line[5..];
                    continue;
                }

                if (onMainText)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        this.MainText = sb.ToString();
                        onMainText = false;
                        sb.Clear();
                    }
                    sb.Append(line);
                }
                else
                {
                    if(string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    var pageOption = new OptionModel();
                    var optionsDetails = line.Split("--");
                    pageOption.PageID = optionsDetails[0];
                    pageOption.OptionText = string.Join("", optionsDetails[1..]);
                    Options.Add(pageOption);
                }
            }
        }
        public string ID { get; set; } = string.Empty;
        public string MainText { get; set; } = string.Empty;
        public string ImageASCII { get; set; } = string.Empty;
        public List<OptionModel> Options { get; set; } = new();

        private string[] GetPageData(string pageId)
        {
            var resourceName = $"Christmasnatch.Pages.{pageId}.txt";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string text = reader.ReadToEnd();
                        return text.Split("\r\n");
                    }
                }
            }
            return [];
        }

    }
}
