using System.IO;

namespace Casbin.AspNetCore.Performance.Utilities
{
    public static class ConfigurationUtility
    {
        public static string GetExampleFile(string fileName)
        {
            return Path.Combine("Examples", fileName);
        }
    }
}
