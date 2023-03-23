using FluentValidation;
using FluentValidation.Validators;
using HomeApi.Contracts.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Contracts.Validation
{
    public class UpdateRoomRequestValidation : AbstractValidator<UpdateRoomRequest>
    {
        public UpdateRoomRequestValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x=>x.Area).NotEmpty();
            RuleFor(x=>x.Voltage).NotEmpty();
            RuleFor(x => x.GasConnected).NotEmpty();
        }
    }
}
