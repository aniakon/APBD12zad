using APBDcw12.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace APBDcw12.Services;

public interface IDbService
{
    Task<tripPageDTO> getTripPagesAsync(int page, int pageSize);
    Task deleteClientAsync(int id);
    Task addClientToTripAsync(int idTrip, clientTripDTO clientTripDto);
}