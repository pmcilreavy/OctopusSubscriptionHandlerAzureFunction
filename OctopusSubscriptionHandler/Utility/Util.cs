using System;

namespace OctopusSubscriptionHandler.Utility
{
    public class Util
    {
        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
