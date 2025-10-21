using System.Text.Json;

using DAL.Models;

namespace DAL.Database
{
    public class JsonDatabase(string fileName) : IDatabase<Book>
    {
        private readonly string _fullPath = Path.Combine(AppContext.BaseDirectory, fileName);

        private async Task CreateJsonDbIfNotExists()
        {
            if (!File.Exists(_fullPath))
            {
                await File.WriteAllTextAsync(_fullPath, "[]");
            }
        }

        public async Task<List<Book>> GetAll()
        {
            await CreateJsonDbIfNotExists();

            using var stream = File.OpenRead(_fullPath);
            try
            {

                List<Book>? books = await JsonSerializer.DeserializeAsync<List<Book>>(stream);
                return books ?? [];
            }
            catch (JsonException)
            {
                // If the JSON is invalid, return an empty list
                return [];
            }
        }

        public async Task<bool> SaveData(List<Book> data)
        {
            await CreateJsonDbIfNotExists();

            try
            {

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
        }
    }
}
