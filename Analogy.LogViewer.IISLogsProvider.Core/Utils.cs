using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
