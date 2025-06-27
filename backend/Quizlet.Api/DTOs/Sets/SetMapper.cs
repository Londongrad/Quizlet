using Quizlet.Domain.Entities;

namespace Quizlet.Api.DTOs.Sets
{
    public static class SetMapper
    {
        public static SetResponse ToResponse(this Set set)
        {
            return new SetResponse(
                set.Id,
                set.Title,
                set.Description,
                set.CreatedAt,
                set.Words?.Count ?? 0
            );
        }
    }
}
