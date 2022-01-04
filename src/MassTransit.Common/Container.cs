using System;

namespace MassTransit.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class Container
    {
        private static bool? _isRunningInContainer;

        public static bool IsRunningInContainer =>
            _isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;
    }
}
