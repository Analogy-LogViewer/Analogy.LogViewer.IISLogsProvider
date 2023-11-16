using System.IO;
using System.Runtime.CompilerServices;

namespace Analogy.LogViewer.IISLogsProvider
{
    public static class Utils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetFileNameAsDataSource(string fileName)
        {
            string file = Path.GetFileName(fileName);
            return fileName.Equals(file) ? fileName : $"{file} ({fileName})";
        }
    }
}