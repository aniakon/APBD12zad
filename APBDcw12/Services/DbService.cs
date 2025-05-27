using APBDcw12.Data;
using APBDcw12.DTOs;
using APBDcw12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBDcw12.Services;

public class DbService : IDbService
{
    private readonly Apbd12Context _context;

    public DbService(Apbd12Context context)
    {
        _context = context;
    }

    public async Task<tripPageDTO> getTripPagesAsync(int page, int pageSize)
    {
        var all = await _context.Trips.CountAsync();
        var res = new tripPageDTO()
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = all,
            Trips = new List<tripDto>()
        };
        
        var trips = await _context.Trips.OrderByDescending(d => d.DateFrom)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(r => new tripDto()
            {
                Name = r.Name,
                Description = r.Description,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo,
                MaxPeople = r.MaxPeople,
                Countries = r.IdCountries.Select(c => new countryDTO() { Name = c.Name }).ToList(),
                Clients = r.ClientTrips.Select(e => new clientDTO()
                {
                    FirstName = e.IdClientNavigation.FirstName,
                    LastName = e.IdClientNavigation.LastName,
                }).ToList(),
            }).ToListAsync();
        res.Trips = trips;
        return res;
    }

    public async Task deleteClientAsync(int id)
    {
        var num = await _context.ClientTrips.Where(e => e.IdClient == id).CountAsync();
        if (num > 0) throw new Exception($"Klient {id} ma wycieczki");
        var client = await _context.Clients.Where(e => e.IdClient == id).FirstOrDefaultAsync();
        if (client == null) throw new Exception($"Klient {id} nie istnieje");
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }

    public async Task addClientToTripAsync(int idTrip, clientTripDTO clientTripDto)
    {
        // czy klient o tym peselu juz istnieje
        var existing = await _context.Clients.AnyAsync(e => e.Pesel == clientTripDto.Pesel);
        if (existing)
            throw new Exception("Klient o tym numerze PESEL już istnieje.");
        
        // czy kleint o tym peselu jest juz zapisany na dana wycieczke
        var idPes = await _context.Clients.FirstOrDefaultAsync(e => e.Pesel == clientTripDto.Pesel);
        if (await _context.ClientTrips.AnyAsync(e => e.IdTrip == idTrip && e.IdClient == idPes.IdClient))
            throw new Exception("Ten klient już jest zapisany na podaną wycieczkę.");
        
        // czy wuceczka istneije i datefrom jest w przyszlosci
        var trip = await _context.Trips.FirstOrDefaultAsync(e => e.IdTrip == idTrip);
        if (trip == null) throw new Exception("Dana wycieczka nie istnieje.");
        if (trip.DateFrom < DateTime.Today) throw new Exception("Dana wycieczka jest w przeszłości.");
        
        // dodaje klienta
        var lastId = _context.Clients.Max(e => e.IdClient);
        await _context.Clients.AddAsync(new Client()
        {
            IdClient = lastId + 1,
            FirstName = clientTripDto.FirstName,
            LastName = clientTripDto.LastName,
            Email = clientTripDto.Email,
            Telephone = clientTripDto.Telephone,
            Pesel = clientTripDto.Pesel,
        });
        await _context.SaveChangesAsync();
        // dodaje client_trip
        await _context.ClientTrips.AddAsync(new ClientTrip()
        {
            IdClient = lastId + 1,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = clientTripDto.PaymentDate != null ? clientTripDto.PaymentDate : null,
        });
        await _context.SaveChangesAsync();
    }
}