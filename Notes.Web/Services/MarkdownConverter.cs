using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Services
{
    public class MarkdownConverter
    {
        public string ToHtml(string content)
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            return Markdown.ToHtml(content, pipeline);
        }
    }
}
