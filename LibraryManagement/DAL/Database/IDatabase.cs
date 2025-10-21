namespace DAL.Database
{
    public interface IDatabase<T>
    {
        public Task<List<T>> GetAll();
        public Task<bool> SaveData(List<T> data);
    }
}
