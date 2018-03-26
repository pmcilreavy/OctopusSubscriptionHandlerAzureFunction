using System;

namespace OctopusSubscriptionHandler.Core.Utility
{
    public class Util
    {
        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
