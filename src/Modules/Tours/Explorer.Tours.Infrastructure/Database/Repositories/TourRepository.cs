﻿using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TourRepository : ITourRepository
    {
        protected readonly ToursContext DbContext;
        private readonly DbSet<Tour> _dbSet;

        public TourRepository(ToursContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<Tour>();
        }


        public List<Equipment> GetEquipment(long tourId)
        {
            ///SAMO JE OVO URADJENO TREBA OSTATAK TESTIRATI
            var equipmentList = DbContext.Equipment
                                .Where(e => e.Tours.Any(t => t.Id == tourId))
                                .ToList();
            return equipmentList;
        }

        public void AddEquipment(long tourId, long equipmentId)
        {
            var tour = DbContext.Tours
                                .Include(t => t.EquipmentList)
                                .Single(t => t.Id == tourId);
            var equipment = DbContext.Equipment
                                     .Single(e => e.Id == equipmentId);
            tour.EquipmentList.Add(equipment);
            
            DbContext.SaveChanges();
        }

        public void DeleteEquipment(long tourId, long equipmentId)
        {
            var tour = DbContext.Tours
                                .Include(t => t.EquipmentList)
                                .Single(t => t.Id == tourId);
            var equipment = DbContext.Equipment
                                     .Single(e => e.Id == equipmentId);
            tour.EquipmentList.Remove(equipment);

            DbContext.SaveChanges();
        }
    }
}