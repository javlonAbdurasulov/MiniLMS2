using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;

namespace MiniLMS.Application.External
{
    public interface IExternalAPIs
    {
        Task<ResponseModel<Catfact>> GetAllCatFacts();
        Task<ResponseModel<Weather>> CreateWeather(Weather weather);
        Task<ResponseModel<List<Weather>>> GetWeather();
        Task<ResponseModel<Weather>> updateWeather(Weather weather);
        Task<ResponseModel<bool>> deleteWeather(int id);

    }
}