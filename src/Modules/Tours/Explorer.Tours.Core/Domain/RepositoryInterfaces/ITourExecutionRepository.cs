﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourExecutionRepository
    {
        TourExecution Get(long tourId, long touristId);
        TourExecution Add(TourExecution tourExecution);
        TourExecution Abandon(long tourId, long touristId);
        TourExecution UpdateNextKeyPoint(long tourExecutionId, long nextKeyPointId);
        TourExecution CompleteTourExecution(long tourExecutionId);
    }
}