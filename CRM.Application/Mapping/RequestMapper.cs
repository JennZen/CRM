using CRM.Application.DTOs.Request;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using Riok.Mapperly.Abstractions;

namespace CRM.Application.Mapping
{
    [Mapper]
    public partial class RequestMapper
    {
        // Domain -> DTO

        public partial RequestDetailsDto ToDetailsDto(Request request);

        public partial RequestListDto ToListDto(Request request);

        public partial List<RequestListDto> ToListDtos(List<Request> requests);

        public partial RequestMiniDto ToMiniDto(Request request);

        public partial List<RequestMiniDto> ToMiniDtos(List<Request> requests);


        // DTO -> Domain

        [MapperIgnoreSource(nameof(RequestCreateDto.CustomerId))]
        [MapperIgnoreSource(nameof(RequestCreateDto.ManagerId))]
        public partial Request ToDomain(RequestCreateDto dto);

        [MapperIgnoreSource(nameof(RequestUpdateDto.CustomerId))]
        [MapperIgnoreSource(nameof(RequestUpdateDto.ManagerId))]
        public partial Request ToDomain(RequestUpdateDto dto);


        // Enum -> string

        private static string MapStatus(Status status)
            => status.ToString();

        private static string MapPriority(Priority priority)
            => priority.ToString();


        // Related entities -> string

        private static string MapCustomer(Customer? customer)
            => customer?.Name ?? string.Empty;

        private static string MapManager(User? manager)
            => manager == null
                ? string.Empty
                : $"{manager.FirstName} {manager.LastName}";


        // string -> enum

        private static Priority MapPriority(string priority)
        {
            if (Enum.TryParse<Priority>(priority, true, out var result))
                return result;

            throw new ArgumentException(
                $"Invalid priority: {priority}");
        }
    }
}