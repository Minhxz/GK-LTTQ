using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Test2
{
    public partial class Form1 : Form
    {
        Classes.DataProcesssor dtBase = new Classes.DataProcesssor();
        Classes.Function ft = new Classes.Function();
        private object maSPDangChon;
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            DataTable dtHV = dtBase.DocBang("Select * from tblHocVien");
            dgvDMHV.DataSource = dtHV;

            dgvDMHV.Columns[0].HeaderText = "Mã HV ";
            dgvDMHV.Columns[1].HeaderText = "Tên HV ";
            dgvDMHV.Columns[2].HeaderText = "SĐT ";
            dgvDMHV.Columns[3].HeaderText = "Giới tính ";
            dgvDMHV.Columns[4].HeaderText = "Quê ";
            dgvDMHV.Columns[5].HeaderText = "Ghi chú ";

            dgvDMHV.Columns[0].Width = 150;
            dgvDMHV.Columns[1].Width = 150;
            dgvDMHV.Columns[2].Width = 150;
            dgvDMHV.Columns[3].Width = 150;
            dgvDMHV.Columns[4].Width = 150;
            dgvDMHV.Columns[5].Width = 200;

            dgvDMHV.BackgroundColor = Color.AliceBlue;

        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn có chắc chắn muốn thoat?", "Xac nhan thoat", MessageBoxButtons.YesNo) == DialogResult.Yes) Application.Exit();
        }

        private void txtSDT_Enter(object sender, EventArgs e)
        {
            
        }

        private void txtSDT_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSDT.Text, out _))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại hợp lệ!");
                txtSDT.Text = string.Empty;
                txtSDT.Focus();
            }
        }

        private void dgvDMHV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex<0 || dgvDMHV.Rows[e.RowIndex].IsNewRow)
            {
                return;
            }
            DataGridViewRow row = dgvDMHV.Rows[e.RowIndex];
            maSPDangChon = row.Cells[0].Value;

            txtMaHV.Text = Convert.ToString(row.Cells[0].Value);
            txtTenHV.Text = Convert.ToString(row.Cells[1].Value);
            string gioiTinh = Convert.ToString(row.Cells[3].Value).Trim();
            rbNam.Checked = gioiTinh == "Nam";
            rbNu.Checked = gioiTinh == "Nữ";
            txtSDT.Text = Convert.ToString(row.Cells[2].Value);
            cboQueQuan.Text = Convert.ToString(row.Cells[4].Value);
            txtGhiChu.Text = Convert.ToString(row.Cells[5].Value);


            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThem.Enabled = false;

           

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHV.Text))
            {
                MessageBox.Show("Bạn chưa nhập mã học viên!");
                txtMaHV.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenHV.Text))
            {
                MessageBox.Show("Bạn chưa nhập tên học viên!");
                txtTenHV.Focus();
                return;
            }

            if (!rbNam.Checked && !rbNu.Checked)
            {
                MessageBox.Show("Bạn chưa chọn giới tính!");
                rbNam.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cboQueQuan.Text))
            {
                MessageBox.Show("Bạn chưa chọn quê quán!");
                cboQueQuan.Focus();
                return;
            }

            string maHV = txtMaHV.Text.Trim();
            string gioiTinh = rbNam.Checked ? "Nam" : "Nữ";

            try
            {
                DataTable dt = dtBase.DocBang("Select MaHV from tblHocVien");
                foreach (DataRow row in dt.Rows)
                {
                    if (string.Equals(Convert.ToString(row["MaHV"]).Trim(), maHV, StringComparison.OrdinalIgnoreCase)){
                        MessageBox.Show("Hoc vien da ton tai! hay nhan ma khac!");
                        txtMaHV.Focus();
                        txtMaHV.SelectAll();
                        return;
                       }
                    }
                string sql = @"Insert into tblHocVien (MaHV,TenHV,SoDT,GioiTinh,Que,GhiChu) 
                                    Values(@MaHV, @TenHV, @SoDT, @GioiTinh, @Que, @GhiChu)";
                dtBase.CapNhatDuLieu(sql,
                        new SqlParameter("@MaHV", maHV),
                        new SqlParameter("@TenHV", txtTenHV.Text.Trim()),
                        new SqlParameter("@SoDT", txtSDT.Text.Trim()),
                        new SqlParameter("@GioiTinh", gioiTinh),
                        new SqlParameter("@Que", cboQueQuan.Text.Trim()),
                        new SqlParameter("@GhiChu", txtGhiChu.Text.Trim()));

                dgvDMHV.DataSource = dtBase.DocBang("Select * from tblHocVien");
                MessageBox.Show("Them hoc vien thanh cong!");
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }



            
            


        }

        private void rbNam_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!rbNam.Checked && !rbNu.Checked)
            {
                MessageBox.Show("Bạn chưa chọn giới tính!");
                return;
            }

            string gioiTinh = rbNam.Checked ? "Nam" : "Nữ";
        }
    }
}
