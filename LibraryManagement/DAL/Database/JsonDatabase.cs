using System.Text.Json;

using DAL.Models;

namespace DAL.Database
{
    public class JsonDatabase(string fileName) : IDatabase<Book>
    {
        private readonly string _fullPath = Path.Combine(AppContext.BaseDirectory, fileName);
        private readonly SemaphoreSlim _semaphoreSlim = new(1);
        private bool _isInitialized = false;
        private async Task CreateJsonDbIfNotExists()
        {
            if (_isInitialized) return;

            if (!File.Exists(_fullPath))
            {
                await File.WriteAllTextAsync(_fullPath, "[]");
            }

            _isInitialized = true;
        }

        public async Task<List<Book>> GetAll()
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                await CreateJsonDbIfNotExists();

                using var stream = File.OpenRead(_fullPath);

                List<Book>? books = await JsonSerializer.DeserializeAsync<List<Book>>(stream);
                return books ?? [];
            }
            catch (JsonException)
            {
                // If the JSON is invalid, return an empty list
                return [];
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<bool> SaveData(List<Book> data)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                await CreateJsonDbIfNotExists();

                string serializedData = JsonSerializer.Serialize(data, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(_fullPath, serializedData);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
