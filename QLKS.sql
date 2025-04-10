﻿CREATE DATABASE QLKS
go
USE QLKS
go
-- 1. Bảng phân quyền nhân viên
CREATE TABLE VaiTro (
    vai_tro_id INT IDENTITY(1,1) PRIMARY KEY,
    ten_vai_tro VARCHAR(50) NOT NULL UNIQUE
);
go
-- 2. Bảng loại phòng
CREATE TABLE LoaiPhong (
    loai_phong_id INT IDENTITY(1,1) PRIMARY KEY,
    ten_loai VARCHAR(50) NOT NULL,
    mo_ta TEXT,
    gia_theo_dem DECIMAL(10, 2) NOT NULL
);
go
-- 3. Bảng phòng
CREATE TABLE Phong (
    phong_id INT IDENTITY(1,1) PRIMARY KEY,
    so_phong VARCHAR(10) NOT NULL UNIQUE,
    loai_phong_id INT NOT NULL,
    trang_thai VARCHAR(20)
);
go
-- 4. Bảng khách hàng
CREATE TABLE KhachHang (
    khach_hang_id INT IDENTITY(1,1) PRIMARY KEY,
    ho_ten VARCHAR(100) NOT NULL,
    dia_chi TEXT,
    so_dien_thoai VARCHAR(15),
    email VARCHAR(100),
    cccd VARCHAR(20) NOT NULL UNIQUE
);
go
-- 5. Bảng dịch vụ
CREATE TABLE DichVu (
    dich_vu_id INT IDENTITY(1,1) PRIMARY KEY,
    ten_dich_vu VARCHAR(100) NOT NULL,
    mo_ta TEXT,
    gia DECIMAL(10, 2) NOT NULL
);
go
-- 6. Bảng nhân viên
CREATE TABLE NhanVien (
    nhan_vien_id INT IDENTITY(1,1) PRIMARY KEY,
    ho_ten VARCHAR(100) NOT NULL,
	sdt VARCHAR(10) NOT NULL,
	chuc_vu VARCHAR(50) NOT NULL,
    ca_lam_viec VARCHAR(50),
    luong DECIMAL(10, 2),
	tai_khoan VARCHAR(20),
	mat_khau VARCHAR(20),
	vai_tro_id INT
);
go
-- 7. Bảng đặt phòng
CREATE TABLE DatPhong (
    dat_phong_id INT IDENTITY(1,1) PRIMARY KEY,
    khach_hang_id INT NOT NULL,
    phong_id INT NOT NULL,
    nhan_vien_id INT NOT NULL,
    ngay_check_in DATE NOT NULL,
    ngay_check_out DATE NOT NULL,
    ngay_dat DATETIME DEFAULT CURRENT_TIMESTAMP,
    trang_thai VARCHAR(20)
);
go
-- 8. Bảng dịch vụ đi kèm đặt phòng
CREATE TABLE DichVuDatPhong (
    dich_vu_dat_phong_id INT IDENTITY(1,1) PRIMARY KEY,
    dat_phong_id INT NOT NULL,
    dich_vu_id INT NOT NULL,
    so_luong INT DEFAULT 1,
    ngay_su_dung DATE NOT NULL
);
go
-- 9. Bảng hóa đơn
CREATE TABLE HoaDon (
    hoa_don_id INT IDENTITY(1,1) PRIMARY KEY,
    dat_phong_id INT NOT NULL,
    nhan_vien_id INT NOT NULL,
    ngay_tao DATETIME DEFAULT CURRENT_TIMESTAMP,
    tong_tien DECIMAL(10, 2) NOT NULL
);
go
-- 10. Bảng thanh toán
CREATE TABLE ThanhToan (
    thanh_toan_id INT IDENTITY(1,1) PRIMARY KEY,
    hoa_don_id INT NOT NULL,
    so_tien DECIMAL(10, 2) NOT NULL,
    phuong_thuc VARCHAR(10) NOT NULL,
    ngay_thanh_toan DATETIME DEFAULT CURRENT_TIMESTAMP
);
go
INSERT INTO VaiTro (ten_vai_tro) VALUES
('ADMIN'),     -- 1
('Quản Lý Khách Sạn'),     -- 2
('Nhân Viên Lễ Tân'),     -- 3
('Nhân Viên Buồng Phòng'),-- 4
('Nhân Viên Phục Vụ'),    -- 5
('Nhân Viên Bảo Vệ'),     -- 6
('Nhân Viên Kế Toán'),    -- 7
('Nhân Viên Kỹ Thuật');   -- 8
go
INSERT INTO LoaiPhong (ten_loai, mo_ta, gia_theo_dem) VALUES
('VIPP', N'Phòng rộng 30m2, view biển', 1500000),
('VIP', N'Phòng 50m2, có bồn tắm', 2500000),
('Thuong', N'Phòng tiêu chuẩn 20m2', 800000);
go
INSERT INTO Phong (so_phong, loai_phong_id, trang_thai) VALUES
('P101', 1, 'trong'),
('P102', 1, 'dang_su_dung'),
('P201', 2, 'trong'),
('P202', 3, 'bao_tri');
go
INSERT INTO KhachHang (ho_ten, dia_chi, so_dien_thoai, email, cccd) VALUES
(N'Nguyễn Văn A', N'Hà Nội', '0912345678', 'a.nguyen@gmail.com', '123456789'),
(N'Trần Thị B', N'TP.HCM', '0987654321', 'b.tran@gmail.com', '987654321'),
(N'Lê Văn C', N'Đà Nẵng', '0901122334', NULL, '1122334455');
go
INSERT INTO DichVu (ten_dich_vu, mo_ta, gia) VALUES
(N'Ăn sáng', N'Buffet sáng', 150000),
(N'Giặt là', N'Giặt ủi quần áo', 100000),
(N'Spa', N'Massage thư giãn', 500000);
go
INSERT INTO NhanVien (ho_ten, sdt, chuc_vu, ca_lam_viec, luong, tai_khoan, mat_khau, vai_tro_id) VALUES
('Nguyễn Văn Quản', '0912345678', 'Quản Lý Khách Sạn', 'Giờ hành chính', 25000000, 'quanly', '123456', 2),
('Trần Thị Lễ', '0923456789', 'Nhân Viên Lễ Tân', 'Ca sáng', 12000000, 'letan1', '123456', 3),
('Lê Văn Tân', '0934567890', 'Nhân Viên Lễ Tân', 'Ca chiều', 12000000, 'letan2', '123456', 3),
('Phạm Thị Phòng', '0945678901', 'Nhân Viên Buồng Phòng', 'Ca sáng', 10000000, 'buongphong', '123456', 4),
('Đỗ Văn Phục', '0956789012', 'Nhân Viên Phục Vụ', 'Ca sáng', 9000000, 'phucvu', '123456', 5),
('Vũ Văn Vệ', '0967890123', 'Nhân Viên Bảo Vệ', 'Ca đêm', 8500000, 'baove', '123456', 6),
('Bùi Thị Toán', '0978901234', 'Nhân Viên Kế Toán', 'Giờ hành chính', 11000000, 'ketoan', '123456', 7),
('Quản trị viên', '00000000', 'ADMIN', '24/24', 0, 'admin', '123', 1);


go
INSERT INTO DatPhong (khach_hang_id, phong_id, nhan_vien_id, ngay_check_in, ngay_check_out, trang_thai) VALUES
(1, 1, 2, '2024-10-20', '2024-10-23', 'da_xac_nhan'),
(2, 3, 2, '2025-1-25', '2025-1-27', 'da_huy');
go
INSERT INTO DichVuDatPhong (dat_phong_id, dich_vu_id, so_luong, ngay_su_dung) VALUES
(1, 1, 2, '2024-10-21'), -- 2 suất ăn sáng
(1, 3, 1, '2024-10-22'); -- 1 lần spa
go
INSERT INTO HoaDon (dat_phong_id, nhan_vien_id, tong_tien) VALUES
(1, 3, 0);
go
INSERT INTO ThanhToan (hoa_don_id, so_tien, phuong_thuc) VALUES
(1, 5650000, 'tien_mat');
