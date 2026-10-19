using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class frm : Form
    {
        Classes.DataProcessor dtBase = new Classes.DataProcessor();
        Classes.Function ft = new Classes.Function();
        private object maSPDangChon;
        public frm()
        {
            InitializeComponent();
        }

        private void frm_Load(object sender, EventArgs e)
        { 
            btnSua.Enabled = false;
            btnXoa.Enabled = false;



            // Load dữ liệu vào ComboBox NhaCungCap
            DataTable dtHang = dtBase.DocBang("Select * from tblSanPham");
            dgvDanhMuc.DataSource = dtHang;

            //dinh dang datagrid
            dgvDanhMuc.Columns[0].HeaderText = "Mã Hàng";
            dgvDanhMuc.Columns[1].HeaderText = "Tên Hàng";
            dgvDanhMuc.Columns[2].HeaderText = "Đơn vị tính";
            dgvDanhMuc.Columns[3].HeaderText = "Đơn giá";
            dgvDanhMuc.Columns[4].HeaderText = "Ghi Chú";
           

            dgvDanhMuc.Columns[0].Width = 150;
            dgvDanhMuc.Columns[1].Width = 250;
            dgvDanhMuc.Columns[2].Width = 150;
            dgvDanhMuc.Columns[3].Width = 150;
            dgvDanhMuc.Columns[4].Width = 150;

            dgvDanhMuc.BackgroundColor = Color.LightBlue;


        }

        private void cboDonViTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDanhMuc.Rows[e.RowIndex].IsNewRow)
                return;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnThem.Enabled = false;

            DataGridViewRow row = dgvDanhMuc.Rows[e.RowIndex];
            maSPDangChon = row.Cells[0].Value;
            // Thứ tự cột của tblSanPham: mã, tên, đơn vị tính, đơn giá, ghi chú.
            txtMaSP.Text = Convert.ToString(row.Cells[0].Value);
            txtTenSP.Text = Convert.ToString(row.Cells[1].Value);
            cboDonViTinh.Text = Convert.ToString(row.Cells[2].Value);
            txtDonGia.Text = Convert.ToString(row.Cells[3].Value);
            txtGhiChu.Text = Convert.ToString(row.Cells[4].Value);

        }

        void ResetValue()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            cboDonViTinh.Text = "";
            cboDonViTinh.SelectedIndex = -1;
            txtDonGia.Text = "";
            txtGhiChu.Text = "";
            maSPDangChon = null;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            int donGia;
            if (!int.TryParse(txtDonGia.Text, out donGia))
            {
                MessageBox.Show("Đơn giá phải là số nguyên!");
                txtDonGia.Focus();
                return;
            }

            // Lấy tên cột thật từ dữ liệu đã nạp, không dùng tiêu đề hiển thị.
            DataTable table = (DataTable)dgvDanhMuc.DataSource;
            string[] columns = table.Columns.Cast<DataColumn>()
                .Select(column => "[" + column.ColumnName.Replace("]", "]]") + "]")
                .ToArray();
            string sql = "UPDATE tblSanPham SET " + columns[0] + " = @MaSP, "
                + columns[1] + " = @TenSP, " + columns[2] + " = @DonViTinh, "
                + columns[3] + " = @DonGia, " + columns[4] + " = @GhiChu WHERE "
                + columns[0] + " = @MaSPCu";

            try
            {
                int affectedRows = dtBase.CapNhatDuLieu(sql,
                    new SqlParameter("@MaSP", txtMaSP.Text.Trim()),
                    new SqlParameter("@TenSP", txtTenSP.Text.Trim()),
                    new SqlParameter("@DonViTinh", cboDonViTinh.Text.Trim()),
                    new SqlParameter("@DonGia", SqlDbType.Int) { Value = donGia },
                    new SqlParameter("@GhiChu", txtGhiChu.Text.Trim()),
                    new SqlParameter("@MaSPCu", maSPDangChon));

                if (affectedRows == 0)
                {
                    MessageBox.Show("Sản phẩm đang chọn không còn tồn tại. Vui lòng tải lại dữ liệu.");
                    return;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Không thể cập nhật sản phẩm: " + ex.Message);
                return;
            }

            dgvDanhMuc.DataSource = dtBase.DocBang("Select * from tblSanPham");
            dgvDanhMuc.ClearSelection();
            ResetValue();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = "DELETE FROM tblSanPham WHERE [MaSP] = @MaSP";
                try
                {
                    int affectedRows = dtBase.CapNhatDuLieu(sql, new SqlParameter("@MaSP", maSPDangChon));
                    if (affectedRows == 0)
                    {
                        MessageBox.Show("Sản phẩm đang chọn không còn tồn tại. Vui lòng tải lại dữ liệu.");
                        return;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Không thể xóa sản phẩm: " + ex.Message);
                    return;
                }
                dgvDanhMuc.DataSource = dtBase.DocBang("Select * from tblSanPham");
                dgvDanhMuc.ClearSelection();
                ResetValue();
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThem.Enabled = true;


                
            }
        }
    }
}   
