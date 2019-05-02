namespace Pagination.Interfaces
{
    public interface IBuildStrategy
    {
        Microsoft.AspNetCore.Mvc.Rendering.TagBuilder Build(Pagination pagination);
    }
}
