using Markdig;

namespace Notes.Web.Services
{
    public class MarkdownConverter
    {
        public string ToHtml(string content)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(content, pipeline);
        }
    }
}
