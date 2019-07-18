using ExpressionEvaluator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ExpressionEvaluatorTests
{
    public static class ExpressionExtensions
    {
        public static string ToJSON(this Expression expression, string rawExpression = "")
        {
            string json = JsonConvert.SerializeObject(expression, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            JObject obj = JObject.Parse(json);

            RemoveProperty(obj, "ParentExpression");
            RemoveProperty(obj, "TopParentExpression");
            RemoveProperty(obj, "NonUnaryParent");
            RemoveProperty(obj, "IsUnary");

            //Make these properties not to hold objects, but their values
            FlattenProperty(obj, "Operator");
            FlattenProperty(obj, "LeftOperand");
            FlattenProperty(obj, "RightOperand");
            
            if (!string.IsNullOrWhiteSpace(rawExpression))
                obj["Raw"] = rawExpression;

            return obj.ToString();
        }

        static void FlattenProperty(JObject obj, string propertyName)
        {
            foreach (var token in obj.SelectTokens($"$..{propertyName}"))
                ((JObject)token.Parent.Parent)[$"{propertyName}"] = token["Value"];
        }

        static void RemoveProperty(JObject obj, string propertyName)
        {
            //Work with an enumerated list, otherwise removing a property causes problems.
            foreach (var token in obj.SelectTokens($"$..{propertyName}").ToList())
                ((JObject)token.Parent.Parent).Remove($"{propertyName}");
        }
    }
}
