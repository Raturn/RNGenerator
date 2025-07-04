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
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;


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

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyPreview = true;
        }


        private void ScenarioInput_TextChanged(object sender, EventArgs e)
        {
            lengthChange.Text = scenarioInput.Text.Length.ToString();
        }


        private static int globalSeatCnt = 0;

        private static string partsTemp = "";     // 자모 입력 창 값 저장
        private static string letterTemp = "";    // 글자 입력 창 값 저장
        private static string wordTemp = "";      // 단어 입력 창 값 저장
        private static string selfTemp = "";      // 수동 입력 창 값 저장


        private static string partsNumOutput = "";     // 자모 입력 및 난수 값
        private static string letterNumOutput = "";    // 글자 입력 및 난수 값
        private static string wordNumOutput = "";      // 단어 입력 및 난수 값
        private static string selfNumOutput = "";      // 수동 입력 및 난수 값

        private static Dictionary<string, string> matchedData = new Dictionary<string, string>();       // 격판의 매칭된 데이터만을 저장 >> 결과값이 아닌 답지

        private static string sentenceData1 = "";       // 결과파일 해독문
        private static string sentenceData2 = "";
        private static string sentenceData3 = "";
        private static string sentenceData4 = "";

        string[] tableClassArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" };  // 테이블 클래스 이름 배열

        private List<System.Windows.Forms.TextBox> matchedTextBoxes = new List<System.Windows.Forms.TextBox>();                 // 검색로직에서 사용할 리스트

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

        private static readonly char[] Initials = {     // 초성
        'ㄱ','ㄲ','ㄴ','ㄷ','ㄸ','ㄹ','ㅁ','ㅂ','ㅃ','ㅅ','ㅆ','ㅇ','ㅈ','ㅉ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ'
        };

        private static readonly char[] Medials = {      // 중성
        'ㅏ','ㅐ','ㅑ','ㅒ','ㅓ','ㅔ','ㅕ','ㅖ','ㅗ','ㅘ','ㅙ','ㅚ',
        'ㅛ','ㅜ','ㅝ','ㅞ','ㅟ','ㅠ','ㅡ','ㅢ','ㅣ'
        };

        private static readonly char[] Finals = {       // 종성
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



        //----------------------------- 검색 기능 -----------------------------
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                SearchBox searchForm = new SearchBox(this);  // 현재 폼 전달
                searchForm.Show(this);  // 검색 팝업 표시
            }
        }


        public void SearchAndFocus(string searchText, int matchIndex)
        {
            // 검색 결과 새로 갱신
            matchedTextBoxes = this.Controls.OfType<System.Windows.Forms.TextBox>()
                .Where(tb => tb.Text.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();

            if (matchedTextBoxes.Count == 0)
            {
                MessageBox.Show("일치하는 항목이 없습니다.");
                return;
            }

            if (matchIndex >= matchedTextBoxes.Count)
                matchIndex = 0;  // 순환 구조

            var tb = matchedTextBoxes[matchIndex];
            tb.Focus();
            tb.Select(tb.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase), searchText.Length);
            this.ScrollControlIntoView(tb);
        }
        //---------------------------------------------------------------------



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

                        //MessageBox.Show("파일을 성공적으로 불러왔습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


            sentenceData1 = "";
            sentenceData2 = "";
            sentenceData3 = "";


            globalSeatCnt = 0;          // 수조 초기화


            sentenceData1 = SplitParts(scenarioInput.Text);     // 해독문으로 세팅
            sentenceData2 = SplitLetter(scenarioInput.Text);
            sentenceData3 = SplitWord(scenarioInput.Text);

            partsInput.Text = InsertNewLine(sentenceData1);         // 입력값을 자모로 분리하여 할당
            letterInput.Text = InsertNewLine(sentenceData2);        // 입력값을 한 글자씩 분리하여 할당
            wordInput.Text = InsertNewLine(sentenceData3);          // 입력값을 단어로 분리하여 할당
        }
        //-----------------------------------------------------------------------------------------------------



        //------------------------------------- 몇 수조인지를 구하는 로직 -------------------------------------
        private int FindSeat()
        {
            int seatCnt = 0;

            foreach (string table in tableClassArray)
            {
                seatCnt = 0;

                string[] controlPatterns = new string[]
                {
                    "metrixX1_",
                    "keyX",
                    "metrixX2_",
                    "metrixY1_",
                    "keyY",
                    "metrixY2_"
                };

                foreach (string pattern in controlPatterns)
                {
                    for (int cnt = 1; cnt <= 10; cnt++)
                    {
                        string controlName = $"{table}{pattern}{cnt}";
                        Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, controlName);

                        if (ctrl != null && !string.IsNullOrWhiteSpace(ctrl.Text))
                        {
                            seatCnt++;
                            break; // 이 그룹에서 하나라도 값이 있으면 더 이상 안 봄
                        }
                    }
                }

                if (seatCnt > 1)
                    break; // 전체 루프 종료
            }

            globalSeatCnt = seatCnt;
            return seatCnt;
        }
        //-----------------------------------------------------------------------------------------------------



        //---------------------------------------------- 각 케이스별 변환 버튼 ----------------------------------------------
        //---------------------------------------------- 자모 난수 변환 버튼 ----------------------------------------------
        private void partsChange_Click(object sender, EventArgs e)
        {
            matchedData = RNGenerator.Instance.MatchedData(matchedData);

            string tempInput = partsInput.Text; // 기존 텍스트 저장
            tempInput = tempInput.Replace("\r", "").Replace("\n", "");  // 줄바꿈 제거


            if (tempInput.Contains("[") && tempInput.Contains("]"))
            {
                tempInput = RemoveOnlyRNum(tempInput); // 대괄호와 대괄호 안의 값 제거  >>  연속 변환 입력 시   사과[57][??] 과 같은 상황을 방지하기 위함
            }

            // 반복 호출하지 않고 캐싱
            int missingLength = FindSeat();
            StringBuilder newInput = new StringBuilder();

            for (int i = 0; i < tempInput.Length;)
            {
                bool matched = false;

                foreach (var item in matchedData.OrderByDescending(k => k.Key.Length))
                {
                    if (tempInput.Substring(i).StartsWith(item.Key))
                    {
                        string value = string.IsNullOrWhiteSpace(item.Value)
                            ? new string('?', missingLength)
                            : item.Value;

                        newInput.Append(item.Key).Append($"[{value}] ");
                        partsNumOutput += value + " ";
                        i += item.Key.Length;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    newInput.Append(tempInput[i]);
                    i++;
                }
            }

            tempInput = newInput.ToString();


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
            //MessageBox.Show("자모 난수 생성이 완료 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        //---------------------------------------------- 글자 난수 변환 버튼 ----------------------------------------------
        private void letterChange_Click(object sender, EventArgs e)
        {
            matchedData = RNGenerator.Instance.MatchedData(matchedData);

            string tempInput = letterInput.Text; // 기존 텍스트 저장
            tempInput = tempInput.Replace("\r", "").Replace("\n", "");  // 줄바꿈 제거


            if (tempInput.Contains("[") && tempInput.Contains("]"))
            {
                tempInput = RemoveOnlyRNum(tempInput); // 대괄호와 대괄호 안의 값 제거  >>  연속 변환 입력 시   사과[57][??] 과 같은 상황을 방지하기 위함
            }


            // 숫자값이 에러를 발생하는 건에 대한 수정사항
            // matchedData를 Key 길이 기준으로 내림차순 정렬
            var sortedMatched = matchedData
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                .OrderByDescending(kvp => kvp.Key.Length)
                .ToList();

            // 반복호출 하지 않고 캐싱
            int missingLength = FindSeat();

            StringBuilder newInput = new StringBuilder();
            int index = 0;

            while (index < tempInput.Length)
            {
                bool matched = false;

                foreach (var kvp in sortedMatched)
                {
                    string key = kvp.Key;
                    string value = string.IsNullOrWhiteSpace(kvp.Value)
                        ? new string('?', missingLength)  // ✅ 변경된 부분
                        : kvp.Value;

                    if (index + key.Length <= tempInput.Length &&
                        tempInput.Substring(index, key.Length) == key)
                    {
                        newInput.Append(key).Append($"[{value}] ");
                        index += key.Length;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    newInput.Append(tempInput[index]);
                    index++;
                }
            }

            tempInput = newInput.ToString();


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
            //MessageBox.Show("글자 난수 생성이 완료 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //---------------------------------------------- 단어 난수 변환 버튼 ----------------------------------------------
        private void wordChange_Click(object sender, EventArgs e)
        {
            matchedData = RNGenerator.Instance.MatchedData(matchedData);

            string tempInput = wordInput.Text;
            tempInput = tempInput.Replace("\r", "").Replace("\n", "");  // 줄바꿈 제거

            if (tempInput.Contains("[") && tempInput.Contains("]"))
            {
                tempInput = RemoveOnlyRNum(tempInput); // 대괄호 제거
            }

            wordNumOutput = ""; // 초기화

            // 숫자값이 에러를 발생하는 건에 대한 수정사항 (인덱스 기반 치환)
            StringBuilder newInput = new StringBuilder();

            // 반복 호출 대신 캐싱
            int missingLength = FindSeat();

            for (int i = 0; i < tempInput.Length;)
            {
                bool matched = false;

                foreach (var item in matchedData.OrderByDescending(k => k.Key.Length))
                {
                    if (!string.IsNullOrWhiteSpace(item.Key) &&
                        i + item.Key.Length <= tempInput.Length &&
                        tempInput.Substring(i, item.Key.Length) == item.Key)
                    {
                        // ✅ 빈 값이면 FindSeat 결과만큼 ? 반복
                        string value = string.IsNullOrWhiteSpace(item.Value)
                            ? new string('?', missingLength)
                            : item.Value;

                        newInput.Append(item.Key).Append($"[{value}] ");
                        wordNumOutput += value + " ";
                        i += item.Key.Length;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    newInput.Append(tempInput[i]);
                    i++;
                }
            }

            tempInput = newInput.ToString();

            // 입력값에서 공백, 줄바꿈 제거
            string cleanedInput = tempInput.Replace(" ", "").Replace("\r", "").Replace("\n", "");

            // 대괄호 안의 값 추출
            var matches = Regex.Matches(cleanedInput, @"\[(.*?)\]");

            string onlyBracketValues = "";
            foreach (Match match in matches)
            {
                onlyBracketValues += match.Groups[1].Value + " ";
            }

            wordNumOutput = onlyBracketValues.Trim(); // 대괄호 안 값 저장

            wordInput.Text = InsertNewLine(tempInput); // 줄바꿈 처리
            wordTemp = InsertNewLine(tempInput);       // 다른 함수에서 사용
                                                       // MessageBox.Show("단어 난수 생성이 완료 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        //---------------------------------------------- 수동 난수 변환 버튼 ----------------------------------------------
        private void selfChange_Click(object sender, EventArgs e)
        {
            // 띄어쓰기와 줄바꿈을 기준으로 입력값을 분리하여 배열로 저장
            string[] selfArray = selfInput.Text
                .Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            sentenceData4 = "";

            foreach (string item in selfArray)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    if (sentenceData4.Length > 0)
                        sentenceData4 += " ";

                    sentenceData4 += item;
                }
            }

            sentenceData4 = sentenceData4.TrimEnd(); // 마지막 띄어쓰기 제거

            // 배열의 각 요소를 Dictionary에 추가
            foreach (string token in selfArray)
            {
                if (!matchedData.ContainsKey(token))
                {
                    matchedData.Add(token, "");
                }
            }

            // 내부치와 매칭
            matchedData = RNGenerator.Instance.MatchedData(matchedData);

            var sortedData = matchedData
                .OrderByDescending(kv => kv.Key.Length)
                .ToList();

            string tempInput = selfInput.Text;
            tempInput = tempInput.Replace("\r", "").Replace("\n", "");  // 줄바꿈 제거

            if (tempInput.Contains("[") && tempInput.Contains("]"))
            {
                tempInput = RemoveOnlyRNum(tempInput); // 대괄호 제거
            }

            selfNumOutput = ""; // 난수값 초기화
            StringBuilder newInput = new StringBuilder();

            // ✅ FindSeat는 루프 외부에서 한 번만 호출
            int missingLength = FindSeat();

            for (int i = 0; i < tempInput.Length;)
            {
                bool matched = false;

                foreach (var item in sortedData)
                {
                    if (!string.IsNullOrWhiteSpace(item.Key) &&
                        i + item.Key.Length <= tempInput.Length &&
                        tempInput.Substring(i, item.Key.Length) == item.Key)
                    {
                        // ✅ 값이 비었을 경우, ?를 missingLength만큼 생성
                        string value = string.IsNullOrWhiteSpace(item.Value)
                            ? new string('?', missingLength)
                            : item.Value;

                        newInput.Append(item.Key).Append($"[{value}] ");
                        selfNumOutput += value + " ";
                        i += item.Key.Length;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    newInput.Append(tempInput[i]);
                    i++;
                }
            }

            tempInput = newInput.ToString();

            // 공백 제거 후 대괄호 안 값 추출
            string cleanedInput = tempInput.Replace(" ", "").Replace("\r", "").Replace("\n", "");
            var matches = Regex.Matches(cleanedInput, @"\[(.*?)\]");

            string onlyBracketValues = "";
            foreach (Match match in matches)
            {
                onlyBracketValues += match.Groups[1].Value + " ";
            }

            selfNumOutput = onlyBracketValues.Trim(); // 대괄호 안 숫자 저장

            selfInput.Text = InsertNewLine(tempInput); // 줄바꿈 처리
            selfTemp = InsertNewLine(tempInput);       // 다른 함수에서 사용

            MessageBox.Show("수동 난수 생성이 완료 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------- 저장 로직 --------------------------------------------------------------------------------------------------------
        // 자모 결과 저장 --------------------------------------------------------------------------------------------------------
        private void partsSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Controller.Instance.metrixName.Text))
                {
                    MessageBox.Show("격판명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (string.IsNullOrEmpty(scenarioName.Text))
                    {
                        MessageBox.Show("시나리오명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sentenceData1) || !string.IsNullOrWhiteSpace(sentenceData1))
                        {
                            string filePath = saveJsonOutput(partsNumOutput, sentenceData1);


                            if (filePath.Equals("빈값발생"))
                            {
                                MessageBox.Show("난수가 없거나 매칭되지 않은 값이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                //MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("입력된 문장이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("저장 중 오류가 발생되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 글자 결과 저장 --------------------------------------------------------------------------------------------------------
        private void letterSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Controller.Instance.metrixName.Text))
                {
                    MessageBox.Show("격판명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (string.IsNullOrEmpty(scenarioName.Text))
                    {
                        MessageBox.Show("시나리오명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sentenceData2) || !string.IsNullOrWhiteSpace(sentenceData2))
                        {
                            string filePath = saveJsonOutput(letterNumOutput, sentenceData2);

                            if (filePath.Equals("빈값발생"))
                            {
                                MessageBox.Show("난수가 없거나 매칭되지 않은 값이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("입력된 문장이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            catch
            {
                MessageBox.Show("저장 중 오류가 발생되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 단어 결과 저장 --------------------------------------------------------------------------------------------------------
        private void wordSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Controller.Instance.metrixName.Text))
                {
                    MessageBox.Show("격판명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (string.IsNullOrEmpty(scenarioName.Text))
                    {
                        MessageBox.Show("시나리오명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sentenceData3) || !string.IsNullOrWhiteSpace(sentenceData3))
                        {
                            string filePath = saveJsonOutput(wordNumOutput, sentenceData3);

                            if (filePath.Equals("빈값발생"))
                            {
                                MessageBox.Show("난수가 없거나 매칭되지 않은 값이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("입력된 문장이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("저장 중 오류가 발생되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 수동 결과 저장 --------------------------------------------------------------------------------------------------------
        private void selfSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Controller.Instance.metrixName.Text))
                {
                    MessageBox.Show("격판명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (string.IsNullOrEmpty(scenarioName.Text))
                    {
                        MessageBox.Show("시나리오명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sentenceData1) || !string.IsNullOrWhiteSpace(sentenceData1))
                        {
                            string filePath = saveJsonOutput(selfNumOutput, sentenceData4);

                            if (filePath.Equals("빈값발생"))
                            {
                                MessageBox.Show("난수가 없거나 매칭되지 않은 값이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show($"파일이 저장되었습니다:\n{filePath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("입력된 문장이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("저장 중 오류가 발생되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //전체 저장 로직 --------------------------------------------------------------------------------------------------------
        private void RNAllSaveBtn1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Controller.Instance.metrixName.Text))
            {
                MessageBox.Show("격판명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrEmpty(scenarioName.Text))
                {
                    MessageBox.Show("시나리오명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (!string.IsNullOrEmpty(sentenceData1) || !string.IsNullOrWhiteSpace(sentenceData1) || !string.IsNullOrEmpty(sentenceData2) || !string.IsNullOrWhiteSpace(sentenceData2) || !string.IsNullOrEmpty(sentenceData3) || !string.IsNullOrWhiteSpace(sentenceData3))
                    {
                        string filePath1 = saveJsonOutput(partsNumOutput, sentenceData1);
                        string filePath2 = saveJsonOutput(letterNumOutput, sentenceData2);
                        string filePath3 = saveJsonOutput(wordNumOutput, sentenceData3);

                        if (filePath1.Equals("빈값발생") || filePath2.Equals("빈값발생") || filePath3.Equals("빈값발생"))
                        {
                            MessageBox.Show("난수가 없거나 매칭되지 않은 값이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("입력된 문장이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RNAllSaveBtn2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Controller.Instance.metrixName.Text))
            {
                MessageBox.Show("격판명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrEmpty(scenarioName.Text))
                {
                    MessageBox.Show("시나리오명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (!string.IsNullOrEmpty(sentenceData1) || !string.IsNullOrWhiteSpace(sentenceData1) || !string.IsNullOrEmpty(sentenceData2) || !string.IsNullOrWhiteSpace(sentenceData2) || !string.IsNullOrEmpty(sentenceData3) || !string.IsNullOrWhiteSpace(sentenceData3))
                    {
                        string filePath1 = saveJsonOutput(partsNumOutput, sentenceData1);
                        string filePath2 = saveJsonOutput(letterNumOutput, sentenceData2);
                        string filePath3 = saveJsonOutput(wordNumOutput, sentenceData3);

                        if (filePath1.Equals("빈값발생") || filePath2.Equals("빈값발생") || filePath3.Equals("빈값발생"))
                        {
                            MessageBox.Show("난수가 없거나 매칭되지 않은 값이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("입력된 문장이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RNAllSaveBtn3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Controller.Instance.metrixName.Text))
            {
                MessageBox.Show("격판명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrEmpty(scenarioName.Text))
                {
                    MessageBox.Show("시나리오명이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (!string.IsNullOrEmpty(sentenceData1) || !string.IsNullOrWhiteSpace(sentenceData1) || !string.IsNullOrEmpty(sentenceData2) || !string.IsNullOrWhiteSpace(sentenceData2) || !string.IsNullOrEmpty(sentenceData3) || !string.IsNullOrWhiteSpace(sentenceData3))
                    {
                        string filePath1 = saveJsonOutput(partsNumOutput, sentenceData1);
                        string filePath2 = saveJsonOutput(letterNumOutput, sentenceData2);
                        string filePath3 = saveJsonOutput(wordNumOutput, sentenceData3);

                        if (filePath1.Equals("빈값발생") || filePath2.Equals("빈값발생") || filePath3.Equals("빈값발생"))
                        {
                            MessageBox.Show("난수가 없거나 매칭되지 않은 값이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show($"파일이 저장되었습니다:\n{filePath1}\n{filePath2}\n{filePath3}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("입력된 문장이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



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



        //---------------------------------- 버튼 클릭 이벤트의 필요에 의해 추가된 함수들 ----------------------------------
        // 입력값 On/Off 공통로직
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



        // 난수값만 On/Off 공통로직
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


        //---------------------------------------------- JSON 파일로 저장 하는 로직 ----------------------------------------------
        private string saveJsonOutput(string numOutput, string inputSentence)
        {
            int chunkSize = 400;
            var caList = new List<string>();

            // 난수 필드 구성
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

            // 구성된 난수 필드에 빈 값이 존재할 경우 또는 변환된 난수가 없을 경우 즉시 함수를 종료
            foreach (var item in caList)
            {
                if (item.Contains("??") || numOutput.Length == 0)
                {
                    return "빈값발생";
                }
            }


            // Metrix 구성
            var metrixWrappedList = new List<object>();

            for (int bundleIndex = 0; bundleIndex < 15; bundleIndex++)
            {
                var left = new List<List<string>>();
                var up = new List<List<string>>();

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

                    // 벡터를 left 또는 up에 추가
                    if (vectorIndex <= 2)
                        left.Add(vector);
                    else
                        up.Add(vector);
                }

                // 내부격자 구성
                var inner = new List<List<string>>();
                for (int row = 1; row <= 10; row++)
                {
                    var rowList = new List<string>();
                    for (int col = 1; col <= 10; col++)
                    {
                        string selectInnerTB = $"{tableClassArray[bundleIndex]}var{row}_{col}";
                        Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, selectInnerTB);
                        string value = ctrl is TextBox tb ? tb.Text : "";
                        rowList.Add(value);
                    }
                    inner.Add(rowList);
                }

                // JSON 개체 하나 구성
                string finalId = Path.GetFileNameWithoutExtension(Controller.Instance.metrixName.Text);     // 파일명(확장자명 없음)
                var oneMetrixObj = new
                {
                    최종격판ID = finalId,  // 여기서 finalId는 파일명 등에서 추출한 최종격판ID 문자열 변수입니다.
                    격판ID = bundleIndex,
                    외부치 = new
                    {
                        left = left,
                        up = up
                    },
                    내부치 = inner
                };

                metrixWrappedList.Add(oneMetrixObj);
            }

            // JSON 직렬화 옵션
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            // 메인 JSON 객체 (Metrix 제외)
            var jsonData = new
            {
                input = $"{string.Join(",", caList.SelectMany(ca => ca.Split(' ', StringSplitOptions.RemoveEmptyEntries)))}",
                target = $"격판ID={Controller.Instance.metrixName.Text}; 해독문=[{string.Join(",", inputSentence.Split(' ', StringSplitOptions.RemoveEmptyEntries))}]"
            };


            // 기본 저장 경로 설정
            string directoryPath = $@"C:\CA연구용역\생성된난수\{globalSeatCnt}수조\" + Controller.Instance.metrixName.Text;
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

            // 메인 JSON 저장
            string json = JsonSerializer.Serialize(jsonData, options);
            File.WriteAllText(filePath, json, Encoding.UTF8);

            // --- Metrix 별도 저장 로직 추가 ---

            string metrixDir = @"C:\CA연구용역\생성된난수\_격판형태";
            if (!Directory.Exists(metrixDir))
                Directory.CreateDirectory(metrixDir);

            //string metrixFileName = Controller.Instance.metrixName.Text + "_격판형태.jsonl"; // 확장자 변경
            string metrixFileName = Controller.Instance.metrixName.Text + ".jsonl"; // 확장자 변경
            string metrixFilePath = Path.Combine(metrixDir, metrixFileName);

            // JSON 직렬화 옵션 (반복 사용될 것이므로 동일하게 유지)
            var metrixOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            // JSONL 파일 쓰기 (한 줄에 하나씩)
            using (var writer = new StreamWriter(metrixFilePath, false, Encoding.UTF8))
            {
                foreach (var metrixObj in metrixWrappedList)
                {
                    string line = JsonSerializer.Serialize(metrixObj, metrixOptions);
                    writer.WriteLine(line);
                }
            }

            return filePath;
        }
        //------------------------------------------------------------------------------------------------------------------------



        //---------------------------------------- 대괄호와 대괄호 안의 값을 삭제하는 로직 ----------------------------------------
        static string RemoveOnlyRNum(string input)
        {
            // [대괄호 안 내용] 전체 삭제
            return Regex.Replace(input, @"\[[^\[\]]*?\]", "");
        }
        //-------------------------------------------------------------------------------------------------------------------------




        public void ScenarioWork(object sender, EventArgs e)
        {
            string baseFolder = @"C:\CA연구용역\시나리오";

            if (!Directory.Exists(baseFolder))
            {
                MessageBox.Show("시나리오 폴더가 존재하지 않습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] files = Directory.GetFiles(baseFolder);

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                scenarioName.Text = fileName;
                scenarioInput.Text = File.ReadAllText(filePath, Encoding.UTF8);

                // 시나리오 변경 처리
                scenarioChange_Click(null, null);

                // 부품 변경 처리
                partsChange_Click(null, null);

                // 부품 저장 처리
                partsSave_Click(null, null);

                // UI 업데이트 및 잠시 대기 (선택사항)
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(200);
            }

            //MessageBox.Show("모든 시나리오 파일 처리가 완료되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void scenarioComplete_Click(object sender, EventArgs e)
        {
            Controller.Instance.LoadMultipleMetrixFiles();
        }


        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
    }

}
