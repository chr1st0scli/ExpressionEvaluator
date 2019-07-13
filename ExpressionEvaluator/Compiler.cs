using ExpressionEvaluator.Parsing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExpressionEvaluator
{
    public static class Compiler
    {
        public static void Compile(string expression, string targetFile)
        {
            var expr = new Parser(expression).Parse();

            using (var stream = File.OpenWrite(targetFile))
                new BinaryFormatter().Serialize(stream, expr);
        }

        public static Expression Load(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
                return (Expression)new BinaryFormatter().Deserialize(stream);
        }
    }
}
