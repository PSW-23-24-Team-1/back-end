﻿namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IKeyPointRepository
{
    List<KeyPoint> GetByTourId(long tourId);
    KeyPoint Create(KeyPoint keyPoint);
    KeyPoint Update(KeyPoint keyPoint);
    void Delete(long id);
}
