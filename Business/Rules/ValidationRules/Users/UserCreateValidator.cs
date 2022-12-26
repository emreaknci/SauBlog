using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs.Users;
using FluentValidation;

namespace Business.Rules.ValidationRules.Users
{
    public class UpdateUserValidator:AbstractValidator<UserForUpdateDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty().WithMessage("Bu alan boş bırakılamaz")
                .NotNull().WithMessage("Bu alan boş bırakılamaz")
                .Length(2,50).WithMessage("Kullanıcı adı 2-50 karakter arasında olmalı");
            RuleFor(u => u.LastName).NotEmpty().WithMessage("Bu alan boş bırakılamaz")
                .NotNull().WithMessage("Bu alan boş bırakılamaz")
                .Length(2, 50).WithMessage("Kullanıcı soyadı 2-50 karakter arasında olmalı");
        }
    }
}
