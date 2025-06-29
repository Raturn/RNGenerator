using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RNGenerator
{
    public partial class Scenario : Form
    {

        public static Scenario Instance;


        public Scenario()
        {
            InitializeComponent();
            Instance = this;
            scenarioInput.TextChanged += ScenarioInput_TextChanged;
        }


        private void ScenarioInput_TextChanged(object sender, EventArgs e)
        {
            lengthChange.Text = scenarioInput.Text.Length.ToString();
        }


        private static string partsTemp = "";     // 자모 입력 창 값 저장
        private static string letterTemp = "";    // 글자 입력 창 값 저장
        private static string wordTemp = "";      // 단어 입력 창 값 저장
        private static string selfTemp = "";      // 수동 입력 창 값 저장


        private static string partsNumOutput = "";     // 자모 입력 및 난수 값
        private static string letterNumOutput = "";    // 글자 입력 및 난수 값
        private static string wordNumOutput = "";      // 단어 입력 및 난수 값
        private static string selfNumOutput = "";      // 수동 입력 및 난수 값

        private static Dictionary<string, string> matchedData = new Dictionary<string, string>();       // 격판의 매칭된 데이터만을 저장 >> 결과값이 아닌 답지

        string[] tableClassArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" }; // 테이블 클래스 이름 배열

        private float currentZoom = 1.0f;
        private readonly float minZoom = 0.5f;
        private readonly float maxZoom = 2.0f;



        // 폼이 로드될 때 호출되는 이벤트 핸들러 >> 줌 기능
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                float scale = e.Delta > 0 ? 1.1f : 0.9f;
                float newZoom = currentZoom * scale;

                if (newZoom >= minZoom && newZoom <= maxZoom)
                {
                    foreach (Control child in this.Controls)
                    {
                        ScaleControl(child, scale);
                    }

                    currentZoom = newZoom;
                }
            }

            base.OnMouseWheel(e);
        }

        // 비율에 맞게 크기 줄이기
        private void ScaleControl(Control ctrl, float scale)
        {
            // 위치와 크기 조정
            ctrl.Left = (int)(ctrl.Left * scale);
            ctrl.Top = (int)(ctrl.Top * scale);
            ctrl.Width = (int)(ctrl.Width * scale);
            ctrl.Height = (int)(ctrl.Height * scale);

            // 폰트 크기 조정
            var oldFont = ctrl.Font;
            float newFontSize = oldFont.Size * scale;
            ctrl.Font = new System.Drawing.Font(oldFont.FontFamily, newFontSize, oldFont.Style);

            // 자식 컨트롤도 재귀적으로 조정
            foreach (Control child in ctrl.Controls)
            {
                ScaleControl(child, scale);
            }
        }
        // ------------------------------------------------



        //-------------------------------- 한글 범위 상수 지정  --------------------------------
        // 한글 음절 조합을 위한 상수 정의 - 함수를 호출할 때 마다 선언하지 않도록 글로벌 변수로 선언
        private const int HANGUL_BASE = 0xAC00;
        private const int INITIAL_BASE = 0x1100;
        private const int MEDIAL_BASE = 0x1161;
        private const int FINAL_BASE = 0x11A7;

        private static readonly char[] Initials = {
        'ㄱ','ㄲ','ㄴ','ㄷ','ㄸ','ㄹ','ㅁ','ㅂ','ㅃ','ㅅ','ㅆ','ㅇ','ㅈ','ㅉ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ'
        };

        private static readonly char[] Medials = {
        'ㅏ','ㅐ','ㅑ','ㅒ','ㅓ','ㅔ','ㅕ','ㅖ','ㅗ','ㅘ','ㅙ','ㅚ',
        'ㅛ','ㅜ','ㅝ','ㅞ','ㅟ','ㅠ','ㅡ','ㅢ','ㅣ'
        };

        private static readonly char[] Finals = {
        '\0','ㄱ','ㄲ','ㄳ','ㄴ','ㄵ','ㄶ','ㄷ','ㄹ','ㄺ','ㄻ','ㄼ','ㄽ','ㄾ','ㄿ','ㅀ','ㅁ','ㅂ','ㅄ','ㅅ',
        'ㅆ','ㅇ','ㅈ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ'
        };
        //--------------------------------------------------------------------------------------



        //-------------------------------- 한글을 자모로 분리하는 함수 --------------------------------
        public static string SplitParts(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return "";

            input = input.Replace(" ", ""); // 공백 제거

            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (c >= 0xAC00 && c <= 0xD7A3)
                {
                    int unicode = c - HANGUL_BASE;
                    int initialIndex = unicode / (21 * 28);
                    int medialIndex = (unicode % (21 * 28)) / 28;
                    int finalIndex = unicode % 28;

                    result.Append(Initials[initialIndex]).Append(' ');
                    result.Append(Medials[medialIndex]).Append(' ');
                    if (finalIndex != 0)
                    {
                        result.Append(Finals[finalIndex]).Append(' ');
                    }
                }
                else
                {
                    // 한글이 아니면 그대로 출력
                    result.Append(c).Append(' ');
                }
            }

            // static 변수가 아닐경우 개체 참조 필수
            string[] resultArray = result.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in resultArray)
            {
                if (!matchedData.ContainsKey(item))
                {  // Dictionary에 해당 데이터가 없다면
                    matchedData.Add(item, "");         // 자모 분리 데이터 저장, value는 빈 문자열로 초기화
                }
            }

            // 마지막 공백 제거
            return result.ToString().TrimEnd();
        }
        //---------------------------------------------------------------------------------------------



        //-------------------------- 한글을 한 글자씩 분리 --------------------------
        public static string SplitLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return String.Empty;

            // 공백 제거
            input = input.Replace(" ", "");




            // Dictionary에 값이 없으면 추가
            for (int cnt = 0; cnt < input.Length; cnt++)
            {
                if (!matchedData.ContainsKey(input[cnt].ToString()))
                {
                    matchedData.Add(input[cnt].ToString(), "");  // 초기값은 빈 문자열
                }
            }



            // 한 글자씩 띄어쓰기 삽입
            string spaced = String.Join(" ", input.ToCharArray()).TrimEnd();

            return spaced;
        }
        //---------------------------------------------------------------------------



        //-------------------------- 한글을 한 단어씩 분리 --------------------------
        public static string SplitWord(string input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            // 띄어쓰기 처리 대상 문자 집합
            char[] specialChars = { ',', '.', '(', ')', '-', '<', '>', '《', '》', '!', '≪', '≫' };

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (Array.IndexOf(specialChars, c) >= 0)
                {
                    // 앞 문자가 공백이 아니면 공백 추가
                    if (i > 0 && input[i - 1] != ' ')
                        result.Append(' ');

                    result.Append(c);

                    // 뒤 문자가 공백이 아니면 공백 추가
                    if (i < input.Length - 1 && input[i + 1] != ' ')
                        result.Append(' ');
                }
                else
                {
                    result.Append(c);
                }
            }


            string[] resultArray = result.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in resultArray)
            {
                if (!matchedData.ContainsKey(item)) // Dictionary에 해당 데이터가 없다면
                {
                    matchedData.Add(item, ""); // 단어 분리 데이터 저장, value는 빈 문자열로 초기화
                }
            }


            return result.ToString();
        }
        //---------------------------------------------------------------------------



        //-------------------------------- 10조마다 라인개행 --------------------------------
        public static string InsertNewLine(string input)
        {
            //if (String.IsNullOrWhiteSpace(input))
            //    return "";

            //string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //StringBuilder result = new StringBuilder();

            //for (int i = 0; i < tokens.Length; i++)
            //{
            //    result.Append(tokens[i]);

            //    // 마지막이 아니라면 구분자 추가
            //    if (i != tokens.Length - 1)
            //    {
            //        if ((i + 1) % 10 == 0)
            //            result.Append(Environment.NewLine); // 10번째마다 줄바꿈
            //        else
            //            result.Append(' '); // 그 외는 공백
            //    }
            //}

            //return result.ToString();

            if (string.IsNullOrWhiteSpace(input))
                return "";

            string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < tokens.Length; i++)
            {
                result.Append(tokens[i]);

                // 마지막 토큰이 아니라면
                if (i != tokens.Length - 1)
                {
                    if ((i + 1) % 10 == 0)
                    {
                        // 10개마다 줄바꿈 2번(빈 줄 포함)
                        result.Append(Environment.NewLine);
                        result.Append(Environment.NewLine);
                    }
                    else
                    {
                        result.Append(' ');
                    }
                }
            }

            return result.ToString();
        }
        //-----------------------------------------------------------------------------------




        //---------------------------------------------------------- 버튼 이벤트 ----------------------------------------------------------
        //---------------------------------- 시나리오 저장 ----------------------------------
        private void scenarioSave_Click(object sender, EventArgs e)         // 저장 버튼 클릭 시 이벤트
        {
            string inputText = scenarioInput.Text;
            string folderPath = @"C:\CA연구용역\시나리오";

            // 파일명: scenarioName TextBox에서 가져오기
            string rawFileName = scenarioName.Text.Trim();
            string[] exceptionWord = new string[] { @"\", "/", ":", "*", "?", "\"", ">", "<", "|" };

            // 파일명으로 부적절한 문자 제거
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                foreach (string word in exceptionWord)
                {
                    rawFileName = rawFileName.Replace(c.ToString(), word);
                }
            }

            // 비어 있으면 기본값 지정
            if (String.IsNullOrWhiteSpace(rawFileName))
            {
                rawFileName = "UntitledScenario";
            }

            string fileName = rawFileName + ".txt";
            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                // 폴더가 없으면 생성
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 파일이 존재하는 경우
                if (File.Exists(filePath))
                {
                    DialogResult result = MessageBox.Show(
                        "같은 이름을 가진 파일이 이미 존재합니다. 덮어쓰기를 하시겠습니까?",
                        "파일 덮어쓰기 확인",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question
                    );

                    if (result != DialogResult.OK)
                    {
                        return; // 취소 선택 시 저장하지 않음
                    }
                }

                // 텍스트 파일로 저장
                File.WriteAllText(filePath, inputText);

                // 저장 완료 메시지
                MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"파일 저장 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //-----------------------------------------------------------------------------------



        //-------------------------------- 시나리오 불러오기 --------------------------------
        private void scenarioLoad_Click(object sender, EventArgs e)
        {
            bool hasExistingData = !String.IsNullOrWhiteSpace(scenarioName.Text) || !String.IsNullOrWhiteSpace(scenarioInput.Text);

            if (hasExistingData)        // 클릭 시 작성 중인 데이터가 이미 존재한다면 덮어쓰기 확인 메시지 박스 표시
            {
                DialogResult result = MessageBox.Show(
                    "작성 중인 데이터가 있습니다. 데이터를 덮어쓰시겠습니까?",
                    "데이터 확인",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.OK)
                {
                    return; // 취소한 경우 아무것도 하지 않음
                }
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "텍스트 파일 (*.txt)|*.txt";
                openFileDialog.Title = "시나리오 파일 불러오기";
                openFileDialog.InitialDirectory = @"C:\CA연구용역\시나리오";        // 기본 폴더 설정

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    try
                    {
                        // 파일 내용 읽기
                        string content = File.ReadAllText(filePath);

                        // 파일명 (확장자 제외) 추출
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                        // TextBox에 값 설정
                        scenarioName.Text = fileNameWithoutExtension;
                        scenarioInput.Text = content;

                        MessageBox.Show("파일을 성공적으로 불러왔습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"파일을 불러오는 중 오류가 발생했습니다:\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------------



        //----------------------- 시나리오를 각각의 형태로 변환하여 각 TextBox에 뿌려줌 -----------------------
        private void scenarioChange_Click(object sender, EventArgs e)
        {
            matchedData.Clear();    // 자모 분리 데이터 초기화

            partsNumOutput = "";       // 자모 난수 값 초기화
            letterNumOutput = "";      // 글자 난수 값 초기화
            wordNumOutput = "";        // 단어 난수 값 초기화
            selfNumOutput = "";        // 수동 난수 값 초기화

            partsInput.Text = InsertNewLine(SplitParts(scenarioInput.Text));        // 입력값을 자모로 분리하여 할당
            letterInput.Text = InsertNewLine(SplitLetter(scenarioInput.Text));      // 입력값을 한 글자씩 분리하여 할당
            wordInput.Text = InsertNewLine(SplitWord(scenarioInput.Text));          // 입력값을 단어로 분리하여 할당
        }
        //-----------------------------------------------------------------------------------------------------



        //---------------------------------------------- 각 케이스별 변환 버튼 ----------------------------------------------
        // 자모 난수 변환 버튼
        private void partsChange_Click(object sender, EventArgs e)
        {
            //partsInput.Text = InsertNewLine(SplitParts(scenarioInput.Text));        // 입력값을 자모로 분리하여 할당  >>  연속으로 누를경우 초기화될 수 있도록


            matchedData = RNGenerator.Instance.MatchedData(matchedData);

            string tempInput = partsInput.Text; // 기존 텍스트 저장
            tempInput = tempInput.Replace("\r", "").Replace("\n", "");  // 줄바꿈 제거


            if (tempInput.Contains("[") && tempInput.Contains("]"))
            {
                tempInput = RemoveOnlyRNum(tempInput); // 대괄호와 대괄호 안의 값 제거  >>  연속 변환 입력 시   사과[57][??] 과 같은 상황을 방지하기 위함
                //tempInput = Regex.Replace(tempInput, @"(\r?\n){3,}", "\n\n");
            }

            foreach (var item in matchedData)
            {
                if (String.IsNullOrEmpty(item.Value) || String.IsNullOrWhiteSpace(item.Value))
                {
                    tempInput = tempInput.Replace(item.Key, item.Key + "[??] ");
                    partsNumOutput += "?? "; // 자모 난수 값이 식별되지 않음을 표시
                }
                else
                {
                    tempInput = tempInput.Replace(item.Key, item.Key + "[" + item.Value + "] ");
                    partsNumOutput += item.Value + " "; // 자모 난수 값 저장
                }
            }

            // 입력값에서 공백, 줄바꿈 제거
            string cleanedInput = tempInput.Replace(" ", "").Replace("\r", "").Replace("\n", "");

            // 대괄호 안의 값 추출
            var matches = Regex.Matches(cleanedInput, @"\[(.*?)\]");

            string onlyBracketValues = "";
            foreach (Match match in matches)
            {
                onlyBracketValues += match.Groups[1].Value + " ";  // 공백으로 조수 구분
                                                                   // onlyBracketValues += match.Groups[1].Value + " "; // ← 공백 구분 원할 시 이쪽 사용
            }

            partsNumOutput = onlyBracketValues.Trim(); // 자모 난수 값에 대괄호 안의 값만 저장


            partsInput.Text = InsertNewLine(tempInput); // 줄바꿈 처리
            partsTemp = InsertNewLine(tempInput);   // 다른 함수에서 사용하기 위해 글로벌 변수에 저장
            MessageBox.Show("자모 난수 생성이 완료 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (partsNumOutput.Length > 500)
            {
                MessageBox.Show("난수가 500자를 넘어갔습니다. 난수의 길이를500자 이하로 맞춰주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 글자 난수 변환 버튼
        private void letterChange_Click(object sender, EventArgs e)
        {
            //letterInput.Text = InsertNewLine(SplitLetter(scenarioInput.Text));      // 입력값을 한 글자씩 분리하여 할당  >>  연속으로 누를경우 초기화될 수 있도록


            matchedData = RNGenerator.Instance.MatchedData(matchedData);

            string tempInput = letterInput.Text; // 기존 텍스트 저장
            tempInput = tempInput.Replace("\r", "").Replace("\n", "");  // 줄바꿈 제거


            if (tempInput.Contains("[") && tempInput.Contains("]"))
            {
                tempInput = RemoveOnlyRNum(tempInput); // 대괄호와 대괄호 안의 값 제거  >>  연속 변환 입력 시   사과[57][??] 과 같은 상황을 방지하기 위함
                //tempInput = Regex.Replace(tempInput, @"(\r?\n){3,}", "\n\n");
            }

            foreach (var item in matchedData)
            {
                if (String.IsNullOrEmpty(item.Value) || String.IsNullOrWhiteSpace(item.Value))
                {
                    tempInput = tempInput.Replace(item.Key, item.Key + "[??] ");
                }
                else
                {
                    tempInput = tempInput.Replace(item.Key, item.Key + "[" + item.Value + "] ");
                }
            }


            // 입력값에서 공백, 줄바꿈 제거
            string cleanedInput = tempInput.Replace(" ", "").Replace("\r", "").Replace("\n", "");

            // 대괄호 안의 값 추출
            var matches = Regex.Matches(cleanedInput, @"\[(.*?)\]");

            string onlyBracketValues = "";
            foreach (Match match in matches)
            {
                onlyBracketValues += match.Groups[1].Value + " ";  // 공백으로 조수 구분
                                                                   // onlyBracketValues += match.Groups[1].Value + " "; // ← 공백 구분 원할 시 이쪽 사용
            }

            letterNumOutput = onlyBracketValues.Trim(); // 자모 난수 값에 대괄호 안의 값만 저장

            letterInput.Text = InsertNewLine(tempInput); // 줄바꿈 처리
            letterTemp = InsertNewLine(tempInput);   // 다른 함수에서 사용하기 위해 글로벌 변수에 저장
            MessageBox.Show("글자 난수 생성이 완료 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (letterNumOutput.Length > 500)
            {
                MessageBox.Show("난수가 500자를 넘어갔습니다. 난수의 길이를500자 이하로 맞춰주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        // 단어 난수 변환 버튼
        private void wordChange_Click(object sender, EventArgs e)
        {
            //wordInput.Text = InsertNewLine(SplitWord(scenarioInput.Text));          // 입력값을 단어로 분리하여 할당  >>  연속으로 누를경우 초기화될 수 있도록


            matchedData = RNGenerator.Instance.MatchedData(matchedData);
            string tempInput = wordInput.Text;
            tempInput = tempInput.Replace("\r", "").Replace("\n", "");  // 줄바꿈 제거


            if (tempInput.Contains("[") && tempInput.Contains("]"))
            {
                tempInput = RemoveOnlyRNum(tempInput); // 대괄호와 대괄호 안의 값 제거  >>  연속 변환 입력 시   사과[57][??] 과 같은 상황을 방지하기 위함
                //tempInput = Regex.Replace(tempInput, @"(\r?\n){3,}", "\n\n");
            }

            wordNumOutput = ""; // ← 기존 값을 초기화

            var sortedData = matchedData
                .OrderByDescending(kv => kv.Key.Length)
                .ToList();

            Dictionary<string, string> placeholderMap = new Dictionary<string, string>();
            int placeholderIndex = 0;

            // 1단계: 자리표시자로 먼저 치환
            foreach (var item in sortedData)
            {
                if (!string.IsNullOrWhiteSpace(item.Key))
                {
                    string placeholder = $"__PLACEHOLDER_{placeholderIndex}__";
                    placeholderIndex++;

                    tempInput = tempInput.Replace(item.Key, placeholder);

                    string valueToInsert = item.Key + "[" + (string.IsNullOrWhiteSpace(item.Value) ? "??" : item.Value) + "]";
                    placeholderMap[placeholder] = valueToInsert;
                }
            }

            // 2단계: 자리표시자를 최종 텍스트로 치환하고 wordNumOutput 구성
            foreach (var kv in placeholderMap)
            {
                tempInput = tempInput.Replace(kv.Key, kv.Value + " ");
            }


            // 입력값에서 공백, 줄바꿈 제거
            string cleanedInput = tempInput.Replace(" ", "").Replace("\r", "").Replace("\n", "");

            // 대괄호 안의 값 추출
            var matches = Regex.Matches(cleanedInput, @"\[(.*?)\]");

            string onlyBracketValues = "";
            foreach (Match match in matches)
            {
                onlyBracketValues += match.Groups[1].Value + " ";  // 공백으로 조수 구분
                                                                   // onlyBracketValues += match.Groups[1].Value + " "; // ← 공백 구분 원할 시 이쪽 사용
            }

            wordNumOutput = onlyBracketValues.Trim(); // 자모 난수 값에 대괄호 안의 값만 저장


            wordInput.Text = InsertNewLine(tempInput); // 줄바꿈 처리
            wordTemp = InsertNewLine(tempInput);   // 다른 함수에서 사용하기 위해 글로벌 변수에 저장
            MessageBox.Show("단어 난수 생성이 완료 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (wordNumOutput.Length > 500)
            {
                MessageBox.Show("난수가 500자를 넘어갔습니다. 난수의 길이를500자 이하로 맞춰주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        // 수동 난수 변환 버튼
        private void selfChange_Click(object sender, EventArgs e)
        {
            // 띄어쓰기와 줄바꿈을 기준으로 입력값을 분리하여 배열로 저장
            string[] selfArray = selfInput.Text
                .Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // 배열의 각 요소를 Dictionary에 추가
            foreach (string token in selfArray)
            {
                if (!matchedData.ContainsKey(token))
                {
                    matchedData.Add(token, "");
                }
            }

            // 수동 입력 데이터를 내부치와 매칭
            matchedData = RNGenerator.Instance.MatchedData(matchedData);

            var sortedData = matchedData
                .OrderByDescending(kv => kv.Key.Length)
                .ToList();

            string tempInput = selfInput.Text; // 기존 텍스트 저장
            selfNumOutput = "";                // 난수 출력용 문자열 초기화
            tempInput = tempInput.Replace("\r", "").Replace("\n", "");  // 줄바꿈 제거


            if (tempInput.Contains("[") && tempInput.Contains("]"))
            {
                tempInput = RemoveOnlyRNum(tempInput); // 대괄호와 대괄호 안의 값 제거  >>  연속 변환 입력 시   사과[57][??] 과 같은 상황을 방지하기 위함
                //tempInput = Regex.Replace(tempInput, @"(\r?\n){3,}", "\n\n");
            }


            Dictionary<string, string> placeholderMap = new Dictionary<string, string>();
            int placeholderIndex = 0;

            // 1단계: 자리표시자로 먼저 치환
            foreach (var item in sortedData)
            {
                if (!string.IsNullOrWhiteSpace(item.Key))
                {
                    string placeholder = $"__PLACEHOLDER_{placeholderIndex}__";
                    placeholderIndex++;

                    tempInput = tempInput.Replace(item.Key, placeholder);

                    string valueToInsert = item.Key + "[" + (string.IsNullOrWhiteSpace(item.Value) ? "??" : item.Value) + "]";
                    placeholderMap[placeholder] = valueToInsert;
                }
            }

            // 2단계: 자리표시자를 최종 텍스트로 치환하고 selfNumOutput 구성
            foreach (var kv in placeholderMap)
            {
                tempInput = tempInput.Replace(kv.Key, kv.Value + " ");
            }



            // 입력값에서 공백, 줄바꿈 제거
            string cleanedInput = tempInput.Replace(" ", "").Replace("\r", "").Replace("\n", "");

            // 대괄호 안의 값 추출
            var matches = Regex.Matches(cleanedInput, @"\[(.*?)\]");

            string onlyBracketValues = "";
            foreach (Match match in matches)
            {
                onlyBracketValues += match.Groups[1].Value + " ";  // 공백으로 조수 구분
                                                                   // onlyBracketValues += match.Groups[1].Value + " "; // ← 공백 구분 원할 시 이쪽 사용
            }

            selfNumOutput = onlyBracketValues.Trim(); // 자모 난수 값에 대괄호 안의 값만 저장

            selfInput.Text = InsertNewLine(tempInput); // 줄바꿈 처리
            selfTemp = InsertNewLine(tempInput);   // 다른 함수에서 사용하기 위해 글로벌 변수에 저장
            MessageBox.Show("수동 난수 생성이 완료 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (selfNumOutput.Length > 500)
            {
                MessageBox.Show("난수가 500자를 넘어갔습니다. 난수의 길이를500자 이하로 맞춰주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        // 수동 입력창 줄 바꾸기
        private void newLineBtn_Click(object sender, EventArgs e)
        {
            string tempInput = selfInput.Text;
            selfInput.Text = InsertNewLine(tempInput); // 공백 10개마다 줄바꿈 처리
        }



        // 자모 결과 저장
        private void partsSave_Click(object sender, EventArgs e)
        {
            string filePath = saveJsonOutput(partsNumOutput);
            MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 글자 결과 저장
        private void letterSave_Click(object sender, EventArgs e)
        {
            string filePath = saveJsonOutput(letterNumOutput);
            MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 단어 결과 저장
        private void wordSave_Click(object sender, EventArgs e)
        {
            string filePath = saveJsonOutput(wordNumOutput);
            MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 수동 결과 저장
        private void selfSave_Click(object sender, EventArgs e)
        {
            string filePath = saveJsonOutput(selfNumOutput);
            MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //------------------ 전체 저장 로직 ------------------
        private void RNAllSaveBtn1_Click(object sender, EventArgs e)
        {
            string filePath1 = saveJsonOutput(partsNumOutput);
            string filePath2 = saveJsonOutput(letterNumOutput);
            string filePath3 = saveJsonOutput(wordNumOutput);

            string selfContents = RNGenerator.Instance.FindControl(this, "selfInput").Text;

            if (!string.IsNullOrEmpty(selfContents))
            {
                string filePath4 = saveJsonOutput(selfNumOutput);

                MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}\n{filePath4}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RNAllSaveBtn2_Click(object sender, EventArgs e)
        {
            string filePath1 = saveJsonOutput(partsNumOutput);
            string filePath2 = saveJsonOutput(letterNumOutput);
            string filePath3 = saveJsonOutput(wordNumOutput);

            string selfContents = RNGenerator.Instance.FindControl(this, "selfInput").Text;

            if (!string.IsNullOrEmpty(selfContents))
            {
                string filePath4 = saveJsonOutput(selfNumOutput);

                MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}\n{filePath4}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RNAllSaveBtn3_Click(object sender, EventArgs e)
        {
            string filePath1 = saveJsonOutput(partsNumOutput);
            string filePath2 = saveJsonOutput(letterNumOutput);
            string filePath3 = saveJsonOutput(wordNumOutput);

            string selfContents = RNGenerator.Instance.FindControl(this, "selfInput").Text;

            if (!string.IsNullOrEmpty(selfContents))
            {
                string filePath4 = saveJsonOutput(selfNumOutput);

                MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}\n{filePath4}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RNAllSaveBtn4_Click(object sender, EventArgs e)
        {
            string filePath1 = saveJsonOutput(partsNumOutput);
            string filePath2 = saveJsonOutput(letterNumOutput);
            string filePath3 = saveJsonOutput(wordNumOutput);

            string selfContents = RNGenerator.Instance.FindControl(this, "selfInput").Text;

            if (!string.IsNullOrEmpty(selfContents))
            {
                string filePath4 = saveJsonOutput(selfNumOutput);

                MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}\n{filePath4}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //----------------------------------------------------



        //---------------------------------- 입력값 On/Off 버튼 클릭 이벤트 ----------------------------------
        private void partsInputSW_Click(object sender, EventArgs e)
        {
            InputValueOnly(partsTemp, RNGenerator.Instance.FindControl(this, "partsInput"));
        }

        private void letterInputSW_Click(object sender, EventArgs e)
        {
            InputValueOnly(letterTemp, RNGenerator.Instance.FindControl(this, "letterInput"));
        }

        private void wordInputSW_Click(object sender, EventArgs e)
        {
            InputValueOnly(wordTemp, RNGenerator.Instance.FindControl(this, "wordInput"));
        }

        private void selfInputSW_Click(object sender, EventArgs e)
        {
            InputValueOnly(selfTemp, RNGenerator.Instance.FindControl(this, "selfInput"));
        }
        //----------------------------------------------------------------------------------------------------



        //---------------------------------- 난수값 On/Off 버튼 클릭 이벤트 ----------------------------------
        private void partsNumSW_Click(object sender, EventArgs e)
        {
            RnumValueOnly(partsTemp, RNGenerator.Instance.FindControl(this, "partsInput"));
        }

        private void letterNumSW_Click(object sender, EventArgs e)
        {
            RnumValueOnly(letterTemp, RNGenerator.Instance.FindControl(this, "letterInput"));
        }

        private void wordNumSW_Click(object sender, EventArgs e)
        {
            RnumValueOnly(wordTemp, RNGenerator.Instance.FindControl(this, "wordInput"));
        }

        private void selfNumSW_Click(object sender, EventArgs e)
        {
            RnumValueOnly(selfTemp, RNGenerator.Instance.FindControl(this, "selfInput"));
        }
        //----------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------





        //---------------------------------- 버튼 클릭 이벤트의 필요에 의해 추가된 함수들 ----------------------------------
        // 입력값 On/Off
        private void InputValueOnly(string input, Control ctrl)
        {
            if (ctrl.Text.Contains("[") && ctrl.Text.Contains("]"))
            {
                // 정규식으로 [대괄호 내부 값] 포함한 부분 제거
                string result = Regex.Replace(input, @"\[[^\[\]]*\]", "");

                // 앞뒤 공백 제거
                result = result.Trim();

                ctrl.Text = result; // 결과를 컨트롤에 설정
            }
            else
            {
                ctrl.Text = input.Trim(); // 입력값 그대로 설정
            }
        }



        // 난수값만 On/Off
        private void RnumValueOnly(string input, Control ctrl)
        {
            if (ctrl.Text.Contains("[") && ctrl.Text.Contains("]"))
            {
                // 대괄호 안 값만 추출
                MatchCollection matches = Regex.Matches(input, @"\[(.*?)\]");

                // 띄어쓰기로 연결
                string result = string.Join(" ", matches.Cast<Match>().Select(m => m.Groups[1].Value));

                ctrl.Text = InsertNewLine(result); // 결과를 컨트롤에 설정
            }
            else
            {
                ctrl.Text = input.Trim(); // 입력값 그대로 설정
            }
        }



        private string saveJsonOutput(string numOutput)
        {
            int chunkSize = 400;
            var caList = new List<string>();

            // CA 필드 구성
            if (numOutput.Length > chunkSize)
            {
                for (int i = 0; i < numOutput.Length; i += chunkSize)
                {
                    int length = Math.Min(chunkSize, numOutput.Length - i);
                    string chunk = numOutput.Substring(i, length);
                    caList.Add(chunk);
                }
            }
            else
            {
                caList.Add(numOutput);
            }

            // Metrix 구성
            var metrix = new List<List<List<string>>>();

            for (int bundleIndex = 0; bundleIndex < 15; bundleIndex++)
            {
                var bundle = new List<List<string>>();

                for (int vectorIndex = 0; vectorIndex < 6; vectorIndex++)
                {
                    var vector = new List<string>();

                    for (int valueIndex = 0; valueIndex < 10; valueIndex++)
                    {
                        string selectTB = "";

                        switch (vectorIndex)
                        {
                            case 0:
                                selectTB = $"{tableClassArray[bundleIndex]}metrixX1_{valueIndex + 1}";
                                break;
                            case 1:
                                selectTB = $"{tableClassArray[bundleIndex]}keyX{valueIndex + 1}";
                                break;
                            case 2:
                                selectTB = $"{tableClassArray[bundleIndex]}metrixX2_{valueIndex + 1}";
                                break;
                            case 3:
                                selectTB = $"{tableClassArray[bundleIndex]}metrixY1_{valueIndex + 1}";
                                break;
                            case 4:
                                selectTB = $"{tableClassArray[bundleIndex]}keyY{valueIndex + 1}";
                                break;
                            case 5:
                                selectTB = $"{tableClassArray[bundleIndex]}metrixY2_{valueIndex + 1}";
                                break;
                        }

                        Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, selectTB);
                        string value = ctrl is TextBox tb ? tb.Text : "";
                        vector.Add(value);
                    }

                    bundle.Add(vector);
                }

                metrix.Add(bundle);
            }

            // JSON 객체 생성 (익명 타입)
            var jsonData = new
            {
                CA = caList,
                Decode = scenarioInput.Text,
                Metrix = metrix
            };

            // 저장 로직
            try
            {
                string directoryPath = @"C:\CA연구용역\생성된난수\2수조";
                string baseFileName = DateTime.Now.ToString("yyyy.MM.dd") + "_" + Controller.Instance.metrixName.Text + "_" + scenarioName.Text;
                string fileName = baseFileName + "_1.json";
                string filePath = Path.Combine(directoryPath, fileName);

                int fileIndex = 1;
                while (File.Exists(filePath))
                {
                    fileIndex++;
                    fileName = $"{baseFileName}_{fileIndex}.json";
                    filePath = Path.Combine(directoryPath, fileName);
                }

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var options = new JsonSerializerOptions { WriteIndented = false };  // False로 변경 시 들여쓰기 안 함
                string json = JsonSerializer.Serialize(jsonData, options);
                File.WriteAllText(filePath, json, Encoding.UTF8);                   // UTF-8로 인코딩

                return filePath;
            }
            catch (Exception)
            {
                MessageBox.Show("저장 중 오류가 발생되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }


        static string RemoveOnlyRNum(string input)
        {
            // [대괄호 안 내용] 전체 삭제
            return Regex.Replace(input, @"\[[^\[\]]*?\]", "");
        }
        //------------------------------------------------------------------------------------------------------------------



        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        //---------------------------------- 자동 Full 변환 ----------------------------------
        private void autoFullChange_Click(object sender, EventArgs e)
        {
            partsChange_Click(null, EventArgs.Empty);
            letterChange_Click(null, EventArgs.Empty);
            wordChange_Click(null, EventArgs.Empty);
        }
        //------------------------------------------------------------------------------------



        //---------------------------------- 자동 Full 저장 ----------------------------------
        private void autoFullSave_Click(object sender, EventArgs e)
        {
            string filePath1 = saveJsonOutput(partsNumOutput);
            string filePath2 = saveJsonOutput(letterNumOutput);
            string filePath3 = saveJsonOutput(wordNumOutput);
        }
        //------------------------------------------------------------------------------------



        //---------------------------------- 자동 Full 할당 ----------------------------------
        private void autoValueBtn_Click(object sender, EventArgs e)
        {
            string clearTB = "";
            Control clearControl = new Control();

            // 내부치 텍스트 박스 초기화
            for (int classIdx = 0; classIdx < 4; classIdx++)
            {
                for (int x = 1; x <= 10; x++)
                {
                    for (int y = 1; y <= 10; y++)
                    {
                        clearTB = $"{tableClassArray[classIdx]}var{x}_{y}";
                        clearControl = RNGenerator.Instance.FindControl(RNGenerator.Instance, clearTB);

                        clearControl.Text = "";
                    }
                }
            }


            Control control = new Control();    // 반복문에서 사용할 Control 생성 및 선언

            string innerText = "";              // innerText 초기화

            List<string> partsKeys = partsInput.Text
                .Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();

            List<string> letterKeys = letterInput.Text
                .Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();

            List<string> wordKeys = wordInput.Text
                .Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();



            for (int classIdx = 0; classIdx < 3; classIdx++)
            {
                int keyCount = 0;           // keys의 인덱스 저장 >> 테이블이 변경될 때마다 초기화
                Boolean isExitFlag = false; // 각 클래스의 반복문 탈출 플래그

                for (int x = 1; x <= 10; x++)
                {
                    for (int y = 1; y <= 10; y++)
                    {
                        innerText = $"{tableClassArray[classIdx]}var{x}_{y}";
                        control = RNGenerator.Instance.FindControl(RNGenerator.Instance, innerText);


                        switch (classIdx)
                        {
                            case 0:
                                if (keyCount < partsKeys.Count && !String.IsNullOrEmpty(partsKeys[keyCount]))
                                {
                                    control.Text = partsKeys[keyCount];
                                }
                                else
                                {
                                    isExitFlag = true; // 마지막 키에 도달하면 플래그 설정
                                }
                                break;

                            case 1:
                                if (keyCount < letterKeys.Count && !String.IsNullOrEmpty(letterKeys[keyCount]))
                                {
                                    control.Text = letterKeys[keyCount];
                                }
                                else
                                {
                                    isExitFlag = true; // 마지막 키에 도달하면 플래그 설정
                                }
                                break;

                            case 2:
                                if (keyCount < wordKeys.Count && !String.IsNullOrEmpty(wordKeys[keyCount]))
                                {
                                    control.Text = wordKeys[keyCount];
                                }
                                else
                                {
                                    isExitFlag = true; // 마지막 키에 도달하면 플래그 설정
                                }
                                break;
                        }

                        if (isExitFlag) // 플래그가 설정되면 x 값 반복문 종료
                        {
                            break;
                        }

                        keyCount++; // 키 카운트 증가
                    }
                    if (isExitFlag) // 플래그가 설정되면 x 값 반복문 종료
                    {
                        break;
                    }
                }
            }
            MessageBox.Show("자동 할당이 완료되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //------------------------------------------------------------------------------------
    }

}
