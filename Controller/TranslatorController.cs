using Microsoft.AspNetCore.Mvc;

namespace MorseCodeSimulator.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class TranslatorController : ControllerBase
    {
        static readonly string LatinAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        static readonly Dictionary<string, string> MorseCodes = new Dictionary<string, string>()
        {
            {"A", ".-"},
            {"B", "-..."},
            {"C", "-.-."},
            {"D", "-.."},
            {"E", "."},
            {"F", "..-."},
            {"G", "--."},
            {"H", "...."},
            {"I", ".."},
            {"J", ".---"},
            {"K", "-.-"},
            {"L", ".-.."},
            {"M", "--"},
            {"N", "-."},
            {"O", "---"},
            {"P", ".--."},
            {"Q", "--.-"},
            {"R", ".-."},
            {"S", "..."},
            {"T", "-"},
            {"U", "..-"},
            {"V", "...-"},
            {"W", ".--"},
            {"X", "-..-"},
            {"Y", "-.--"},
            {"Z", "--.."},
            {"1", ".----"},
            {"2", "..---"},
            {"3", "...--"},
            {"4", "....-"},
            {"5", "....."},
            {"6", "-...."},
            {"7", "--..."},
            {"8", "---.."},
            {"9", "----."},
            {"0", "-----"}
        };

        static void Encode(ref string code)
        {
            string tempCode = code;
            code = "";

            foreach (char c in tempCode)
            {
                if (!String.IsNullOrWhiteSpace(c.ToString()))
                {
                    foreach (KeyValuePair<string, string> item in MorseCodes)
                    {
                        if (String.Equals(c.ToString(), item.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            code += item.Value + "|";
                        }
                    }
                }
                else
                {
                    code += " ";
                }
            }
            code = code.Substring(0, code.Length - 1); // Remove last char which is "|"
        }

        static void Decode(ref string code)
        {
            string tempCode = code;
            code = "";
            string tempString = "";

            List<string> codeList = new List<string>();
            
            foreach (char c in tempCode)
            {
                if (String.Equals(c.ToString(), "|"))
                {
                    codeList.Add(tempString);
                    tempString = "";
                }
                else if (String.Equals(c.ToString(), " "))
                {
                    codeList.Add(" ");
                }
                else
                {
                    tempString += c;
                }
            }

            if (tempString != "") codeList.Add(tempString); // It adds the last morse code to codeList

            foreach (string c in codeList)
            {
                if (String.IsNullOrWhiteSpace(c))
                {
                    code += " ";
                    continue;
                }

                foreach (KeyValuePair<string, string> item in MorseCodes)
                {
                    if (String.Equals(c.ToString(), item.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        code += item.Key;
                    }
                }
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public JsonResult Get([FromQuery(Name = "code")] string code)
        {
            int encode = 0;
            int decode = 0;

            foreach (char c in code)
            {
                if ((c.ToString() is ".") || (c.ToString() is "-") || (c.ToString() is "|"))
                {
                    decode++;
                }
                else if (LatinAlphabet.Contains(c, StringComparison.OrdinalIgnoreCase))
                {
                    encode++;
                }
            }

            if (encode == 0 || decode == 0)
            {
                if (encode > 0) Encode(ref code);
                else Decode(ref code);

                return new JsonResult(new { Resp = code });
            }
            else
            {
                return new JsonResult(new { Resp = "Invalid input." });
            }
        }
    }
}
