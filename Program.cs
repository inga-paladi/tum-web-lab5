using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return;
        }

        string option = args[0];
        if (option.Equals("-h"))
        {
            ShowHelp();
            return;
        }

        if (option.Equals("-u"))
        {
            if (args.Length < 2)
            {
                Console.WriteLine("URL is missing.");
                return;
            }
            string url = args[1];
            await MakeWebSocketRequest(url);
        }
        else if (option.Equals("-s"))
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Search term is missing.");
                return;
            }
            string searchTerm = string.Join(" ", args, 1, args.Length - 1);
            // Implement your search functionality here
        }
        else
        {
            Console.WriteLine("Invalid option.");
            ShowHelp();
        }
    }

    static async Task MakeWebSocketRequest(string url)
    {
        try
        {
            using (var webSocket = new ClientWebSocket())
            {
                await webSocket.ConnectAsync(new Uri(url), CancellationToken.None);

                var receiveBuffer = new byte[1024];
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    string responseText = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                    Console.WriteLine(responseText);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ShowHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("go2web -u <URL>");
        Console.WriteLine("go2web -s <search-term>");
        Console.WriteLine("go2web -h");
    }
}
