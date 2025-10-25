using DAL.DTO;

using FluentValidation;

namespace BLL.Validators
{
    public class AddedBookValidator : AbstractValidator<AddBookRequest>, IBookValidator
    {
        public AddedBookValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Book name must be between 1 and 200 characters.");

            RuleFor(b => b.Author)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Author name must be between 1 and 100 characters.");

            RuleFor(b => b.YearOfPublish)
                .GreaterThan(0)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Year of publish must be between 1 and the current year.");
        }

        public bool ValidateAddedBook(AddBookRequest book)
        {
            var result = Validate(book);
            if (!result.IsValid)
            {
                throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));
            }
            return true;
        }
    }
}
