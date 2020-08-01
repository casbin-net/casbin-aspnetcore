using System.IO;

namespace Casbin.AspNetCore.Tests.Utilities
{
    public static class TestUtility
    {
        public static string GetExampleFile(string fileName)
        {
            return Path.Combine("Examples", fileName);
        }
    }
}
