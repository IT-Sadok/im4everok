namespace Application.Interfaces.External
{
    public interface IFileHashCalculationService
    {
        string CalculateHash(Stream file);
    }
}
