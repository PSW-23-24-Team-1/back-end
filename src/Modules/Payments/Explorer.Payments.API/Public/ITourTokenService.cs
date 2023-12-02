using Explorer.Payments.API.Dtos;
using FluentResults;

namespace Explorer.Payments.API.Public;

public interface ITourTokenService
{
    Result<TourTokenResponseDto> AddToken(TourTokenCreateDto token, double totalPrice);
    Result<List<TourTokenResponseDto>> GetTouristsTokens(long touristId);
    Result<List<long>> GetTouristToursId(long touristId);
}
