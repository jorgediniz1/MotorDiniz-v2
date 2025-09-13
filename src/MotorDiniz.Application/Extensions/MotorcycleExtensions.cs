using MotorDiniz.Application.CQRS.Motorcycles.Commands;
using MotorDiniz.Application.DTOs.Motorcycle;
using MotorDiniz.Domain.Entities;

namespace MotorDiniz.Application.Extensions
{
    public static class MotorcycleExtensions
    {
        public static MotorcycleViewDto? ToDto(this Motorcycle? motorcycle)
        {
            if (motorcycle is null) return null;
            return new MotorcycleViewDto
            {
                Identifier = motorcycle.Identifier,
                Year = motorcycle.Year,
                Model = motorcycle.Model,
                Plate = motorcycle.Plate
            };
        }

        public static IEnumerable<MotorcycleViewDto> ToDto(this IEnumerable<Motorcycle>? list)
        {
            if (list == null) return Enumerable.Empty<MotorcycleViewDto>();

            return list.Select(m => new MotorcycleViewDto
            {
                Identifier = m.Identifier,
                Year = m.Year,
                Model = m.Model,
                Plate = m.Plate
            });
        }

        public static Motorcycle? ToEntity(this MotorcycleCreateDto? dto)
        {
            if (dto is null) return null;
            return new Motorcycle(dto.Identifier, dto.Year, dto.Model, dto.Plate);
        }

        public static CreateMotorcycleCommand? ToCreateCommand(this MotorcycleCreateDto? dto)
        {
            if (dto is null) return null;

            return new CreateMotorcycleCommand
            {
                Identifier = dto.Identifier,
                Year = dto.Year,
                Model = dto.Model,
                Plate = dto.Plate
            };
        }
        public static UpdatePlateCommand ToUpdatePlateCommand(this UpdatePlateDto dto, string identifier)
        {
            return new UpdatePlateCommand
            {
                Identifier = identifier,
                NewPlate = dto.Plate
            };
        }

        public static DeleteMotorcycleCommand ToDeleteCommand(this string identifier)
            => new DeleteMotorcycleCommand { Identifier = identifier };
    }
}
