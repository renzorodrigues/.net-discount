using DiscountGRPC;
using Grpc.Net.Client;

class Program
{
    private static DiscountService.DiscountServiceClient _client;

    static async Task Main(string[] args)
    {
        Console.WriteLine("Initializing gRPC client...");

        using var channel = GrpcChannel.ForAddress("http://localhost:5116");
        _client = new DiscountService.DiscountServiceClient(channel);

        Console.WriteLine("Connected to gRPC server.\n");

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Discount Service Client ===");
            Console.WriteLine("1. Generate Codes");
            Console.WriteLine("2. Use Code");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await GenerateCodes();
                    break;
                case "2":
                    await UseCode();
                    break;
                case "3":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Press Enter to try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static async Task GenerateCodes()
    {
        Console.Clear();
        Console.WriteLine("=== Generate Codes ===");

        Console.Write("Enter the number of codes to generate: ");
        if (!int.TryParse(Console.ReadLine(), out int count))
        {
            Console.WriteLine("Invalid number. Press Enter to return to the menu.");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter the length of each code: ");
        if (!int.TryParse(Console.ReadLine(), out int length))
        {
            Console.WriteLine("Invalid length. Press Enter to return to the menu.");
            Console.ReadLine();
            return;
        }

        try
        {
            var response = await _client.GenerateCodesAsync(new GenerateCodesRequest
            {
                Count = (uint)count,
                Length = (uint)length
            });

            if (!response.Result)
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
            }
            else
            {
                Console.WriteLine("Generated codes:");
                foreach (var code in response.Codes)
                {
                    Console.WriteLine(code);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating codes: {ex.Message}");
        }

        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();
    }

    private static async Task UseCode()
    {
        Console.Clear();
        Console.WriteLine("=== Use Code ===");

        Console.Write("Enter the code to use: ");
        var code = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(code))
        {
            Console.WriteLine("Invalid code. Press Enter to return to the menu.");
            Console.ReadLine();
            return;
        }

        try
        {
            var response = await _client.UseCodeAsync(new UseCodeRequest { Code = code });

            Console.WriteLine($"Result: {response.Result}");
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
            }
            else
            {
                Console.WriteLine("Code used successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error using code: {ex.Message}");
        }

        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();
    }
}
