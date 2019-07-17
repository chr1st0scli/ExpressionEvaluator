using ExpressionEvaluator.Parsing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExpressionEvaluator
{
    public static class Compiler
    {
        /// <summary>
        /// Builds a parse tree and saves it on disk.
        /// </summary>
        /// <param name="expression">The expression to compile.</param>
        /// <param name="targetFile">The output of the compilation.</param>
        public static void Compile(string expression, string targetFile)
        {
            var expr = new Parser(expression).Parse();

            using (var stream = File.OpenWrite(targetFile))
                new BinaryFormatter().Serialize(stream, expr);
        }

        /// <summary>
        /// Loads an expression that is ready to be evaluated from disk.
        /// </summary>
        /// <param name="fileName">The compiled file to load from.</param>
        /// <returns>The expression that can be fed to an evaluator.</returns>
        public static Expression Load(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
                return (Expression)new BinaryFormatter().Deserialize(stream);
        }
    }
}
