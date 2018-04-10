using System.Collections.Generic;

namespace OneTravelApi.Responses
{
    public interface IListModelResponse<TModel> : IResponse
    {
        int PageSize { get; set; }

        int PageNumber { get; set; }

        IEnumerable<TModel> Model { get; set; }
    }
}
