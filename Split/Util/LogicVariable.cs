using System;
using System.Text.RegularExpressions;
using FluentDateTime;

namespace Split.Util
{
    public static class LogicVariable
    {
        public static int count_pages = 0, count_even = 0, count_odd = 0, count_field = 0, count_split = 0;
        public static string file = string.Empty;
        public static string time;


        public static void ResetVariable()
        {
            count_pages = count_even = count_odd = count_field = count_split = 0;
            string file = string.Empty;
        }

        public static string Get(Match logicVariable, int count = 1)
        {
            if ($"{logicVariable.Groups[1]}".StartsWith("s_Date"))
            {
               return GetDate(logicVariable);
            }
            else
            if ($"{logicVariable.Groups[1]}" == "s_fileName")
            {
                return getFileName(mask: $"{logicVariable.Groups["MaskFormat"]}");
            }
            else
            if ($"{logicVariable.Groups[1]}"[0] == 'c')
            {
                return getCounters(mask: $"{logicVariable.Groups["MaskFormat"]}", value: count);
            }else
            if($"{logicVariable.Groups[1]}" == "s_Time")
            {
                return GetTime(mask: $"{logicVariable.Groups["MaskFormat"]}");
            }

            return "";
        }


        public static string GetProcess(Match logicVariable)
        {
            var aux_mask = $"{logicVariable.Groups["MaskFormat"]}";

            switch ($"{logicVariable.Groups[1]}")
            {
                case "s_Date":
                    return GetDate(logicVariable);
                case "s_Time":
                    return GetTime(mask: aux_mask, time: time);
                case "s_fileName":
                    return getFileName(fileName:file , mask: aux_mask);
                case "c_Field":
                    return getCounters(mask: aux_mask, value: count_field);
                case "c_Page":
                    return getCounters(mask: aux_mask, value: count_pages);
                case "c_Split":
                    return getCounters(mask: aux_mask, value: count_split);
                case "code_c_Field":
                    return getCounters(mask: aux_mask, value: count_field);
                case "code_c_Page":
                    return getCounters(mask: aux_mask, value: count_pages);
                case "code_c_Split":
                    return getCounters(mask: aux_mask, value: count_split);

                default:

                    if ($"{logicVariable.Groups[1]}".StartsWith("s_Date_"))
                    {
                        return GetDate(logicVariable);
                    }

                    throw new Exception("Campo não localizado!");
            }
        }

  

        private static string GetDate(Match logicVariable)
        {
            try
            {
                bool success_Inc = int.TryParse($"{logicVariable.Groups["IncDays"]}", out int inc);
                bool workDay = $"{logicVariable.Groups["WorkDay"]}" == "U" ? true : false;

                if (success_Inc)
                {
                    return GetDate(mask: $"{logicVariable.Groups["MaskFormat"]}", incDays: inc, workDay: workDay);
                }

                throw new Exception("Expressão de Data Inválida!");

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static string Get(string logicVariable, string maskformat = "", int count = 1)
        {
            if (logicVariable.Contains("Date"))
            {
                return GetDate(maskformat);
            }
            else
            if (logicVariable.Contains("fileName"))
            {
                return getFileName(mask: maskformat);
            }
            else
            if (logicVariable[0] == 'c')
            {
                return getCounters(mask: maskformat, value: count);
            }

            return "";
        }

        private static string getCounters(string mask, int value)
        {
            return string.Format(mask, value);
        }

        public static string getFileName(string fileName = "Nome_Arquivo.PDF", string mask = "SN")
        {
            if (mask[0] == 'S')
                fileName = Regex.Replace(fileName, @"\.\w{3}$", "");

            if (mask[1] == 'L')
                fileName = fileName.ToLower();
            else
                if (mask[1] == 'U')
                fileName = fileName.ToUpper();

            return fileName;
        }

        public static string GetDate(string mask = "dd/MM/yyyy", int incDays = 0, bool workDay = true)
        {
            //Dias úteis
            if (workDay)
            {
                return DateTime.Today.AddBusinessDays(incDays).ToString(mask);
            }
            else
            {
                return DateTime.Today.AddDays(incDays).ToString(mask);
            }
            
        }

        public static string GetTime(string mask = "HH:mm:ss")
        {
                return DateTime.Now.ToString(mask);
        }


        private static string GetTime(string mask, string time)
        {
            return DateTime.Parse(time).ToString(mask);
        }
    }
}
