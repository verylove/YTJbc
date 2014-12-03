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
    public partial class FormRooms : Form
    {
        DbEntities _db = new DbEntities();
        Dictionary<string, string> _dictRoomLevel = new Dictionary<string, string>();
        DataTable _dtRoomLevel = null;

        SocketClient _socket = null;

        const string _RoomPointFile = "point.txt";

        #region 私有方法
        private void InitCmbRoomLevel()
        {
            foreach (var item in _db.RoomLevels)
            {
                _dictRoomLevel[item.Name] = item.Price.ToString();
            }
            cmbRoomLevel.Items.Clear();
            foreach (var item in _dictRoomLevel)
            {
                cmbRoomLevel.Items.Add(item.Key);
            }
            cmbRoomLevel.SelectedIndex = 0;
        }

        private void InitDataGridView()
        {
            _dtRoomLevel = new DataTable();
            _dtRoomLevel.Columns.Add("RoomId", typeof(string));
            _dtRoomLevel.Columns.Add("RoomLevel", typeof(string));
            _dtRoomLevel.Columns.Add("RoomPrice", typeof(string));
            _dtRoomLevel.Columns.Add("RoomPoint", typeof(string));

            foreach (var room in _db.Rooms)
            {
                DataRow dr = _dtRoomLevel.NewRow();
                var roomLv = _db.RoomLevels.Where(x => x.Id == room.RoomLvId).FirstOrDefault();
                if (roomLv == null) continue;
                dr["RoomId"] = room.Code;
                dr["RoomLevel"] = roomLv == null ? "" : roomLv.Name;
                dr["RoomPrice"] = roomLv == null ? "" : roomLv.Price.ToString();
                dr["RoomPoint"] = (int)roomLv.Price / 2 / 3;
                _dtRoomLevel.Rows.Add(dr);
            }

            dgvPlayer.DataSource = _dtRoomLevel;
            dgvPlayer.Refresh();
        }

        /// <summary>
        /// 生成房间积分配置文件
        /// </summary>
        private void MakeRoomPointFile()
        {
            try
            {
                var strRoomPointFilePath = Application.StartupPath + "\\" + _RoomPointFile;
                if (File.Exists(strRoomPointFilePath))
                {
                    File.Delete(strRoomPointFilePath);
                }

                StringBuilder sb = new StringBuilder();
                foreach (var room in _db.Rooms)
                {
                    sb.Append(room.Code + "=" + room.Point + "\r\n");
                }

                var roomPointFile = File.CreateText(strRoomPointFilePath);
                roomPointFile.Write(sb.ToString());
                roomPointFile.Flush();
                roomPointFile.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public FormRooms(SocketClient socket)
        {
            _socket = socket;
            InitializeComponent();
            InitCmbRoomLevel();
            InitDataGridView();
        }

        #region 事件
        private void FormRooms_Load(object sender, EventArgs e)
        {
        }

        private void cmbRoomLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int price = int.Parse(_dictRoomLevel[cmbRoomLevel.SelectedItem.ToString()]);
            int point = price / 2 / 3;

            txtPoint.Text = point.ToString();
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            string strRoomId = txtRoomId.Text;
            string strRoomLv = cmbRoomLevel.Text;
            string strRoomPoint = txtPoint.Text;
            try
            {

                var roomLv = _db.RoomLevels.Where(x => x.Name == strRoomLv).FirstOrDefault();
                if (roomLv == null)
                {
                    throw new Exception("未找到房间等级信息!");
                }

                var room = _db.Rooms.Where(x => x.Code == strRoomId).FirstOrDefault();
                if (room == null)
                {
                    room = new Room();

                    room.RoomLvId = roomLv.Id;
                    room.Code = strRoomId;
                    room.Price = roomLv.Price;
                    room.Point = int.Parse(strRoomPoint);
                    _db.Rooms.Add(room);

                }
                else
                {
                    room.RoomLvId = roomLv.Id;
                    room.Code = strRoomId;
                    room.Price = roomLv.Price;
                    room.Point = int.Parse(strRoomPoint);
                    _db.Entry(room).State = EntityState.Modified;
                }
                _db.SaveChanges();

                MessageBox.Show("保存成功!\n(注意:房间设置需要上传到服务器才能在游戏中生效)", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("保存失败!原因:" + ex.Message, "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            InitDataGridView();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            FormRoomLevel frm = new FormRoomLevel();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string strRoomId = txtRoomId.Text;
            if (string.IsNullOrEmpty(strRoomId))
            {
                MessageBox.Show("请选择要删除的房间\n或者输入要删除的房间号!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var room = _db.Rooms.Where(x => x.Code == strRoomId).FirstOrDefault();
                if (room == null)
                {
                    MessageBox.Show("删除失败!原因:房间号不存在.", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult dr = MessageBox.Show("确认要删除房间[" + strRoomId + "]的信息?", "剪包锤游戏管理程序", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        _db.Rooms.Remove(room);
                        _db.SaveChanges();
                        MessageBox.Show("删除成功!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InitDataGridView();
                    }

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("删除失败!原因:" + ex.Message, "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvPlayer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRow = dgvPlayer.SelectedRows[0];
            string roomId = selectedRow.Cells["ColumnID"].Value.ToString();
            if (!string.IsNullOrEmpty(roomId))
            {
                var room = _db.Rooms.Where(x => x.Code == roomId).FirstOrDefault();
                if (room != null)
                {
                    txtRoomId.Text = room.Code;
                    txtPoint.Text = room.Point.ToString();
                    var roomLv = _db.RoomLevels.Where(x => x.Id == room.RoomLvId).FirstOrDefault();
                    if (roomLv != null)
                    {
                        cmbRoomLevel.SelectedItem = roomLv.Name;
                    }
                }
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            var strRoomPointFilePath = Application.StartupPath + "\\" + _RoomPointFile;
            MakeRoomPointFile();
            if (File.Exists(strRoomPointFilePath))
            {
                try
                {
                    //dgvPlayer.Height -= progressBar1.Height + 10;
                    groupBox1.Height -= progressBar1.Height + 10;
                    progressBar1.Visible = true;
                    //lblProcess.Visible = true;
                    Application.DoEvents();

                    int result = GameUtil.UploadRequest(Properties.Settings.Default.UploadUrl, strRoomPointFilePath, _RoomPointFile, 3, progressBar1);
                    if (result == 1)
                    {
                        MessageBox.Show(this, "上传房间积分配置成功!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //发送服务器更新配置指令
                        _socket.Send(GameCommand.UpdateSetting);
                    }
                    else
                    {
                        MessageBox.Show(this, "上传房间积分配置失败!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "上传房间积分配置失败!原因:" + ex.Message, "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    progressBar1.Visible = false;
                    groupBox1.Height += progressBar1.Height + 10;
                    //dgvPlayer.Height += progressBar1.Height + 10;
                    Application.DoEvents();

                    if (File.Exists(strRoomPointFilePath))
                    {
                        File.Delete(strRoomPointFilePath);
                    }
                    //lblProcess.Visible = false;
                }
            }
        }
        #endregion

    }
}
