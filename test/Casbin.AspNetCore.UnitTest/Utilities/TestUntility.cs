using System.IO;

namespace Casbin.AspNetCore.UnitTest.Utilities
{
    public static class TestUtility
    {
        public static string GetExampleFile(string fileName)
        {
            return Path.Combine("Examples", fileName);
        }
    }
}
