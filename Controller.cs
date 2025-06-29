using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RNGenerator
{
    public partial class Controller : Form
    {
        public Controller()
        {
            InitializeComponent();
            Instance = this;
        }


        public static Controller Instance;  // 다른 클래스에서 접근할 수 있도록 static 변수

        string[] tableClassArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" }; // 테이블 클래스 이름 배열


        private float currentZoom = 1.0f;
        private readonly float minZoom = 0.5f;
        private readonly float maxZoom = 2.0f;


        //---------------------------------------------------------- 버튼 이벤트 ----------------------------------------------------------
        //------------------------ 불러오기 버튼 ------------------------
        private void metrixLoad_Click(object sender, EventArgs e)
        {
            try
            { // 파일 불러오기
                //DialogResult result = MessageBox.Show("현재 작성된 내용들을 덮어 쓰시겠습니까?", "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                //if (result == DialogResult.OK)
                //{
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text Files (*.txt)|*.txt";
                openFileDialog.Title = "텍스트 파일 선택";
                openFileDialog.InitialDirectory = @"C:\CA연구용역\격판";    // 기본 폴더 설정

                // 파일 선택되었을 때
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 파일 경로
                    string filePath = openFileDialog.FileName;

                    // 파일명(확장자 제외) 추출 후 변수에 저장
                    metrixName.Text = Path.GetFileNameWithoutExtension(filePath);

                    // 파일 내용 읽기
                    string fileContent = File.ReadAllText(filePath);

                    // tempInput_chk 텍스트박스에 내용 할당
                    string[] eachTableArray = fileContent.Split("@@@");

                    for (int tableNum = 0; tableNum < eachTableArray.Length; tableNum++)
                    {
                        string className = tableClassArray[tableNum];   // A ~ O 까지의 테이블 번호
                        string tableContent = eachTableArray[tableNum]; // A ~ O 까지의 테이블 내용

                        // 변치와 내부치를 구분 >> 인덱스 0은 변치, 인덱스 1은 내부치
                        string[] outerInner = tableContent.Split("+구분값+"); // +구분값+ 으로 나누기

                        // 변치를 각각 할당
                        if (!outerInner[0].Equals("|||||"))     // 비어있다면 구분자의 집합 |||||를 출력함
                        {
                            string[] outerValues = outerInner[0].Split('|'); // |로 나누기
                            int cnt = 0; // 카운트 변수 초기화


                            if (String.IsNullOrEmpty(outerValues[0]))     // 비어있다면 동작 안 함
                            {
                                string[] eachElementArray = outerValues[0].Split(',');
                                cnt = 1;

                                foreach (string eachElement in eachElementArray)
                                {
                                    Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{className}metrixX1_{cnt}");
                                    if (ctrl != null)
                                    {
                                        TextBox eachTB = ctrl as TextBox;  // 다운캐스팅으로 오류 최소화

                                        if (!eachElement.Equals("^")) // 비어있는 값은 ^로 표시되기 때문에 아닐 때만 동작
                                        {
                                            eachTB.Text = eachElement; // 텍스트박스에 값 할당
                                        }
                                    }
                                    cnt++;
                                }
                            }


                            if (!String.IsNullOrEmpty(outerValues[1]))     // 비어있다면 동작 안 함
                            {
                                string[] eachElementArray = outerValues[1].Split(',');
                                cnt = 1;

                                foreach (string eachElement in eachElementArray)
                                {
                                    Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{className}keyX{cnt}");
                                    if (ctrl != null)
                                    {
                                        TextBox eachTB = ctrl as TextBox;  // 다운캐스팅으로 오류 최소화

                                        if (!eachElement.Equals("^")) // 비어있는 값은 ^로 표시되기 때문에 아닐 때만 동작
                                        {
                                            eachTB.Text = eachElement; // 텍스트박스에 값 할당
                                        }
                                    }
                                    cnt++;
                                }
                            }


                            if (!String.IsNullOrEmpty(outerValues[2]))     // 비어있다면 동작 안 함
                            {
                                string[] eachElementArray = outerValues[2].Split(',');
                                cnt = 1;

                                foreach (string eachElement in eachElementArray)
                                {
                                    Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{className}metrixX2_{cnt}");
                                    if (ctrl != null)
                                    {
                                        TextBox eachTB = ctrl as TextBox;  // 다운캐스팅으로 오류 최소화

                                        if (!eachElement.Equals("^")) // 비어있는 값은 ^로 표시되기 때문에 아닐 때만 동작
                                        {
                                            eachTB.Text = eachElement; // 텍스트박스에 값 할당
                                        }
                                    }
                                    cnt++;
                                }
                            }


                            if (!String.IsNullOrEmpty(outerValues[3]))     // 비어있다면 동작 안 함
                            {
                                string[] eachElementArray = outerValues[3].Split(',');
                                cnt = 1;

                                foreach (string eachElement in eachElementArray)
                                {
                                    Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{className}metrixY1_{cnt}");
                                    if (ctrl != null)
                                    {
                                        TextBox eachTB = ctrl as TextBox;  // 다운캐스팅으로 오류 최소화

                                        if (!eachElement.Equals("^")) // 비어있는 값은 ^로 표시되기 때문에 아닐 때만 동작
                                        {
                                            eachTB.Text = eachElement; // 텍스트박스에 값 할당
                                        }
                                    }
                                    cnt++;
                                }
                            }


                            if (!String.IsNullOrEmpty(outerValues[4]))     // 비어있다면 동작 안 함
                            {
                                string[] eachElementArray = outerValues[4].Split(',');
                                cnt = 1;

                                foreach (string eachElement in eachElementArray)
                                {
                                    Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{className}keyY{cnt}");
                                    if (ctrl != null)
                                    {
                                        TextBox eachTB = ctrl as TextBox;  // 다운캐스팅으로 오류 최소화

                                        if (!eachElement.Equals("^")) // 비어있는 값은 ^로 표시되기 때문에 아닐 때만 동작
                                        {
                                            eachTB.Text = eachElement; // 텍스트박스에 값 할당
                                        }
                                    }
                                    cnt++;
                                }
                            }


                            if (!String.IsNullOrEmpty(outerValues[5]))     // 비어있다면 동작 안 함
                            {
                                string[] eachElementArray = outerValues[5].Split(',');
                                cnt = 1;

                                foreach (string eachElement in eachElementArray)
                                {
                                    Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{className}metrixY2_{cnt}");
                                    if (ctrl != null)
                                    {
                                        TextBox eachTB = ctrl as TextBox;  // 다운캐스팅으로 오류 최소화

                                        if (!eachElement.Equals("^")) // 비어있는 값은 ^로 표시되기 때문에 아닐 때만 동작
                                        {
                                            eachTB.Text = eachElement; // 텍스트박스에 값 할당
                                        }
                                    }
                                    cnt++;
                                }
                            }
                        }

                        // 내부치를 할당
                        if (!outerInner[1].Equals("|||||||||"))     // 비어있다면 구분자의 집합 |||||||||를 출력함
                        {
                            string[] innerValues = outerInner[1].Split('|'); // |로 나누기

                            for (int x = 0; x < 10; x++)
                            {
                                string[] eachElementArray = innerValues[x].Split(",");
                                int cnt = 1; // 카운트 변수 초기화

                                foreach (string eachElement in eachElementArray)
                                {
                                    string ctrlKey = $"{className}var{x + 1}_{cnt}"; // 컨트롤 이름
                                    Control ctrl_in = RNGenerator.Instance.FindControl(RNGenerator.Instance, ctrlKey);

                                    if (ctrl_in is System.Windows.Forms.TextBox textBox)
                                    {
                                        if (!eachElement.Equals("^")) // 비어있는 값은 ^로 표시되기 때문에 아닐 때만 동작
                                        {
                                            ctrl_in.Text = eachElement; // 텍스트박스에 값 할당
                                        }
                                    }

                                    cnt++; // 다음 내부치로 이동
                                }
                            }
                        }
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일 불러오기 중 오류 발생 :\n올바른 데이터 형식이 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //---------------------------------------------------------------



        //------------------------ 전체 체크 / 체크 해제 버튼 ------------------------
        private void allSelect_Click(object sender, EventArgs e)
        {
            // 하나라도 체크 안 돼있으면 전체 체크로 간주
            bool shouldCheck = tableClassArray.Any(name =>
            {
                var chk = RNGenerator.Instance.FindControl(this, "chk" + name) as CheckBox;
                return chk != null && !chk.Checked;
            });

            foreach (string name in tableClassArray)
            {
                var chk = RNGenerator.Instance.FindControl(this, "chk" + name) as CheckBox;
                if (chk != null)
                    chk.Checked = shouldCheck;
            }
        }
        //-----------------------------------------------------------------------------



        //------------------------ 삭제 버튼 >> 체크된 항목만 삭제 ------------------------
        private void delBtn_Click(object sender, EventArgs e)
        {
            var outerChkBox = RNGenerator.Instance.FindControl(this, "chkOuter") as CheckBox;
            var innerChkBox = RNGenerator.Instance.FindControl(this, "chkInner") as CheckBox;


            foreach (string name in tableClassArray)
            {
                var chk = RNGenerator.Instance.FindControl(this, "chk" + name) as CheckBox;
                if (chk != null && chk.Checked)
                {
                    if (outerChkBox != null && outerChkBox.Checked)     // 변치가 체크되어 있을 경우
                    {
                        for (int outNum = 1; outNum <= 10; outNum++)
                        {
                            Control ctrl1 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}keyX{outNum}");
                            Control ctrl2 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}keyY{outNum}");
                            Control ctrl3 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}metrixX1_{outNum}");
                            Control ctrl4 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}metrixX2_{outNum}");
                            Control ctrl5 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}metrixY1_{outNum}");
                            Control ctrl6 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}metrixY2_{outNum}");

                            // 컨트롤들을 배열로 묶어서 처리
                            Control[] controls = new Control[] { ctrl1, ctrl2, ctrl3, ctrl4, ctrl5, ctrl6 };

                            foreach (Control ctrl in controls)
                            {
                                if (ctrl is System.Windows.Forms.TextBox textBox)
                                {
                                    textBox.Text = ""; // 텍스트박스 비우기
                                }
                            }
                        }
                    }

                    if (innerChkBox != null && innerChkBox.Checked)     // 내부치가 체크되어 있을 경우
                    {
                        for (int x = 1; x <= 10; x++)
                        {
                            for (int y = 1; y <= 10; y++)
                            {
                                Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}var{x}_{y}");

                                if (ctrl is System.Windows.Forms.TextBox textBox)
                                {
                                    ctrl.Text = ""; // 텍스트박스 비우기
                                }
                            }
                        }
                    }
                }
            }
        }
        //---------------------------------------------------------------------------


        //------------------------ 저장 버튼 ------------------------
        private void metrixSave_Click(object sender, EventArgs e)
        {
            string basePath = @"C:\CA연구용역\격판";
            string fileName = metrixName.Text.Trim();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("파일명을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullPath = Path.Combine(basePath, fileName + ".txt");
            string content = ""; // result() 함수가 string 반환한다고 가정



            foreach (string name in tableClassArray)
            {
                // .txt 파일 내용으로 작성할 변치 할당
                for (int outNum1 = 0; outNum1 < 10; outNum1++) 
                {
                    Control ctrl1 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}metrixX1_{outNum1 + 1}");

                    if (ctrl1 is System.Windows.Forms.TextBox textBox)
                    {
                        if (String.IsNullOrEmpty(ctrl1.Text))
                        {
                            content += "^"; // 비어있는 값은 ^로 표시
                        }
                        else
                        {
                            content += ctrl1.Text;
                        }

                        if (outNum1 < 9) // 마지막 내부치가 아닌 경우에만 구분자 추가
                        {
                            content += ","; // 내부치 구분자
                        }
                    }
                }
                content += "|"; // 변치 구분자

                for (int outNum2 = 0; outNum2 < 10; outNum2++)
                {
                    Control ctrl2 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}keyX{outNum2 + 1}");

                    if (ctrl2 is System.Windows.Forms.TextBox textBox)
                    {
                        if (String.IsNullOrEmpty(ctrl2.Text))
                        {
                            content += "^"; // 비어있는 값은 ^로 표시
                        }
                        else
                        {
                            content += ctrl2.Text;
                        }

                        if (outNum2 < 9) // 마지막 내부치가 아닌 경우에만 구분자 추가
                        {
                            content += ","; // 내부치 구분자
                        }
                    }
                }
                content += "|"; // 변치 구분자

                for (int outNum3 = 0; outNum3 < 10; outNum3++)
                {
                    Control ctrl3 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}metrixX2_{outNum3 + 1}");

                    if (ctrl3 is System.Windows.Forms.TextBox textBox)
                    {
                        if (String.IsNullOrEmpty(ctrl3.Text))
                        {
                            content += "^"; // 비어있는 값은 ^로 표시
                        }
                        else
                        {
                            content += ctrl3.Text;
                        }

                        if (outNum3 < 9) // 마지막 내부치가 아닌 경우에만 구분자 추가
                        {
                            content += ","; // 내부치 구분자
                        }
                    }
                }
                content += "|"; // 변치 구분자

                for (int outNum4 = 0; outNum4 < 10; outNum4++)
                {
                    Control ctrl4 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}metrixY1_{outNum4 + 1}");

                    if (ctrl4 is System.Windows.Forms.TextBox textBox)
                    {
                        if (String.IsNullOrEmpty(ctrl4.Text))
                        {
                            content += "^"; // 비어있는 값은 ^로 표시
                        }
                        else
                        {
                            content += ctrl4.Text;
                        }

                        if (outNum4 < 9) // 마지막 내부치가 아닌 경우에만 구분자 추가
                        {
                            content += ","; // 내부치 구분자
                        }
                    }
                }
                content += "|"; // 변치 구분자

                for (int outNum5 = 0; outNum5 < 10; outNum5++)
                {
                    Control ctrl5 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}keyY{outNum5 + 1}");

                    if (ctrl5 is System.Windows.Forms.TextBox textBox)
                    {
                        if (String.IsNullOrEmpty(ctrl5.Text))
                        {
                            content += "^"; // 비어있는 값은 ^로 표시
                        }
                        else
                        {
                            content += ctrl5.Text;
                        }

                        if (outNum5 < 9) // 마지막 내부치가 아닌 경우에만 구분자 추가
                        {
                            content += ","; // 내부치 구분자
                        }
                    }
                }
                content += "|"; // 변치 구분자

                for (int outNum6 = 0; outNum6 < 10; outNum6++)
                {
                    Control ctrl6 = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}metrixY2_{outNum6 + 1}");

                    if (ctrl6 is System.Windows.Forms.TextBox textBox)
                    {
                        if (String.IsNullOrEmpty(ctrl6.Text))
                        {
                            content += "^"; // 비어있는 값은 ^로 표시
                        }
                        else
                        {
                            content += ctrl6.Text;
                        }

                        if (outNum6 < 9) // 마지막 내부치가 아닌 경우에만 구분자 추가
                        {
                            content += ","; // 내부치 구분자
                        }
                    }
                }
                content += "+구분값+"; // 변치,내부치 구분자


                // .txt 파일 내용으로 작성할 내부치 할당
                for (int x = 1; x <= 10; x++)
                {
                    for (int y = 1; y <= 10; y++)
                    {
                        Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, $"{name}var{x}_{y}");

                        if (ctrl is System.Windows.Forms.TextBox textBox)
                        {
                            if (String.IsNullOrEmpty(ctrl.Text))
                            {
                                content += "^"; // 비어있는 값은 ^로 표시
                            }
                            else
                            {
                                content += ctrl.Text;
                            }

                            if (y < 10) // 마지막 내부치가 아닌 경우에만 구분자 추가
                            {
                                content += ","; // 내부치 구분자
                            }
                        }
                    }

                    if (x < 10) // 마지막 내부치가 아닌 경우에만 구분자 추가
                    {
                        content += "|"; // 내부치 구분자
                    }
                }
                if (name != tableClassArray.Last()) // 마지막 테이블이 아닌 경우에만 구분자 추가
                {
                    content += "@@@"; // 테이블 구분자
                }
            }


            try
            {
                // 디렉토리 없으면 생성
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                // 파일이 존재하는 경우
                if (File.Exists(fullPath))
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

                // 파일 저장 (덮어쓰기 또는 새로 생성)
                File.WriteAllText(fullPath, content, Encoding.UTF8);
                MessageBox.Show($"파일이 저장되었습니다:\n{fullPath}", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"파일 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //-----------------------------------------------------------


        //------------------------ 새 메트릭스 생성 버튼 ------------------------
        private void newMetrix_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tableClassArray.Length; i++) // 조기 종료 조건
            {
                for (int x = 1; x <= 10; x++)
                {
                    for (int y = 1; y <= 10; y++)
                    {
                        string ctrlKey = $"{tableClassArray[i]}var{x}_{y}"; // 컨트롤 이름
                        Control ctrl = RNGenerator.Instance.FindControl(RNGenerator.Instance, ctrlKey);

                        if (ctrl is System.Windows.Forms.TextBox textBox)
                        {
                            ctrl.Text = ""; // 텍스트박스 비우기
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}
