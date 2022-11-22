using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Asynchronous
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

			ActivityContext context;
			using (var activity = source.StartActivity("root", ActivityKind.Internal, context))
			{
				context = activity.Context;
				Console.WriteLine("root");
				Thread.Sleep(TimeSpan.FromSeconds(30));
			}

			// When sleep >= 10 min the spans are not linked in the tracing ui
			Thread.Sleep(TimeSpan.FromMinutes(10));

			using (_ = source.StartActivity("child 01", ActivityKind.Internal, context))
			{
				Console.WriteLine("child 01");
				Thread.Sleep(TimeSpan.FromSeconds(30));
			}
		}
	}
}
