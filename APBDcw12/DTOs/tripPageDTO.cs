namespace APBDcw12.DTOs;

public class tripPageDTO
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<tripDto> Trips { get; set; }
}

public class tripDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<countryDTO> Countries { get; set; }
    public List<clientDTO> Clients { get; set; }
}

public class countryDTO
{
    public string Name { get; set; }
}

public class clientDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}