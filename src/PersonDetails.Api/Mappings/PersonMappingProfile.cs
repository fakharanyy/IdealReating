using AutoMapper;
using PersonDetails.Api.Data.Entities;
using PersonDetails.Api.Models;

namespace PersonDetails.Api.Mappings;

public class PersonMappingProfile : Profile
{
    public PersonMappingProfile()
    {
        CreateMap<Person, PersonResponseModel>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => GetFirstName(src.Name)))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => GetLastName(src.Name)))
            .ForMember(dest => dest.TelephoneCode, opt => opt.MapFrom(src => GetTelephoneCode(src.TelephoneNumber)))
            .ForMember(dest => dest.TelephoneNumber,
                opt => opt.MapFrom(src => GetTelephoneNumber(src.TelephoneNumber)));
    }

    private string GetFirstName(string fullName)
    {
        return fullName?.Split(' ')[0];
    }

    private string GetLastName(string fullName)
    {
        var parts = fullName?.Split(' ');
        return parts?.Length > 1 ? parts[1] : string.Empty;
    }

    private string GetTelephoneCode(string telephoneNumber)
    {
        if (string.IsNullOrEmpty(telephoneNumber))
            return string.Empty;

        var parts = telephoneNumber.Split('-');
        return parts.Length > 1 ? parts[0] : string.Empty;
    }

    private string GetTelephoneNumber(string telephoneNumber)
    {
        if (string.IsNullOrEmpty(telephoneNumber))
            return string.Empty;

        var parts = telephoneNumber.Split('-');
        return parts.Length > 1 ? parts[1] : telephoneNumber;
    }
}