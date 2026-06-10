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
        public async Task<ResponseModel<PersonDto>> GetPersonById(Guid personId)
            => await personService.GetPersonByIdAsync(personId);

        [HttpPut]
        public async Task<ResponseModel<bool>> UpdatePerson(PersonDto personDto)
            => await personService.UpdatePersonAsync(personDto);

        [HttpPost]
        public async Task<ResponseModel<bool>> AddSpouse(SpouseDto spouseDto)
            => await personService.AddSpouseAsync(spouseDto);

        [HttpGet]
        public async Task<ResponseModel<PagedResult<SpouseViewDto>>> GetAllSpouses([FromQuery] FilterModel filterModel)
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
    }
}
