using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SulolaMap.Api.Controllers
{
    public class PersonController : BaseController
    {
        private readonly IPersonService personService;

        public PersonController(IPersonService personService)
        {
            this.personService = personService;
        }

        [HttpPost]
        public async Task<ResponseModel<bool>> CreatePerson(PersonDto personDto)
            => await personService.CreatedPersonAsync(personDto);

        [HttpDelete]
        public async Task<ResponseModel<bool>> DeletePerson(Guid personId)
            => await personService.DeletePersonAsync(personId);

        [HttpPost]
        public async Task<ResponseModel<PagedResult<PersonDto>>> GetAllPersons([FromBody] FilterModel filterModel)
            => await personService.GetAllPersonsAsync(filterModel);

        [HttpGet]
        public async Task<ResponseModel<List<PersonDto1>>> GetPersonById(Guid personId)
            => await personService.GetPersonByIdAsync(personId);

        [HttpPut]
        public async Task<ResponseModel<bool>> UpdatePerson(PersonDto personDto)
            => await personService.UpdatePersonAsync(personDto);

        [HttpPost]
        public async Task<ResponseModel<bool>> AddSpouse(SpouseDto spouseDto)
            => await personService.AddSpouseAsync(spouseDto);

        [HttpPost]
        public async Task<ResponseModel<PagedResult<SpouseViewDto>>> GetAllSpouses([FromBody] FilterModel filterModel)
            => await personService.GetAllSpousesAsync(filterModel);

        [HttpPut]
        public async Task<ResponseModel<bool>> AssignParents(AssignParentDto assignParent)
            => await personService.AssignParentsAsync(assignParent);

        [HttpPost]
        public async Task<ResponseModel<Guid>> CreateGeneration(GenerationDto generationDto)
           => await personService.CreateGenerationAsync(generationDto);

        [HttpPut]
        public async Task<ResponseModel<bool>> AssignGeneration(AssignGenerationDto assignGeneration)
           => await personService.AssignGenerationAsync(assignGeneration);

        [HttpGet]
        public async Task<ResponseModel<GenerationViewDto>> GetByGenerationId(Guid generationId)
            => await personService.GetByGenerationId(generationId);

        [HttpGet]
        public async Task<ResponseModel<List<GenerationViewDto>>> GetAllGenerations()
            => await personService.GetAllGenerationsAsync();

        [HttpDelete]
        public async Task<ResponseModel<bool>> DeleteGeneration(Guid generationId)
            => await personService.DeleteGenerationAsync(generationId);

        [HttpPut]
        public async Task<ResponseModel<bool>> UpdateGeneration(GenerationDto generationDto)
            => await personService.UpdateGenerationAsync(generationDto);
    }
}
