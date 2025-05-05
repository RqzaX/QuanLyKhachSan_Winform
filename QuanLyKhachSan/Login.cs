using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.AcceptButton = btnDangNhap;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //textBox2.UseSystemPasswordChar = true;
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
            pictureBox3.BackColor = Color.Gainsboro;
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            pictureBox3.BackColor = Color.White;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string userName = txtTaiKhoan.Text.Trim();
            string passWord = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Bạn chưa nhập Tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTaiKhoan.Focus();
                return;
            }
            if (string.IsNullOrEmpty(passWord))
            {
                MessageBox.Show("Bạn chưa nhập Mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                using (var db = new QLKSDataContext())
                {
                    var nhanVien = (from x in db.NhanViens
                              join vt in db.VaiTros on x.vai_tro_id equals vt.vai_tro_id
                              where x.tai_khoan == userName
                              select new
                              {
                                  Entity = x,
                                  TenChucVu = vt.ten_vai_tro
                              }).SingleOrDefault();

                    if (nhanVien == null) { /* lỗi */ }

                    // Tìm user theo tài khoản
                    var nv = db.NhanViens
                               .SingleOrDefault(x => x.tai_khoan == userName);

                    if (nv == null)
                    {
                        MessageBox.Show("Tài khoản không tồn tại.", "Lỗi Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtTaiKhoan.Focus();
                        return;
                    }

                    // So khớp mật khẩu
                    if (nv.mat_khau != passWord)
                    {
                        MessageBox.Show("Mật khẩu không đúng.", "Lỗi Đăng nhập",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Clear();
                        txtPassword.Focus();
                        return;
                    }
                    InfoNhanVien.CurrentUser = nv;
                    this.Hide();
                    var main = new TrangChu(nhanVien.Entity.ho_ten, nhanVien.TenChucVu);
                    main.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llbQuenMatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Liên hệ quản trị viên hoặc quản lý để đổi lại mật khẩu !","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
