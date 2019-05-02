namespace Pagination.DataTransferObject
{
    public class UrlInfo
    {
        public Microsoft.AspNetCore.Mvc.IUrlHelper UrlHelper { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
