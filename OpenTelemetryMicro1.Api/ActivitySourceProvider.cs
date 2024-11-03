using System.Diagnostics;

namespace OpenTelemetryMicro1.Api
{
    public class ActivitySourceProvider
    {
        public static ActivitySource Instance=new ActivitySource("OpenTelemetryMicro1.Api.Source","v1");
    }
}
