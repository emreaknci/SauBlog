using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs.Blog;
using FluentValidation;

namespace Business.Rules.ValidationRules.Blogs
{
    public class CreateBlogValidator : AbstractValidator<BlogForCreateDto>
    {
        public CreateBlogValidator()
        {
            RuleFor(b => b.Title).NotEmpty().WithMessage("Blog başlığı boş bırakılamaz")
                .Length(2, 75).WithMessage("Blog başlığı 2-75 karakter arasında olmalı");

            RuleFor(b => b.Content).NotEmpty().WithMessage("Blog içeriği boş bırakılamaz")
                .MinimumLength(2).WithMessage("Blog içeriği minimum 2 karakter olmalı");
            RuleFor(b => b.WriterId).NotNull();
            RuleFor(b => b.CategoryIds).NotEmpty().WithMessage("En az bir kategori seçmelisiniz");
            //RuleFor(b => b.Image).NotEmpty().WithMessage("Kapak resmi seçmek zorundasınız");
        }
    }
}
