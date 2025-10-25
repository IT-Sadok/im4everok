namespace LibraryManagement.Strategies
{
    internal record AppStrategy(int Order, string Label, Func<Task> Action);

    internal class Application(ICollection<AppStrategy> strategies)
    {
        private readonly List<AppStrategy> _strategies = strategies.OrderBy(s => s.Order).ToList();

        internal async Task Run()
        {
            while (true)
            {
                Console.WriteLine("\n=== Library Management ===");
                foreach (var strategy in _strategies)
                {
                    Console.WriteLine($"{strategy.Order}. {strategy.Label}");
                }
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int choice) || (choice < 0 || choice > _strategies.Count))
                {
                    Console.WriteLine("\n❌ Invalid choice. Please try again.");
                    continue;
                }
                if (choice == 0)
                {
                    Console.WriteLine("Exiting the application. Goodbye!");
                    break;
                }
                var selectedStrategy = _strategies.First(s => s.Order == choice);
                await selectedStrategy.Action();
            }
        }
    }
}
