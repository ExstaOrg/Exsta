using Microsoft.Extensions.Hosting;

namespace Exsta_Shared.Extensions;

public static class HostingEnvironmentExtensions {
    public static bool IsLocalDevelopment(this IHostEnvironment env) {
        return env.EnvironmentName.Equals("LocalDevelopment", StringComparison.OrdinalIgnoreCase);
    }
}
