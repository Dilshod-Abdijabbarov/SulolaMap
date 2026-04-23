
using Domain.Models;

namespace Application.Interfaces;

public interface IPersonService
{
    Task<ResponseModel<PagedResult<PersonDto>>> GetAllPersonsAsync(FilterModel filterModel);
    Task<ResponseModel<PersonDto>> GetPersonByIdAsync(Guid personId);
    Task<ResponseModel<bool>> CreatedPersonAsync(PersonDto personDto);
    Task<ResponseModel<bool>> DeletePersonAsync(Guid personId);
    Task<ResponseModel<bool>> UpdatePersonAsync(PersonDto personDto);
}
