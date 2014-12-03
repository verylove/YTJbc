using FakedGameClient.ServerController.Database;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FakedGameClient.ServerController
{
    public partial class FormRoomLevel : Form
    {
        DbEntities _db = new DbEntities();
        public FormRoomLevel()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddLevel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("请输入房费!", "剪包锤游戏管理程序");
                txtPrice.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtLevel.Text))
            {
                MessageBox.Show("请输入房间等级!", "剪包锤游戏管理程序");
                txtLevel.Focus();
                return;
            }

            ListViewItem lvi = null;

            foreach (ListViewItem item in lv.Items)
            {
                if (item.SubItems[1].Text == txtLevel.Text)
                {
                    lvi = item;
                    break;
                }
            }

            if (lvi == null)
            {
                lvi = new ListViewItem();
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lv.Items.Add(lvi);
            }

            lvi.SubItems[0].Text = txtPrice.Text;
            lvi.SubItems[1].Text = txtLevel.Text;

            try
            {
                var tmp = _db.RoomLevels.Where(x => x.Name == txtLevel.Text).FirstOrDefault();
                if (tmp == null)
                {
                    _db.RoomLevels.Add(new RoomLevel()
                    {
                        Code = txtLevel.Text,
                        Name = txtLevel.Text,
                        Price = decimal.Parse(txtPrice.Text)
                    }
                    );
                }
                else
                {
                    tmp.Name = txtLevel.Text;
                    tmp.Code = txtLevel.Text;
                    tmp.Price = decimal.Parse(txtPrice.Text);
                    _db.Entry(tmp).State = EntityState.Modified;
                }
                _db.SaveChanges();
                MessageBox.Show(this, "保存成功!", "剪包锤游戏管理程序");

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, "保存失败!", "剪包锤游戏管理程序");
            }
        }

        private void FormRoomLevel_Load(object sender, EventArgs e)
        {
            lv.Items.Clear();
            foreach (var entity in _db.RoomLevels)
            {
                lv.Items.Add(new ListViewItem(new string[] { entity.Price.ToString(), entity.Name }));
            }
        }

        private void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv.SelectedItems.Count > 0)
            {
                txtPrice.Text = lv.SelectedItems[0].SubItems[0].Text;
                txtLevel.Text = lv.SelectedItems[0].SubItems[1].Text;
            }
        }
    }
}
