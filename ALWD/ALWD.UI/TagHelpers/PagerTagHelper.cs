using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ALWD.UI.TagHelpers
{
    [HtmlTargetElement("Pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; }

        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; }

        [HtmlAttributeName("category")]
        public string Category { get; set; }

        [HtmlAttributeName("admin")]
        public bool IsAdmin { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.AddClass("pagination", HtmlEncoder.Default);

            var prevPage = CurrentPage > 1 ? CurrentPage - 1 : 1;
            output.Content.AppendHtml(CreatePageLink("Previous", prevPage));

            for (int i = 1; i <= TotalPages; i++)
            {
                if (i == CurrentPage)
                {
                    TagBuilder link = CreatePageLink(i.ToString(), i);
                    link.AddCssClass("active");
                    output.Content.AppendHtml(link);
                }
                else
                    output.Content.AppendHtml(CreatePageLink(i.ToString(), i));
                
            }

            var nextPage = CurrentPage < TotalPages ? CurrentPage + 1 : TotalPages;
            output.Content.AppendHtml(CreatePageLink("Next", nextPage));
        }

        private TagBuilder CreatePageLink(string text, int pageNumber)
        {
            var link = new TagBuilder("a");
            link.AddCssClass("page-link");
            link.Attributes["href"] = GenerateUrl(pageNumber);
            link.InnerHtml.Append(text);

            var listItem = new TagBuilder("li");
            listItem.AddCssClass("page-item");
            listItem.InnerHtml.AppendHtml(link);

            return listItem;
        }

        private string GenerateUrl(int pageNumber)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            HttpRequest request = httpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.Path}?page={pageNumber}&catregory={Category}";
        }
    }

}
