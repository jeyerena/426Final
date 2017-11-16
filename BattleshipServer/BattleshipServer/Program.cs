using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BattleshipServer
{
	class Program
	{
		static void Main(string[] args)
		{
			var web = new HttpListener();
			web.Prefixes.Add("http://localhost:80/");
			Console.WriteLine("Listening..");
			web.Start();

			HttpListenerContext context = web.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;
			String dataType = request.ContentType;

			StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8);
			Console.WriteLine($"HTTP Method: {request.HttpMethod}");
			Console.WriteLine($"Header: {request.Headers}");
			Console.WriteLine($"Content: {reader.ReadToEnd()}");

			const string responseString = "<html><body>Hello world</body></html>";
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
			response.ContentLength64 = buffer.Length;

			System.IO.Stream output = response.OutputStream;
			output.Write(buffer, 0, buffer.Length);

			output.Close();
			web.Stop();
			Console.ReadKey();
		}
	}
}
