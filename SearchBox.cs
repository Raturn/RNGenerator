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
        private Form _targetForm;
        private int _currentMatchIndex = 0;
        private string _lastKeyword = "";

        public SearchBox(Form targetForm)
        {
            InitializeComponent();
            _targetForm = targetForm;

            searchTextBox.KeyDown += SearchTextBox_KeyDown;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                DoSearch();
            }
        }


        private List<(TextBox tb, int index)> _matchedPositions = new List<(TextBox, int)>();

        private void DoSearch()
        {
            string keyword = searchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
                return;

            if (_lastKeyword != keyword)
            {
                _currentMatchIndex = 0;
                _lastKeyword = keyword;
                _matchedPositions.Clear();

                // 텍스트박스 + 인덱스 전체 수집
                CollectMatchingPositions(_targetForm, keyword, _matchedPositions);
            }

            if (_matchedPositions.Count == 0)
                return;

            if (_currentMatchIndex >= _matchedPositions.Count)
                _currentMatchIndex = 0;

            var (target, index) = _matchedPositions[_currentMatchIndex];
            target.Focus();
            target.Select();
            target.SelectionStart = index;
            target.SelectionLength = keyword.Length;

            // 내부 스크롤 이동
            target.ScrollToCaret(); // TextBox 내부 커서 위치까지 스크롤
            _targetForm.ScrollControlIntoView(target); // Form 자체 스크롤도 이동

            _currentMatchIndex++;
        }


        private void CollectMatchingPositions(Control parent, string keyword, List<(TextBox, int)> list)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox tb)
                {
                    string name = tb.Name.ToLower();

                    if (!name.Contains("metrix") && !name.Contains("key"))
                    {
                        string text = tb.Text;
                        int start = 0;

                        while (start < text.Length)
                        {
                            int idx = text.IndexOf(keyword, start, StringComparison.OrdinalIgnoreCase);
                            if (idx == -1)
                                break;

                            list.Add((tb, idx));
                            start = idx + keyword.Length; // 다음 검색 위치로 이동
                        }
                    }
                }

                if (control.HasChildren)
                    CollectMatchingPositions(control, keyword, list);
            }
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

                if (control.HasChildren)
                    CollectMatchingTextBoxes(control, keyword, list);
            }
        }
    }
}
