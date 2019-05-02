using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pagination.Interfaces
{
    public interface IPaginationBuilder
    {
        TagBuilder BuildBody();
        TagBuilder GenerateLink(int page, string text, bool isActive, bool isDisabled, DataTransferObject.UrlInfo urlInfo);
    }
}
