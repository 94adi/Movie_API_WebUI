using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Movie.WebUI.Areas.Admin.TagHelpers;

[HtmlTargetElement("label", Attributes = "asp-for")]
public class RequiredLabelTagHelper : TagHelper
{
    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For == null)
            return;

        var isRequired = For.ModelExplorer.Metadata.ValidatorMetadata
            .OfType<RequiredAttribute>()
            .Any();

        if (isRequired)
        {
            output.Content.AppendHtml(" <span style='color:red'>*</span>");
        }
    }
}
