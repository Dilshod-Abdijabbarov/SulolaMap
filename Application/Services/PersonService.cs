
using AutoMapper;
using Domain.Entity;
using Domain.Models;
using Infrastructure.db;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Services
{
    public class PersonService
    {
        private readonly SulolaDbContext dbContext;
        private readonly IMapper mapper;
        public PersonService(SulolaDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<bool>> CreatedPerson(PersonDto personDto)
        {
            var person = mapper.Map<Person>(personDto);
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
            var persons = dbContext.Persons;

            var result = new PagedResult<PersonDto>();

            var data = await persons
                .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
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
            person.ParentSpouseId = personDto.ParentSpouseId;

            dbContext.Persons.Update(person);

            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }
    }
}
