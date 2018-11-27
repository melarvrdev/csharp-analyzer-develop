using System.IO;

namespace Exercism.Analyzers.CSharp.Tests.Analysis.Solutions
{
    internal static class DirectoryInfoExtensions
    {
        public static void Recreate(this DirectoryInfo directoryInfo)
        {
            if (directoryInfo.Exists)
                directoryInfo.Delete(true);

            directoryInfo.Create();
        }
    }
}