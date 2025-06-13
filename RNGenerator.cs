using System;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RNGenerator
{
    public partial class RNGenerator : Form
    {
        string[] strArr_TempContent = new string[100];
        string[] strArr_XKey = new string[10];
        string[] strArr_YKey = new string[10];

        public RNGenerator()
        {
            InitializeComponent();

            Input_A.TextChanged += (s, e) => DecipherAMetrix("A");
            Input_B.TextChanged += (s, e) => DecipherAMetrix("B");
            Input_C.TextChanged += (s, e) => DecipherAMetrix("C");
        }



        //------------------------------- 컨트롤 탐색 -------------------------------
        private Control FindControl(Control _parent, string _name)     // 재귀 탐색 >> 선택한 항목의 자식들을 탐색하여 내가 원하는 요소를 매칭
        {
            foreach (Control control in _parent.Controls)
            {
                if (control.Name == _name)
                    return control;

                Control found = FindControl(control, _name);
                if (found != null)
                    return found;
            }
            return null;
        }
        //---------------------------------------------------------------------------


        //------------------------------- 자모음 분리 -------------------------------
        public static string SplitKorean(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length != 1)
                return "";

            char c = input[0];

            string[] ChoSung = new string[]
            {
                "ㄱ","ㄲ","ㄴ","ㄷ","ㄸ","ㄹ","ㅁ","ㅂ","ㅃ","ㅅ",
                "ㅆ","ㅇ","ㅈ","ㅉ","ㅊ","ㅋ","ㅌ","ㅍ","ㅎ"
            };

            string[] JungSung = new string[]
            {
                "ㅏ","ㅐ","ㅑ","ㅒ","ㅓ","ㅔ","ㅕ","ㅖ","ㅗ","ㅘ",
                "ㅙ","ㅚ","ㅛ","ㅜ","ㅝ","ㅞ","ㅟ","ㅠ","ㅡ","ㅢ","ㅣ"
            };

            string[] JongSung = new string[]
            {
                "", "ㄱ","ㄲ","ㄳ","ㄴ","ㄵ","ㄶ","ㄷ","ㄹ","ㄺ",
                "ㄻ","ㄼ","ㄽ","ㄾ","ㄿ","ㅀ","ㅁ","ㅂ","ㅄ","ㅅ",
                "ㅆ","ㅇ","ㅈ","ㅊ","ㅋ","ㅌ","ㅍ","ㅎ"
            };

            if (c < 0xAC00 || c > 0xD7A3)
                return "";

            int unicode = c - 0xAC00;
            int choIndex = unicode / (21 * 28);
            int jungIndex = (unicode % (21 * 28)) / 28;
            int jongIndex = unicode % 28;

            // 초성 + 중성 + 종성을 문자열로 합쳐 반환
            return ChoSung[choIndex] + JungSung[jungIndex] + JongSung[jongIndex];
        }
        //---------------------------------------------------------------------------



        //------------------------------- 모음, 쌍자음, 겹자음 분리 -------------------------------
        private string DetachedElement(string _inputValue)
        {
            return _inputValue switch
            {
                "ㅘ" => "ㅗㅏ",     // 모음
                "ㅙ" => "ㅗㅐ",
                "ㅚ" => "ㅗㅣ",
                "ㅝ" => "ㅜㅓ",
                "ㅞ" => "ㅜㅔ",    //ㅜㅓㅣ
                "ㅟ" => "ㅜㅣ",
                "ㅢ" => "ㅡㅣ",
                "ㅐ" => "ㅏㅣ",
                "ㅔ" => "ㅓㅣ",
                "ㅒ" => "ㅑㅣ",
                "ㅖ" => "ㅕㅣ",
                "ㄲ" => "ㄱㄱ",    // 쌍자음
                "ㄸ" => "ㄷㄷ",
                "ㅃ" => "ㅂㅂ",
                "ㅆ" => "ㅅㅅ",
                "ㅉ" => "ㅈㅈ",
                "ㄳ" => "ㄱㅅ",    // 겹자음
                "ㄵ" => "ㄴㅈ",
                "ㄶ" => "ㄴㅎ",
                "ㄺ" => "ㄹㄱ",
                "ㄻ" => "ㄹㅁ",
                "ㄼ" => "ㄹㅂ",
                "ㄽ" => "ㄹㅅ",
                "ㄾ" => "ㄹㅌ",
                "ㄿ" => "ㄹㅍ",
                "ㅀ" => "ㄹㅎ",
                "ㅄ" => "ㅂㅅ",
                _ => _inputValue,  // 그 외는 그대로 반환
            };
        }
        //---------------------------------------------------------------------------------------------



        //-------------------------------------------------------- A매트릭스 분석 --------------------------------------------------------
        //-------------------------------- 2수조 난수 추출 --------------------------------
        private string getNum(string _menu, string _contentAddress)
        {
            string[] locationInfo = _contentAddress.Split("to");       // MetrixA1to1 형식의 데이터를 MetrixA1, 1 형식으로 나눔

            string xNum = locationInfo[0].Substring(7);     // X에 해당하는 위치값
            string yNum = locationInfo[1];                  // Y에 해당하는 위치값

            string RNum = FindControl(this, $"Metrix{_menu}XKey{xNum}").Text + FindControl(this, $"Metrix{_menu}YKey{yNum}").Text;

            return RNum;
        }
        //----------------------------------------------------------------------------------


        //-------------------------------- 위치값 치환 --------------------------------
        private Dictionary<string, string> GetLocationValue(string _inputValue, string _menu, Dictionary<string, string> _matchedData)      // 입력값, 동작할 매트릭스, 추출된 데이터를 저장할 Dictionary를 매개변수로 받음
        {
            for (int row = 1; row <= 10; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (_inputValue.Contains(ctrl.Text) && !_matchedData.ContainsKey(ctrl.Text))         // 입력값이 내부치를 포함하고 있다면 Key 값을 내부치로 Value 값을 추출된 난수로 할당
                    {
                        _matchedData.Add(ctrl.Text, getNum(_menu, controlName));
                    }
                }
            }

            return _matchedData;
        }
        //-----------------------------------------------------------------------------


        //-------------------------------- 10개마다 라인개행 --------------------------------
        public static string InsertNewLine(string input, int groupSize = 10)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < parts.Length; i++)
            {
                result.Append(parts[i]);

                if (i != parts.Length - 1)
                    result.Append(' ');

                // groupSize개마다 줄바꿈
                if ((i + 1) % groupSize == 0 && i != parts.Length - 1)
                    result.Append(Environment.NewLine);
            }

            return result.ToString();
        }
        //-----------------------------------------------------------------------------------


        private void DecipherAMetrix(string _menu)      // 현재 2수조의 경우만 작성
        {
            string inputValue = FindControl(this, $"Input_{_menu}").Text;           // 입력값
            string removedSameValue = inputValue;               // 문장에서 이미 난수 추출이된 값을 제외하기 위해 사용한 변수
            Dictionary<string, string> matchedData = new();     // Key 값을 입력값으로, Value 값을 난수로 가지는 Dictionary



            matchedData = GetLocationValue(inputValue, _menu, matchedData); // 입력값에 해당하는 내부치와 난수 매칭



            if (matchedData.Count > 0)          // 문장 또는 단어, 한 글자로 매칭된 데이터가 존재하는 경우 해당 단어들 제외
            {
                foreach (KeyValuePair<string, string> item in matchedData)
                {
                    try
                    {
                        removedSameValue = removedSameValue.Replace(item.Key, "");
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


            if (removedSameValue.Length > 0) // 문장, 단어, 한 글자 단위로 나눈 후 변환되지 않은 단어가 남아 있을 경우
            {
                string[] restWord = new string[removedSameValue.Length];


                for (int i = 0; i < removedSameValue.Length; i++)       // 남아있는 문장을 한 글자 단위로 나누기
                {
                    restWord[i] = removedSameValue[i].ToString();
                }

                foreach (var eachWord in restWord)      // 한 글자씩 반복문을 돌려 난수를 추출
                {
                    string detachedWord = SplitKorean(eachWord);      // 자모음 분리 >> 하나의 string으로 출력됨 ex) 강 -> ㄱㅏㅇ
                    inputValue = inputValue.Replace(eachWord, detachedWord);      // 입력값 중 난수로 치환되지 않은 단어를 자모음으로 분리하여 저장

                    for (int cnt = 0; cnt < detachedWord.Length; cnt++)
                    {
                        GetLocationValue(detachedWord[cnt].ToString(), _menu, matchedData); // 자모음 분리된 단어를 다시 난수로 치환하기 위해 GetLocationValue 호출
                    }
                }
            }


            string tempInput = inputValue;            // inputValue로 반복하는동안 inputValue의 값을 조정할 임시 변수

            for (int dtc = 0; dtc < inputValue.Length; dtc++)   // 겹자음 등을 각각의 단자음으로 구성하였을 경우 분리 시키는 로직
            {
                tempInput = tempInput.Replace(inputValue[dtc].ToString(), DetachedElement(inputValue[dtc].ToString())); // 입력값 중 모음, 쌍자음, 겹자음을 분리하여 저장
            }

            inputValue = tempInput;        // 분리된 모음, 쌍자음, 겹자음을 포함한 입력값으로 변경



            var sortedKeys = matchedData.Keys.OrderByDescending(k => k.Length).ToList();   // Key값의 길이 기준으로 내림차순 정렬
            string result = "";         // 결과를 저장할 변수

            while (inputValue.Length > 0)
            {
                string buffer = ""; // 잘라낸 글자들 저장
                bool matched = false;

                // inputValue에서 뒤에서부터 한 글자씩 잘라가며 매칭 시도
                for (int len = inputValue.Length; len > 0; len--)
                {
                    string current = inputValue.Substring(0, len); // 앞에서 len만큼 자른 부분

                    foreach (var key in sortedKeys)
                    {
                        if (current == key)
                        {
                            result += matchedData[key] + " ";
                            inputValue = inputValue.Substring(len); // 매칭된 부분 제거
                            inputValue = buffer + inputValue;       // 자른 부분 복구
                            matched = true;
                            break;
                        }
                    }

                    if (matched)
                        break;

                    // 매칭 실패 시, 맨 끝 글자를 buffer로 이동
                    buffer = inputValue[inputValue.Length - 1] + buffer;
                    inputValue = inputValue.Substring(0, inputValue.Length - 1);
                }

                // 매칭된 게 하나도 없고 inputValue가 줄어든 상태로 남아 있으면 무한 루프 방지
                if (!matched)
                {
                    Console.WriteLine($"[경고] 매칭 실패: '{inputValue}'는 어떤 키에도 해당하지 않습니다.");
                    break;
                }
            }


            FindControl(this, $"Output_{_menu}").Text = InsertNewLine(result); // 최종적으로 난수로 치환된 문자열을 출력창에 표시, 10개마다 개행 처리
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        
        

        //-------------------------------------------------------- B매트릭스 분석 --------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------



        //-------------------------------------------------------- C매트릭스 분석 --------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------



        //---------------------------------------------------------- 버튼 이벤트 기능 ----------------------------------------------------------
        //----------------------- 복사, 붙여넣기 옵션 -----------------------

        private void CopyKeyValue(string _menu)             // Key값 복사
        {
            int index = 0;

            for (int keyNum = 1; keyNum <= 10; keyNum++)
            {
                string controlName1 = $"Metrix{_menu}XKey{keyNum}";
                string controlName2 = $"Metrix{_menu}YKey{keyNum}";

                Control ctrl1 = FindControl(this, controlName1);
                Control ctrl2 = FindControl(this, controlName2);

                if (ctrl1 is System.Windows.Forms.TextBox tb1)
                {
                    strArr_XKey[index] = tb1.Text;        // XKey 값을 글로벌에서 선언된 배열에 할당
                }
                else
                {
                    strArr_XKey[index] = string.Empty;
                }

                if (ctrl2 is System.Windows.Forms.TextBox tb2)
                {
                    strArr_YKey[index] = tb2.Text;        // YKey 값을 글로벌에서 선언된 배열에 할당
                }
                else
                {
                    strArr_YKey[index] = string.Empty;
                }

                index++;
            }
        }

        private void PasteKeyValue(string _menu)            // Key값 붙여넣기
        {
            int index = 0;

            for (int keyNum = 1; keyNum <= 10; keyNum++)
            {
                string controlName1 = $"Metrix{_menu}XKey{keyNum}";
                string controlName2 = $"Metrix{_menu}YKey{keyNum}";

                Control ctrl1 = FindControl(this, controlName1);
                Control ctrl2 = FindControl(this, controlName2);

                if (ctrl1 is System.Windows.Forms.TextBox tb1)
                {
                    tb1.Text = strArr_XKey[index];        // XKey 배열에 저장된 값을 할당
                }

                if (ctrl2 is System.Windows.Forms.TextBox tb2)
                {
                    tb2.Text = strArr_YKey[index];        // YKey 배열에 저장된 값을 할당
                }

                index++;
            }

        }


        private void CopyContent(string _menu)             // 내부치 복사
        {
            int index = 0;

            for (int row = 1; row <= 10; row++) // X
            {
                for (int col = 1; col <= 10; col++) // Y
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (ctrl is System.Windows.Forms.TextBox tb)
                    {
                        strArr_TempContent[index] = tb.Text;        // 내부치 값을 글로벌에서 선언된 배열에 할당
                    }
                    else
                    {
                        strArr_TempContent[index] = string.Empty;
                    }

                    index++;
                }
            }
        }

        private void PasteContent(string _menu)            // 내부치 붙여넣기
        {
            int index = 0;

            for (int row = 1; row <= 10; row++) // X
            {
                for (int col = 1; col <= 10; col++) // Y
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (ctrl is System.Windows.Forms.TextBox tb)
                    {
                        tb.Text = strArr_TempContent[index];        // 내부치 배열에 저장된 값을 할당
                    }

                    index++;
                }
            }
        }
        //-----------------------------------------------------------


        //----------------------- 삭제 옵션 -----------------------
        private void DelCols(string _menu)      // 지시부 삭제
        {
            for (int row = 1; row <= 2; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    string controlName1 = $"MultiVal{_menu}X{row}to{col}";
                    string controlName2 = $"MultiVal{_menu}Y{row}to{col}";

                    Control ctrl1 = FindControl(this, controlName1);
                    Control ctrl2 = FindControl(this, controlName2);

                    if (ctrl1 is System.Windows.Forms.TextBox tb1)
                    {
                        tb1.Text = "";
                    }

                    if (ctrl2 is System.Windows.Forms.TextBox tb2)
                    {
                        tb2.Text = "";
                    }
                }
            }
        }

        private void DelKeyValues(string _menu)      // Key값 삭제
        {
            for (int row = 1; row <= 10; row++)
            {
                string controlName1 = $"Metrix{_menu}XKey{row}";
                string controlName2 = $"Metrix{_menu}YKey{row}";

                Control ctrl1 = FindControl(this, controlName1);
                Control ctrl2 = FindControl(this, controlName2);

                if (ctrl1 is System.Windows.Forms.TextBox tb1)
                {
                    tb1.Text = "";
                }

                if (ctrl2 is System.Windows.Forms.TextBox tb2)
                {
                    tb2.Text = "";
                }
            }
        }


        private void DelContent(string _menu)      // 내부치 삭제
        {
            for (int row = 1; row <= 10; row++) // X
            {
                for (int col = 1; col <= 10; col++) // Y
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (ctrl is System.Windows.Forms.TextBox tb)
                    {
                        tb.Text = "";
                    }
                }
            }
        }
        //-------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------






        //---------------------------------------------------------- 버튼 이벤트 사용 ----------------------------------------------------------
        //Key값 복사 ----------------------------------------------
        private void KeyValCopy1_Click(object sender, EventArgs e)
        {
            CopyKeyValue("A");
        }

        private void KeyValCopy2_Click(object sender, EventArgs e)
        {
            CopyKeyValue("B");
        }

        private void KeyValCopy3_Click(object sender, EventArgs e)
        {
            CopyKeyValue("C");
        }
        //----------------------------------------------------------


        //Key값 붙여넣기 -------------------------------------------
        private void KeyValPaste1_Click(object sender, EventArgs e)
        {
            PasteKeyValue("A");
        }

        private void KeyValPaste2_Click(object sender, EventArgs e)
        {
            PasteKeyValue("B");
        }

        private void KeyValPaste3_Click(object sender, EventArgs e)
        {
            PasteKeyValue("C");
        }
        //----------------------------------------------------------


        //내부치 복사 ----------------------------------------------
        private void ContentCopy1_Click(object sender, EventArgs e)
        {
            CopyContent("A");
        }

        private void ContentCopy2_Click(object sender, EventArgs e)
        {
            CopyContent("B");
        }

        private void ContentCopy3_Click(object sender, EventArgs e)
        {
            CopyContent("C");
        }
        //----------------------------------------------------------


        //내부치 붙여넣기 ------------------------------------------
        private void ContentPaste1_Click(object sender, EventArgs e)
        {
            PasteContent("A");
        }

        private void ContentPaste2_Click(object sender, EventArgs e)
        {
            PasteContent("B");
        }

        private void ContentPaste3_Click(object sender, EventArgs e)
        {
            PasteContent("C");
        }
        //----------------------------------------------------------


        //지시부 삭제 ----------------------------------------------
        private void RowDel1_Click(object sender, EventArgs e)
        {
            DelCols("A");
        }

        private void RowDel2_Click(object sender, EventArgs e)
        {
            DelCols("B");
        }
        private void RowDel3_Click(object sender, EventArgs e)
        {
            DelCols("C");
        }
        //----------------------------------------------------------


        //Key값 삭제 ----------------------------------------------
        private void KeyValDel1_Click(object sender, EventArgs e)
        {
            DelKeyValues("A");
        }

        private void KeyValDel2_Click(object sender, EventArgs e)
        {
            DelKeyValues("B");
        }

        private void KeyValDel3_Click(object sender, EventArgs e)
        {
            DelKeyValues("C");
        }
        //--------------------------------------------------------


        //내부치 삭제 ----------------------------------------------
        private void ContentDel1_Click(object sender, EventArgs e)
        {
            DelContent("A");
        }

        private void ContentDel2_Click(object sender, EventArgs e)
        {
            DelContent("B");
        }

        private void ContentDel3_Click(object sender, EventArgs e)
        {
            DelContent("C");
        }
        //----------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}
