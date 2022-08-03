using System.Text;
using System.Text.RegularExpressions;

namespace Split.Util
{
    public static class Regexp
    {
        public static string Field = @"<(?<FieldName>(?:s_(?:Date_(?:(?<IncDays>\d+)(?<WorkDay>[NU]))|fileName|Time))|(?:(?:code_)?c_(?:Page|Field|fileSplit)))<(?<MaskFormat>[^>]+)>>";
        public static string FieldCode = "<(?<FieldName>(?:code_c_(?:Page|Field|fileSplit)))<(?<MaskFormat>[^>]+)>>";
        public static string Variable_Counters = "<(?<FieldName>(?:(?:code_)?c_(?:Page|Field|fileSplit)))<(?<MaskFormat>[^>]+)>>";
        public static string Variable_FileSplit_Counter = "<(?<FieldName>(?:(?:code_)?c_(?:fileSplit)))<(?<MaskFormat>[^>]+)>>";
        public static string Variable_Field_Counter = "<(?<FieldName>(?:(?:code_)?c_(?:Field)))<(?<MaskFormat>[^>]+)>>";
        //public static string VariableCounters = "<(?<FieldName>(?:c_(?:Page|Field|fileSplit)))<(?<MaskFormat>[^>]+)>>";
        public static string NotVariableCounters = @"<(?<FieldName>(?:s_(?:Date_(?:(?<IncDays>\d+)(?<WorkDay>[NU]))|fileName|Time)))<(?<MaskFormat>[^>]+)>>";
        public static string isPdf = @"(?i:\.pdf$)";


        public static bool ExpressionExists(string subject, string pattern)
        {
            if (string.IsNullOrWhiteSpace(subject))
                return false;

            return Regex.IsMatch(subject, pattern);
        }

        public static string CreateLogicVariable(string expression)
        {
            StringBuilder expression_Tmp = new StringBuilder(expression);
            StringBuilder expression_aux = new StringBuilder();

             //Existe algum campo Lógico
            if (ExpressionExists(expression, Field))
            {
                while (true)
                {
                    var match = Regex.Match($"{expression_Tmp}", Field);

                    if (match.Success == false)
                        break;

                    expression_aux.Clear();

                    expression_aux.Append(Regex.Replace($"{expression_Tmp}", $"{match.Groups[0]}".Replace(".", "\\."), Util.LogicVariable.Get(match)));

                    expression_Tmp.Clear();
                    expression_Tmp.Append($"{expression_aux}");
                }
            }
 
            return $"{expression_Tmp}";
        }

        public static string LogicVariable(string expression, string find)
        {
            StringBuilder expression_Tmp = new StringBuilder(expression);
            StringBuilder expression_aux = new StringBuilder();

            //Existe algum campo Lógico
            if (ExpressionExists($"{expression}", find))
            {
                while (true)
                {
                    var match = Regex.Match($"{expression_Tmp}", find);

                    if (match.Success == false)
                        break;

                    expression_aux.Clear();

                    expression_aux.Append(Regex.Replace($"{expression_Tmp}", $"{match.Groups[0]}".Replace(".", "\\."), Util.LogicVariable.GetProcess(match)));

                    expression_Tmp.Clear();
                    expression_Tmp.Append($"{expression_aux}");
                }
            }

            return $"{expression_Tmp}";
        }


        public static void CalculateLogics(ref StringBuilder expression)
        {
            var V_Not_Counters = Regex.Matches($"{expression}", NotVariableCounters);

            for (int i = V_Not_Counters.Count - 1; i >= 0; i--)
            {
                expression.Remove(V_Not_Counters[i].Index, V_Not_Counters[i].Length);
                expression.Insert(V_Not_Counters[i].Index, Util.LogicVariable.GetProcess(V_Not_Counters[i]));
            }

            var V_Counters = Regex.Matches($"{expression}", Variable_Counters);

            for (int i = V_Counters.Count - 1; i >= 0; i--)
            {
                expression.Remove(V_Counters[i].Index, V_Counters[i].Length);

                if (Regex.IsMatch($"{V_Counters[i].Groups[0]}", Variable_FileSplit_Counter))
                    expression.Insert(V_Counters[i].Index, "{2:");
                else
                    if (Regex.IsMatch($"{V_Counters[i].Groups[0]}", Variable_Field_Counter))
                    expression.Insert(V_Counters[i].Index, "{1:");
                else
                    expression.Insert(V_Counters[i].Index, "{0:");

                expression.Insert(V_Counters[i].Index + 3, $"{V_Counters[i].Groups["MaskFormat"]}".Substring(3));
            }

        }

        public static int GetIdLayout(string id)
        {
            var split = Regex.Split(id, "_");

            return int.Parse(split[0]);
        }

        public static int GetIdField(string id)
        {
            var split = Regex.Split(id, "_");

            return int.Parse(split[1])+1;
        }

    }
}
