using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNGenerator
{
    internal class HangulDisassembler
    {
        static readonly string[] CHOSUNG = new string[]
        {
        "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ",
        "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ",
        "ㅌ", "ㅍ", "ㅎ"
        };

        static readonly string[] JUNGSUNG = new string[]
        {
        "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ",
        "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ",
        "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ"
        };

        static readonly string[] JONGSUNG = new string[]
        {
        "", "ㄱ", "ㄲ", "ㄳ", "ㄴ", "ㄵ", "ㄶ", "ㄷ",
        "ㄹ", "ㄺ", "ㄻ", "ㄼ", "ㄽ", "ㄾ", "ㄿ", "ㅀ",
        "ㅁ", "ㅂ", "ㅄ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅊ",
        "ㅋ", "ㅌ", "ㅍ", "ㅎ"
        };

        public static char[] DisassembleHangul(string input)
        {
            StringBuilder sbOutput = new StringBuilder();

            foreach (char c in input)
            {
                if (c >= 0xAC00 && c <= 0xD7A3) // 한글 음절 범위
                {
                    int unicode = c - 0xAC00;
                    int cho = unicode / (21 * 28);
                    int jung = (unicode % (21 * 28)) / 28;
                    int jong = unicode % 28;

                    sbOutput.Append($"{CHOSUNG[cho]} {JUNGSUNG[jung]} {(JONGSUNG[jong] != "" ? JONGSUNG[jong] : "")}");
                    sbOutput.Append($"-"); // 글자간 공백추가
                }
                else
                {
                    sbOutput.Append($"{c}");
                }
            }

            return sbOutput.ToString().ToCharArray();
        }

        //static void Main()
        //{
        //    Console.OutputEncoding = System.Text.Encoding.UTF8;
        //    Console.Write("입력 문자열: ");
        //    string input = Console.ReadLine();
        //    DisassembleHangul(input);
        //}
    }


}
