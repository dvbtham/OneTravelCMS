using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OneTravelCMS.Core.Extensions.TagHelpers
{
    [HtmlTargetElement("tbl-actions")]
    public class TableActionTagHelper : TagHelper
    {
        [HtmlAttributeName("edit-label")]
        public string EditLabel { get; set; }

        [HtmlAttributeName("del-class-name")]
        public string DeleteClassName { get; set; }

        [HtmlAttributeName("del-label")]
        public string DeleteLabel { get; set; }

        [HtmlAttributeName("tag-id")]
        public string DataId { get; set; }

        [HtmlAttributeName("delete-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("tag-name")]
        public string DataName { get; set; }

        [HtmlAttributeName("url")]
        public string Url { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var html = $@"<i class='fa fa-edit text-success'></i><a href='{Url}' data-id='{DataId}' data-name='{DataName}'> {EditLabel}</a> 
            &nbsp;<i class='fa fa-trash text-danger'></i><a href = '#' data-id = '{DataId}' data-name = '{DataName}' data-controller='{Controller}' class='{DeleteClassName}'> {DeleteLabel}</a>";
            output.Content.SetHtmlContent(html);
        }
    }
}
