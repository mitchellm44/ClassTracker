﻿using KadGen.ClassTracker.Domain;
using KadGen.Common.Repository;
using System.Linq;

namespace KadGen.ClassTracker.Repository
{
    public class OrganizationRepository
            : BaseEfRepository<Organization, int, EfOrganization, ClassTrackerDbContext>
    {
        public OrganizationRepository(ClassTrackerDbContext dbContext)
            : base(dbContext,
                  getDbSet: dc => dc.Organizations,
                  getPKey: o => o.Id,
                  mapEntityToDomain: Mapper.MapEntityToDomain,
                  mapDomainToEntity: Mapper.MapDomainToEntity)
        { }

        internal class Mapper
        {
            public static Organization MapEntityToDomain(EfOrganization entity)
            {
                return entity == null 
                    ? null
                    : new Organization(entity.Id, entity.Name,
                            org => entity.Terms.Select(x 
                                => TermRepository.Mapper.MapEntityToDomainForOrganization(x, org)).ToList(),
                            org => entity.Instructors.Select(x 
                                => InstructorRepository.Mapper.MapEntityToDomainForOrganization(x, org)).ToList(),
                            org => entity.Courses.Select(x 
                                => CourseRepository.Mapper.MapEntityToDomainForOrganization(x, org)).ToList());
            }

            //public static EfOrganization MapDomainToNewEntity(Organization domain)
            //{
            //    var entity = new EfOrganization();
            //    MapDomainToEntity(domain, entity);
            //    return entity;
            //}

            public static void MapDomainToEntity(Organization domain, EfOrganization entity)
            {
                entity.Id = domain.Id;
                entity.Name = domain.Name;
            }
        }
    }
}
