using MotorDiniz.Application.CQRS.DeliveryRiders.Commands;
using MotorDiniz.Application.DTOs.DeliveryRider;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Enums;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Application.Extensions
{
    public static class DeliveryRiderExtensions
    {
        public static DeliveryRider? ToEntity(this DeliveryRiderCreateDto? dto)
        {
            if (dto is null) return null;

            return new DeliveryRider(
                dto.Identifier,
                dto.Name,
                dto.Cnpj,
                dto.BirthDate,
                dto.CnhNumber,
                ParseCnh(dto.CnhType)
            );
        }

        public static CreateDeliveryRiderCommand? ToCreateCommand(this DeliveryRiderCreateDto? dto)
        {
            if (dto is null) return null;
            return new CreateDeliveryRiderCommand
            {
                Identifier = dto.Identifier,
                Name = dto.Name,
                Cnpj = dto.Cnpj,
                BirthDate = dto.BirthDate,
                CnhNumber = dto.CnhNumber,
                CnhType = ParseCnh(dto.CnhType),
                CnhImageBase64 = dto.CnhImageBase64
            };
        }

        public static UploadDeliveryRiderCnhCommand? ToUploadCnhCommand(this DeliveryRiderCnhUploadDto dto, string identifier)
        {
            if (dto is null) return null;

            return new UploadDeliveryRiderCnhCommand
            {
                Identifier = identifier,
                CnhImageBase64 = dto.CnhImageBase64 
            };
        }

        private static CnhType ParseCnh(string? value) => value switch
        {
            "A" => CnhType.A,
            "B" => CnhType.B,
            "A+B" => CnhType.AB,
            _ => throw new DomainExceptionValidation("CNH type is invalid.")
        };
    }
}
