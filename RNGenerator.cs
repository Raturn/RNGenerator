using System;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RNGenerator
{
    public partial class RNGenerator : Form
    {
        public static RNGenerator Instance;  // 다른 클래스에서 접근할 수 있도록 static 변수


        public RNGenerator()
        {
            InitializeComponent();
            Instance = this;

            this.KeyPreview = true;
        }


        string[] tableClassArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" }; // 테이블 클래스 이름 배열


        private float currentZoom = 1.0f;
        private readonly float minZoom = 0.5f;
        private readonly float maxZoom = 2.0f;


        private List<System.Windows.Forms.TextBox> matchedTextBoxes = new List<System.Windows.Forms.TextBox>();



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



        public int GetMatchCount(string keyword)
        {
            return this.Controls
                .OfType<System.Windows.Forms.TextBox>()
                .Count(tb => tb.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
        //---------------------------------------------------------------------



        //------------------------------- 컨트롤 탐색 -------------------------------
        public Control FindControl(Control _parent, string _name)     // 재귀 탐색 >> 선택한 항목의 자식들을 탐색하여 내가 원하는 요소를 매칭
        {
            foreach (Control control in _parent.Controls)
            {
                if (control.Name == _name)
                    return control;

                Control found = FindControl(control, _name);
                if (found != null)
                    return found;
            }


            //MessageBox.Show($"[오류] '{_name}' 컨트롤을 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);


            return null;
        }
        //---------------------------------------------------------------------------



        //-------------------- 위치값 추출 --------------------
        // 우선 2수조만 진행
        private string positionValue(string tableClass, string xVal, string yVal)
        {
            string position = FindControl(this, $"{tableClass}keyX{xVal}").Text + FindControl(this, $"{tableClass}keyY{yVal}").Text;

            return position;
        }
        //-----------------------------------------------------

        //-------------------- 내부치 매칭 --------------------
        public Dictionary<string, string> MatchedData(Dictionary<string, string> _dic)
        {
            bool isMatched = false; // 해당 key에 대한 매칭 여부
            List<string> keys = _dic.Keys.ToList(); // 키 목록 복사


            foreach (string key in keys)
            {
                isMatched = false;

                for (int i = 0; i < tableClassArray.Length && !isMatched; i++) // 조기 종료 조건
                {
                    for (int x = 1; x <= 10 && !isMatched; x++)
                    {
                        for (int y = 1; y <= 10 && !isMatched; y++)
                        {
                            string ctrlKey = $"{tableClassArray[i]}var{x}_{y}"; // 컨트롤 이름
                            Control ctrl = FindControl(this, ctrlKey);

                            if (ctrl is System.Windows.Forms.TextBox textBox)
                            {
                                // 여기서 비교 기준이 무엇이냐에 따라 달라짐
                                // Text 값이 현재 key와 같다면? 또는 단순히 비어있지 않다면?
                                if (textBox.Text == key) // 정확한 매칭 시만 등록
                                {
                                    _dic[key] = positionValue(tableClassArray[i], x.ToString(), y.ToString());
                                    isMatched = true; // 더 이상 반복하지 않게
                                }
                            }
                        }
                    }
                }
            }

            // 값 확인용 출력
            //string aaa = string.Join(Environment.NewLine, _dic.Select(kv => $"{kv.Key}: {kv.Value}"));
            //MessageBox.Show(aaa);

            return _dic;
        }
        //-----------------------------------------------------


        //---------------------------------------------------------- 버튼 이벤트 동작 ----------------------------------------------------------
        private void Controller_Btn_Click(object sender, EventArgs e)
        {
            Controller controller = new Controller(); // 새 폼 생성
            controller.Name = "Controller";           // 폼 이름 설정

            Form existingForm = Application.OpenForms["Controller"];
            if (existingForm != null)
            {
                existingForm.BringToFront();  // 화면 앞으로 가져옴
                existingForm.Activate();      // 포커스 줌
            }
            else
            {

                controller.Show();                // 비모달로 열기
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Scenario scenario = new Scenario();
            scenario.Name = "Scenario";

            Form existingForm = Application.OpenForms["Scenario"];
            if (existingForm != null)
            {
                existingForm.BringToFront();
                existingForm.Activate();
            }
            else
            {
                scenario.Show();
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}

