
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

            var generation = await dbContext.Generations.FindAsync(personDto.GenerationId);

            if (generation != null)
                person.Generation = generation;

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

        //public async Task<ResponseModel<List<PersonDto>>> GetPersonByIdAsync(Guid sulolaId)
        //{
        //    //var person = await dbContext.Persons.FindAsync(personId);

        //    //if (person == null)
        //    //    return new("Person not found", HttpStatusCode.NotFound);

        //    var persons = await dbContext.Persons.Where(x=>x.GenerationId == sulolaId).OrderBy(x=>x.GenerationLevel).ToListAsync();

        //    var personDto = mapper.Map<List<PersonDto>>(persons);

        //    var data = new Dictionary<int, List<PersonDto>>();


        //    for (int i = 1; i <= 7; i++)
        //    {
        //        var avlod = personDto.Where(x => x.GenerationLevel == i).ToList();
        //        data.Add(i, avlod);
        //        var result = new List<PersonDto>();
        //        foreach (var item in avlod)
        //        {
        //          result.Add(GetAllChildNormativeDoc(personDto, item.Id));
        //        }

        //    }

        //    return new(personDto);
        //}

        //private List<PersonDto> GetAllChildNormativeDoc(List<PersonDto> persons, Guid? parentId)
        //{
        //    var result = new List<PersonDto>();

        //    var children = persons.Where(d => d.ParentId == parentId).ToList();

        //    foreach (var child in children)
        //    {
        //        result.Add(child);
        //        result.AddRange(GetAllChildNormativeDoc(persons, child.Id)); // Rekursiv chaqirish
        //    }

        //    return result;
        //}


        public async Task<ResponseModel<List<PersonDto1>>> GetPersonByIdAsync(Guid sulolaId)
        {
            var persons = await dbContext.Persons
                .Where(x => x.GenerationId == sulolaId)
                .OrderBy(x => x.GenerationLevel)
                .ThenBy(x => x.ChildOrder)
                .ToListAsync();

            var personDtos = mapper.Map<List<PersonDto1>>(persons);

            var roots = personDtos
                .Where(x => x.ParentId == null || !personDtos.Any(p => p.Id == x.ParentId))
                .OrderBy(x => x.ChildOrder)
                .ToList();

            foreach (var root in roots)
            {
                BuildTree(personDtos, root);
            }

            return new(roots);
        }

        private void BuildTree(List<PersonDto1> allPersons, PersonDto1 parent)
        {
            parent.Children = allPersons
                .Where(x => x.ParentId == parent.Id)
                .OrderBy(x => x.ChildOrder)
                .ToList();

            foreach (var child in parent.Children)
            {
                BuildTree(allPersons, child);
            }
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

            var generation = await dbContext.Generations.FindAsync(personDto.GenerationId);

            if (generation != null)
                person.Generation = generation;

            person.FirstName = personDto.FirstName;
            person.LastName = personDto.LastName;
            person.MiddleName = personDto.MiddleName;
            person.ChildOrder = personDto.Order;
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
            person.GenerationId = personDto.GenerationId;

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
            var spouses = dbContext.Spouses
                .Include(x => x.Husband)
                .Include(x => x.Wife)
                .AsQueryable();

            if (filterModel.Filters != null && filterModel.Filters.Any())
            {
                var personIdsQuery = dbContext.Persons.AsQueryable().ApplyFilters(filterModel.Filters).Select(x => x.Id);
                spouses = spouses.Where(x => (x.HusbandId != null && personIdsQuery.Contains(x.HusbandId.Value)) 
                                          || (x.WifeId != null && personIdsQuery.Contains(x.WifeId.Value)));
            }

            var result = new PagedResult<SpouseViewDto>();

            result.TotalItems = await spouses.CountAsync();

            if (!string.IsNullOrWhiteSpace(filterModel.SortField))
            {
                spouses = spouses.ApplySorting(filterModel.SortField, filterModel.IsDescending);
            }

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
            person.ChildOrder = assignParent.Order;

            dbContext.Persons.Update(person);

            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }

        public async Task<ResponseModel<Spouse>> GetSpouseByIdAsync(Guid spouseId)
        {
            var spouse = await dbContext.Spouses.Include(x => x.Childrens).FirstOrDefaultAsync(x => x.Id == spouseId);

            if (spouse == null)
                return new("Spouse not found.", HttpStatusCode.NotFound);

            return new(spouse);
        }

        public async Task<ResponseModel<Guid>> CreateGenerationAsync(GenerationDto generationDto)
        {
            var generation = mapper.Map<Generation>(generationDto);

            if (generation == null)
                return new("Dto not found.", HttpStatusCode.NotFound);

            generation.Id = Guid.NewGuid();
            generation.CreatedAt = DateTime.UtcNow.AddHours(5);
            await dbContext.Generations.AddAsync(generation);

            if (await dbContext.SaveChangesAsync() > 0)
                return new(generation.Id);

            return new("Error", HttpStatusCode.BadRequest);
        }

        public async Task<ResponseModel<bool>> AssignGenerationAsync(AssignGenerationDto assignGeneration)
        {
            var person = await dbContext.Persons.FindAsync(assignGeneration.PersonId);

            if (person == null)
                return new("Person not found.", HttpStatusCode.NotFound);

            var generation = await dbContext.Generations.FindAsync(assignGeneration.GenerationId);

            if (generation == null)
                return new("Generation not found.", HttpStatusCode.NotFound);

            person.GenerationId = generation.Id;
            person.Generation = generation;

            dbContext.Persons.Update(person);

            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }

        public async Task<ResponseModel<GenerationViewDto>> GetByGenerationId(Guid generationId)
        {
            var correntDate = DateTime.UtcNow.AddHours(5);
            var generation = await dbContext.Generations.Where(x => x.Id == generationId && x.IsActive)
                .Include(x => x.Persons).FirstOrDefaultAsync();

            if (generation == null)
                return new("Generation not found.", HttpStatusCode.NotFound);

            if (generation.ExpireDate < correntDate)
            {
                generation.IsActive = false;
                dbContext.Generations.Update(generation);
                await dbContext.SaveChangesAsync();
                return new("Generation expired.", HttpStatusCode.BadRequest);
            }

            var persons = generation.Persons.Select(x => new PersonDto
            {
                Id = x.Id,
                Pinfl = x.Pinfl,
                Gender = x.Gender,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                Biography = x.Biography,
                BirthDate = x.BirthDate,
                BirthPlace = x.BirthPlace,
                GenerationId = x.GenerationId,
                Description = x.Description,
                GenerationLevel = x.GenerationLevel,
                IsAlive = x.IsAlive,
                PhoneNumber = x.PhoneNumber,
                PhotoUrl = x.PhotoUrl,
                InstagramLink = x.InstagramLink,
                TelegramLink = x.TelegramLink,
                DeathDate = x.DeathDate,
                Order = x.ChildOrder,
                ParentSpouseId = x.ParentSpouseId
            }).ToList();

            var generationDto = new GenerationViewDto
            {
                Id = generation.Id,
                Name = generation.Name,
                Description = generation.Description,
                Persons = persons
            };

            return new(generationDto);
        }

        public async Task<ResponseModel<List<GenerationViewDto>>> GetAllGenerationsAsync()
        {
            var generations = await dbContext.Generations
                .Include(x => x.Persons)
                .ToListAsync();

            var result = generations.Select(generation => new GenerationViewDto
            {
                Id = generation.Id,
                Name = generation.Name,
                Description = generation.Description,
                PersonCount = generation.Persons?.Count() ?? 0
            }).ToList();

            return new(result);
        }

        public async Task<ResponseModel<bool>> DeleteGenerationAsync(Guid generationId)
        {
            var generation = await dbContext.Generations.FindAsync(generationId);

            if (generation == null)
                return new("Generation not found.", HttpStatusCode.NotFound);

            dbContext.Generations.Remove(generation);
            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }

        public async Task<ResponseModel<bool>> UpdateGenerationAsync(GenerationDto generationDto)
        {
            var generation = await dbContext.Generations.FindAsync(generationDto.Id);

            if (generation == null)
                return new("Generation not found.", HttpStatusCode.NotFound);

            generation.Name = generationDto.Name;
            generation.Description = generationDto.Description;
            generation.ExpireDate = generationDto.ExpireDate;

            dbContext.Generations.Update(generation);
            if (await dbContext.SaveChangesAsync() > 0)
                return new(true);

            return new(false);
        }
    }
}
