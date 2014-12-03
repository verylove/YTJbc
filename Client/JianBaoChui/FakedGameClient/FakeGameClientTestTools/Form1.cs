using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FakeGameClientTestTools
{
    public partial class FormMain : Form
    {
        string[] _lines;
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            _lines = null;
            _lines = txtContent.Lines;
            //统计出所有玩家
            HashSet<string> players = new HashSet<string>();
            foreach (string line in _lines)
            {
                if (!line.Contains("玩家ID["))
                {
                    continue;
                }

                int index = line.IndexOf("玩家ID[");
                string playerId = line.Substring(index, line.IndexOf("]", index) - index + 1);
                players.Add(playerId);
            }

            lstPlayer.Items.Clear();
            //汇总每个玩家的信息
            foreach (var playerId in players)
            {
                lstPlayer.Items.Add(playerId);
            }

        }

        private void lstPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string playerId = lstPlayer.SelectedItem.ToString();
            StringBuilder sb = new StringBuilder();

            foreach (var line in _lines)
            {
                if (line.Contains(playerId))
                {
                    sb.AppendLine(line);
                }
            }
            txtContent.Text = sb.ToString();
            sb.Clear();
        }
    }
}
