
using Domain.Models;

namespace Application.Interfaces;

public interface IPersonService
{
    Task<ResponseModel<PagedResult<PersonDto>>> GetAllPersonsAsync(FilterModel filterModel);
    Task<ResponseModel<List<PersonDto>>> GetPersonByIdAsync(Guid personId);
    Task<ResponseModel<bool>> CreatedPersonAsync(PersonDto personDto);
    Task<ResponseModel<bool>> DeletePersonAsync(Guid personId);
    Task<ResponseModel<bool>> UpdatePersonAsync(PersonDto personDto);
    Task<ResponseModel<bool>> AddSpouseAsync(SpouseDto spouseDto);
    Task<ResponseModel<PagedResult<SpouseViewDto>>> GetAllSpousesAsync(FilterModel filterModel);
    Task<ResponseModel<bool>> AssignParentsAsync(AssignParentDto assignParent);
    Task<ResponseModel<Guid>> CreateGenerationAsync(GenerationDto generationDto);
    Task<ResponseModel<bool>> AssignGenerationAsync(AssignGenerationDto assignGeneration);
    Task<ResponseModel<GenerationViewDto>> GetByGenerationId(Guid generationId);
}
