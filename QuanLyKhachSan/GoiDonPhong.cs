﻿using System;
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
    public partial class GoiDonPhong : Form
    {
        private QLKSDataContext db = new QLKSDataContext();
        public GoiDonPhong()
        {
            InitializeComponent();
            dgvPhong.AutoGenerateColumns = false;

            dgvPhong.Columns.Add("so_phong", "Số phòng");
            dgvPhong.Columns.Add("ten_loai", "Tên loại phòng");
            dgvPhong.Columns.Add("trang_thai", "Trạng thái");

            dgvPhong.Columns["so_phong"].DataPropertyName = "so_phong";
            dgvPhong.Columns["ten_loai"].DataPropertyName = "ten_loai";
            dgvPhong.Columns["trang_thai"].DataPropertyName = "trang_thai";
            LoadDataToDataGridView();
            dgvPhong.RowTemplate.Height = 35;
        }
        private void AddButtonColumn()
        {
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "Thao tác";
            btnDelete.Text = "Dọn";
            btnDelete.UseColumnTextForButtonValue = true;
            dgvPhong.Columns.Add(btnDelete);
        }
        private void LoadDataToDataGridView()
        {
            try
            {
                var query = from p in db.Phongs
                            join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                            select new
                            {
                                p.so_phong,
                                lp.ten_loai,
                                TrangThai = p.trang_thai == "trong" ? "Trống"
                                 : p.trang_thai == "dang_su_dung" ? "Đang sử dụng"
                                 : p.trang_thai == "bao_tri" ? "Bảo trì"
                                 : p.trang_thai == "da_dat" ? "Đã đặt"
                                 : p.trang_thai,
                            };

                dgvPhong.DataSource = query.ToList();
                AddButtonColumn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(dgvPhong.SelectedCells[2].Value?.ToString());
        }
    }
}
