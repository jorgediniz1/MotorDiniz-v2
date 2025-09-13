using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MotorDiniz.Application.CQRS.Rentals.Commands
{
    public sealed record InformReturnDateCommand : IRequest<Unit>
    {
        public string Identifier { get; init; } = default!;
        public DateTime ReturnDate { get; set; }
    }
}
