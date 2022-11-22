using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Synchronous
{
	public class Program
	{
		public static void Main(string[] args)
		{
			ActivitySource source = new ActivitySource("test");
			using var tracerProvider = Sdk.CreateTracerProviderBuilder()
				.AddSource(source.Name)
				.AddOtlpExporter(config =>
				{
					config.Endpoint = new Uri("https://otlp.nr-data.net:4317");
					config.Headers = $"api-key=TODO"; // replace with license key
				})
				.Build();


			using (source.StartActivity("root"))
			{
				// When i > 60 the root span does not appear in New Relic
				for (int i = 0; i < 65; i++)
				{
					using (source.StartActivity($"child {i:00}"))
					{
						Console.WriteLine(i);
						Thread.Sleep(TimeSpan.FromMinutes(1));
					}
				}
			}
		}
	}
}
