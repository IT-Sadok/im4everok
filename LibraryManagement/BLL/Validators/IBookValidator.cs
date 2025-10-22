using DAL.DTO;

namespace BLL.Validators
{
    public interface IBookValidator
    {
        bool ValidateAddedBook(AddBookRequest book);
    }
}
