﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.Tours;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IKeyPointRepository
{
    List<KeyPoint> GetByTourId(long tourId);
    KeyPoint Create(KeyPoint keyPoint);
    KeyPoint Update(KeyPoint keyPoint);
    void Delete(long id);
    KeyPoint Get(long id);
    KeyPoint GetFirstByTourId(long tourId);
    PagedResult<KeyPoint> GetPaged(int page, int pageSize);
}
