using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bai1
{
    public partial class Form1 : Form
    {
        private List<KhachHang> danhSachKhachHang;
        public Form1()
        {
            InitializeComponent();
            danhSachKhachHang = new List<KhachHang>();
        }

        private void button2_Click(object sender, EventArgs e)
            //Nut thoat
        {
            // Hiển thị hộp thoại xác nhận thoát
            var result = MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Thoát chương trình
                Application.Exit();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            

            // Kiểm tra các điều kiện
            if (txtMa.Text.Length != 6 || string.IsNullOrEmpty(txtTen.Text) ||
                string.IsNullOrEmpty(txtDC.Text) || !DateTime.TryParse(txtNgay.Text, out _) ||
                string.IsNullOrEmpty(txtTrc.Text) || string.IsNullOrEmpty(txtNay.Text) ||
                int.Parse(txtTrc.Text) >= int.Parse(txtNay.Text))
            {
                MessageBox.Show("Vui lòng nhập lại thông tin!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Tạo khách hàng mới
            KhachHang khachHang = new KhachHang
            {
                MaKH = txtMa.Text,
                HoTen = txtTen.Text,
                DiaChi = txtDC.Text,
                NgayChotSo = DateTime.Parse(txtNgay.Text),
                SoThangTruoc = int.Parse(txtTrc.Text),
                SoThangNay = int.Parse(txtNay.Text),
                SoTienDien = int.Parse(TinhSoTienDien.Text),
            };

            // Thêm vào danh sách khách hàng
            danhSachKhachHang.Add(khachHang);

            MessageBox.Show("Thêm khách hàng thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

     /*   private void button5_Click(object sender, EventArgs e)
        {
            // Đặt các textbox thành rỗng
            txtMa.Text = "";
            txtTen.Text = "";
            txtDC.Text = "";
            txtNgay.Text = "";
            txtTrc.Text = "";
            txtNay.Text = "";

            // Focus vào textbox Mã KH
            txtMa.Focus();

        }*/

        /*int soThangTruoc = int.Parse(txtTrc.Text);
        int soThangNay = int.Parse(txtNay.Text);
        int soTienDien = TinhSoTienDien(soThangTruoc, soThangNay);
*/
        
}
}
