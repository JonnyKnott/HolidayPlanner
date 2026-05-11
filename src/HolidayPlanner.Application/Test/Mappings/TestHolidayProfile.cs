using AutoMapper;
using HolidayPlanner.Application.Test.DTOs;
using HolidayPlanner.Domain.Test;

namespace HolidayPlanner.Application.Test.Mappings;

public sealed class TestHolidayProfile : Profile
{
    public TestHolidayProfile()
    {
        CreateMap<TestHoliday, TestHolidayDto>();
    }
}
