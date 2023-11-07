﻿using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IProblemRepository
    {
        PagedResult<Problem> GetByAuthor(int page, int pageSize, List<long> tourIds);
        PagedResult<Problem> GetAll(int page, int pageSize);
        PagedResult<Problem> GetByUserId(int page, int pageSize, long id);
        Problem Get(long id);
        Problem GetByAnswerId(long id);
    }
}
