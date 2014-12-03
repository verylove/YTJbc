using FakedGameClient.ServerController.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FakedGameClient.ServerController
{
    public partial class FormRoomsPoint : Form
    {
        DbEntities _db = new DbEntities();
        DataTable _dtRoomLevel = null;

        #region 私有方法
        private void InitCmbRoomLevel()
        {
            cmbRoomLevel.Items.Clear();
            foreach (var lv in _db.RoomLevels.ToList())
            {
                cmbRoomLevel.Items.Add(lv.Name);
            }
            cmbRoomLevel.Items.Insert(0, "全部");
            cmbRoomLevel.SelectedIndex = 0;
        }

        private void InitDataGridView()
        {
            _dtRoomLevel = new DataTable();
            _dtRoomLevel.Columns.Add("RoomId", typeof(string));
            _dtRoomLevel.Columns.Add("RoomLevel", typeof(string));
            _dtRoomLevel.Columns.Add("RoomPrice", typeof(string));
            _dtRoomLevel.Columns.Add("RoomPoint", typeof(string));
            _dtRoomLevel.Columns.Add("RoomCurPoint", typeof(string));

            foreach (var room in _db.Rooms)
            {
                DataRow dr = _dtRoomLevel.NewRow();
                var roomLv = _db.RoomLevels.Where(x => x.Id == room.RoomLvId).FirstOrDefault();
                if (roomLv == null) continue;
                dr["RoomId"] = room.Code;
                dr["RoomLevel"] = roomLv == null ? "" : roomLv.Name;
                dr["RoomPrice"] = roomLv == null ? "" : roomLv.Price.ToString();
                dr["RoomPoint"] = (int)roomLv.Price / 2 / 3;
                dr["RoomCurPoint"] = room.CurPoint;
                _dtRoomLevel.Rows.Add(dr);
            }

            dgvPlayer.DataSource = _dtRoomLevel;
            dgvPlayer.Refresh();
        }

        /// <summary>
        /// 上传房间积分配置
        /// </summary>
        private void UploadRooms()
        {

        }

        public void DataToExcel(DataGridView m_DataView)
        {
            SaveFileDialog kk = new SaveFileDialog();
            kk.Title = "保存EXECL文件";
            kk.Filter = "EXECL文件(*.xls) |*.xls |所有文件(*.*) |*.*";
            kk.FilterIndex = 1;
            if (kk.ShowDialog() == DialogResult.OK)
            {
                string FileName = kk.FileName;
                if (File.Exists(FileName))
                    File.Delete(FileName);
                FileStream objFileStream;
                StreamWriter objStreamWriter;
                string strLine = "";
                objFileStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
                objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);
                for (int i = 0; i < m_DataView.Columns.Count; i++)
                {
                    if (m_DataView.Columns[i].Visible == true)
                    {
                        strLine = strLine + m_DataView.Columns[i].HeaderText.ToString() + Convert.ToChar(9);
                    }
                }
                objStreamWriter.WriteLine(strLine);
                strLine = "";
                for (int i = 0; i < m_DataView.Rows.Count; i++)
                {
                    if (m_DataView.Columns[0].Visible == true)
                    {
                        if (m_DataView.Rows[i].Cells[0].Value == null)
                            strLine = strLine + " " + Convert.ToChar(9);
                        else
                            strLine = strLine + m_DataView.Rows[i].Cells[0].Value.ToString() + Convert.ToChar(9);
                    }
                    for (int j = 1; j < m_DataView.Columns.Count; j++)
                    {
                        if (m_DataView.Columns[j].Visible == true)
                        {
                            if (m_DataView.Rows[i].Cells[j].Value == null)
                                strLine = strLine + " " + Convert.ToChar(9);
                            else
                            {
                                string rowstr = "";
                                rowstr = m_DataView.Rows[i].Cells[j].Value.ToString();
                                if (rowstr.IndexOf("\r\n") > 0)
                                    rowstr = rowstr.Replace("\r\n", " ");
                                if (rowstr.IndexOf("\t") > 0)
                                    rowstr = rowstr.Replace("\t", " ");
                                strLine = strLine + rowstr + Convert.ToChar(9);
                            }
                        }
                    }
                    objStreamWriter.WriteLine(strLine);
                    strLine = "";
                }
                objStreamWriter.Close();
                objFileStream.Close();
                MessageBox.Show(this, "保存EXCEL成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        public FormRoomsPoint()
        {
            InitializeComponent();
            InitDataGridView();
            InitCmbRoomLevel();
        }

        #region 事件
        private void FormRooms_Load(object sender, EventArgs e)
        {
        }

        private void cmbRoomLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strLv = cmbRoomLevel.Text;
            if (strLv == "全部")
            {
                InitDataGridView();
            }
            else
            {
                var dv = _dtRoomLevel.DefaultView;
                dv.RowFilter = "RoomLevel='" + cmbRoomLevel.Text + "'";
            }
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {
            string strRoomId = txtRoomId.Text;
            if (string.IsNullOrEmpty(strRoomId))
            {
                return;
            }

            foreach (DataGridViewRow dgvr in dgvPlayer.Rows)
            {
                if (dgvr.Cells["ColumnID"].Value.ToString() == strRoomId)
                {
                    dgvr.Selected = true;
                    dgvPlayer.CurrentCell = dgvr.Cells[0];
                    break;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            InitDataGridView();
        }
        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataToExcel(dgvPlayer);
        }
    }
}
