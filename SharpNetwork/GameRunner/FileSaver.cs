
using System.IO;

namespace GameRunner
{
    public class FileSaver
    {
        public static void AddData(string data)
        {
            File.AppendAllText("trainingData.txt", data);
        }
    }
}