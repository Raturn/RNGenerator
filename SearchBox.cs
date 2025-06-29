using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RNGenerator
{
    public partial class SearchBox : Form
    {
        private RNGenerator _mainForm;
        private int _currentMatchIndex = 0;
        private string _lastKeyword = "";


        string[] tableClassArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" }; // 테이블 클래스 이름 배열


        public SearchBox(RNGenerator mainform)
        {
            InitializeComponent();
            _mainForm = mainform;

            searchTextBox.KeyDown += SearchTextBox_KeyDown;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        // Enter 키 처리
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // '띡' 소리 제거
                DoSearch();
            }
        }


        private void DoSearch()
        {
            string keyword = searchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
                return;

            if (_lastKeyword != keyword)
            {
                _currentMatchIndex = 0;
                _lastKeyword = keyword;
            }

            // 검색 가능한 TextBox 컨트롤 목록 수집
            List<TextBox> matchedTextBoxes = new List<TextBox>();
            CollectMatchingTextBoxes(_mainForm, keyword, matchedTextBoxes);

            if (matchedTextBoxes.Count == 0)
                return;

            if (_currentMatchIndex >= matchedTextBoxes.Count)
                _currentMatchIndex = 0;

            // 현재 일치 항목 포커스
            TextBox target = matchedTextBoxes[_currentMatchIndex];
            target.Focus();
            target.Select();
            target.SelectionStart = target.Text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
            target.SelectionLength = keyword.Length;

            _currentMatchIndex++;
        }



        private void CollectMatchingTextBoxes(Control parent, string keyword, List<TextBox> list)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox tb)
                {
                    string name = tb.Name.ToLower();
                    if (!name.Contains("metrix") && !name.Contains("key") &&
                        tb.Text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        list.Add(tb);
                    }
                }

                // 자식 컨트롤 재귀 탐색
                if (control.HasChildren)
                {
                    CollectMatchingTextBoxes(control, keyword, list);
                }
            }
        }
        //private void DoSearch()
        //{
        //    string keyword = searchTextBox.Text.Trim();

        //    if (string.IsNullOrEmpty(keyword))
        //        return;

        //    // 새 키워드 입력 시 인덱스 초기화
        //    if (_lastKeyword != keyword)
        //    {
        //        _currentMatchIndex = 0;
        //        _lastKeyword = keyword;
        //    }

        //    _mainForm.SearchAndFocus(keyword, _currentMatchIndex);

        //    _currentMatchIndex++;

        //    if (_currentMatchIndex >= _mainForm.GetMatchCount(keyword))
        //    {
        //        _currentMatchIndex = 0; // 순환
        //    }
        //}
    }
}
