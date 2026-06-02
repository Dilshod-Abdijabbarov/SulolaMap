
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entity;
using Domain.Models;
using Infrastructure.db;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly SulolaDbContext dbContext;
        private readonly IMapper mapper;
        public PersonService(SulolaDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<bool>> CreatedPersonAsync(PersonDto personDto)
        {
            var person = mapper.Map<Person>(personDto);

            person.Id = Guid.NewGuid();
            person.CreatedBy = Guid.NewGuid();
            await dbContext.Persons.AddAsync(person);

            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }

        public async Task<ResponseModel<bool>> DeletePersonAsync(Guid personId)
        {
            var person = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null) return new(false);

            dbContext.Persons.Remove(person);
            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }

        public async Task<ResponseModel<PagedResult<PersonDto>>> GetAllPersonsAsync(FilterModel filterModel)
        {
            var persons = dbContext.Persons.AsQueryable();

            filterModel.PageSize = filterModel.PageSize == 0 ? 10 : filterModel.PageSize;

            persons = persons.ApplyFilters(filterModel.Filters);

            var result = new PagedResult<PersonDto>();

            var data = await persons
                .Skip((filterModel.PageNumber) * filterModel.PageSize)
                .Take(filterModel.PageSize)
                .ToListAsync();

            result.TotalItems = persons.Count();

            result.Items = mapper.Map<List<PersonDto>>(data);

            return new(result);
        }

        public async Task<ResponseModel<PersonDto>> GetPersonByIdAsync(Guid personId)
        {
            var person = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
                return new("Person not found", HttpStatusCode.NotFound);

            var personDto = mapper.Map<PersonDto>(person);

            return new(personDto);
        }

        public async Task<ResponseModel<bool>> UpdatePersonAsync(PersonDto personDto)
        {
            var person = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personDto.Id);

            if (person == null)
                return new("Person not found", HttpStatusCode.NotFound);

            if (personDto.ParentSpouseId is not null)
            {
                var parent = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personDto.ParentSpouseId);


            }

            person.FirstName = personDto.FirstName;
            person.LastName = personDto.LastName;
            person.MiddleName = personDto.MiddleName;
            person.Order = personDto.Order;
            person.GenerationLevel = personDto.GenerationLevel;
            person.Gender = personDto.Gender;
            person.BirthDate = personDto.BirthDate;
            person.PhotoUrl = personDto.PhotoUrl;
            person.PhoneNumber = personDto.PhoneNumber;
            person.IsAlive = personDto.IsAlive;
            person.DeathDate = personDto.DeathDate;
            person.Pinfl = personDto.Pinfl;
            person.ParentSpouseId = personDto.ParentSpouseId;
            person.BirthPlace = personDto.BirthPlace;
            person.Biography = personDto.Biography;
            person.TelegramLink = personDto.TelegramLink;
            person.InstagramLink = personDto.InstagramLink;
            person.Description = personDto.Description;

            dbContext.Persons.Update(person);

            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }

        public async Task<ResponseModel<bool>> AddSpouseAsync(SpouseDto spouseDto)
        {
            var husband = await dbContext.Persons.FirstOrDefaultAsync(x => x.Id == spouseDto.HusbandId);

            if (husband == null)
                return new("Husband not found.", HttpStatusCode.NotFound);

            var wife = await dbContext.Persons.FirstOrDefaultAsync(x => x.Id == spouseDto.WifeId);

            if (husband == null)
                return new("Wife not found.", HttpStatusCode.NotFound);

            var spouses = await dbContext.Spouses.Where(x => x.HusbandId == husband.Id).OrderByDescending(x => x.Order).ToListAsync();

            var order = spouses?.FirstOrDefault()?.Order ?? 1;

            if (!spouses.Any(x => x.WifeId == wife?.Id))
            {
                var spouse = new Spouse
                {
                    HusbandId = husband.Id,
                    WifeId = wife?.Id,
                    Order = order,
                    Husband = husband,
                    Wife = wife,
                };

                await dbContext.Spouses.AddAsync(spouse);

                if (await dbContext.SaveChangesAsync() > 0)
                    return new(true);

                return new(false);
            }

            return new("Already added", true, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<PagedResult<SpouseViewDto>>> GetAllSpousesAsync(FilterModel filterModel)
        {
            var spouses = dbContext.Spouses.AsQueryable();

            spouses = spouses.ApplyFilters(filterModel.Filters);

            var result = new PagedResult<SpouseViewDto>();

            result.TotalItems = await spouses.CountAsync();

            var data = await spouses
                .Skip(filterModel.PageNumber * filterModel.PageSize)
                .Take(filterModel.PageSize)
                .ToListAsync();

            result.Items = mapper.Map<List<SpouseViewDto>>(data);

            return new(result);
        }

        public async Task<ResponseModel<bool>> AssignParentsAsync(AssignParentDto assignParent)
        {
            var person = await dbContext.Persons.FirstOrDefaultAsync(x => x.Id == assignParent.PersonId);

            if (person == null)
                return new("Person not found.", HttpStatusCode.NotFound);

            var spouse = await dbContext.Spouses.FirstOrDefaultAsync(x => x.Id == assignParent.SpouseId);

            if (spouse == null)
                return new("Parents not found.", HttpStatusCode.NotFound);

            person.ParentSpouseId = spouse.Id;
            person.BornFromMarriage = spouse;
            person.Order = assignParent.Order;

            dbContext.Persons.Update(person);

            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }

        public async Task<ResponseModel<Spouse>> GetSpouseByIdAsync(Guid spouseId)
        {
            var spouse = await dbContext.Spouses.Include(x=>x.Childrens).FirstOrDefaultAsync(x => x.Id == spouseId);

            if (spouse == null)
                return new("Spouse not found.", HttpStatusCode.NotFound);

            return new(spouse);
        }
    }
}
