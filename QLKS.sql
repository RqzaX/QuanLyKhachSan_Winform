CREATE DATABASE QLKS
go
USE QLKS
go
-- 1. Bảng phân quyền nhân viên
CREATE TABLE VaiTro (
    vai_tro_id INT IDENTITY(1,1) PRIMARY KEY,
    ten_vai_tro NVARCHAR(50) NOT NULL UNIQUE
);
go
-- 2. Bảng loại phòng
CREATE TABLE LoaiPhong (
    loai_phong_id INT IDENTITY(1,1) PRIMARY KEY,
    ten_loai NVARCHAR(50),
    mo_ta NVARCHAR(MAX),
    gia_theo_dem DECIMAL(10, 2) NOT NULL
);
go
-- 3. Bảng phòng
CREATE TABLE Phong (
    phong_id INT IDENTITY(1,1) PRIMARY KEY,
    so_phong NVARCHAR(10) NOT NULL UNIQUE,
    loai_phong_id INT NOT NULL,
    trang_thai NVARCHAR(20)
);
go
-- 4. Bảng khách hàng
CREATE TABLE KhachHang (
    khach_hang_id INT IDENTITY(1,1) PRIMARY KEY,
    ho_ten NVARCHAR(100) NOT NULL,
    dia_chi NVARCHAR(MAX),
    so_dien_thoai NVARCHAR(15),
    email NVARCHAR(100),
    cccd NVARCHAR(20) NOT NULL UNIQUE
);
go
-- 5. Bảng dịch vụ
CREATE TABLE DichVu (
    dich_vu_id INT IDENTITY(1,1) PRIMARY KEY,
    ten_dich_vu NVARCHAR(100),
    mo_ta NVARCHAR(MAX),
    gia DECIMAL(10, 2) NOT NULL
);
go
-- 6. Bảng nhân viên
CREATE TABLE NhanVien (
    nhan_vien_id INT IDENTITY(1,1) PRIMARY KEY,
    ho_ten NVARCHAR(100),
	sdt NVARCHAR(10),
	vai_tro_id INT,
    ca_lam_viec NVARCHAR(50),
    luong DECIMAL(10, 2),
	tai_khoan NVARCHAR(20),
	mat_khau NVARCHAR(20)
);
go
-- 7. Bảng đặt phòng
CREATE TABLE DatPhong (
    dat_phong_id INT IDENTITY(1,1) PRIMARY KEY,
    khach_hang_id INT NOT NULL,
    phong_id INT NOT NULL,
	khuyen_mai_id INT NULL,
    nhan_vien_id INT NOT NULL,
    ngay_check_in DATE NOT NULL,
    ngay_check_out DATE NOT NULL,
    ngay_dat DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    trang_thai NVARCHAR(20)
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
	ten_nhan_vien NVARCHAR(100),
    ngay_tao DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    tong_tien DECIMAL(10, 2) NOT NULL
);
-- select * from HoaDon
go
-- 10. Bảng thanh toán
CREATE TABLE ThanhToan (
    thanh_toan_id INT IDENTITY(1,1) PRIMARY KEY,
    hoa_don_id INT NOT NULL,
    so_tien DECIMAL(10, 2) NOT NULL,
    phuong_thuc NVARCHAR(10),
    ngay_thanh_toan DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE)
);
go
-- 11. Bảng chương trình khuyến mãi
CREATE TABLE KhuyenMai (
    khuyen_mai_id INT IDENTITY(1,1) PRIMARY KEY,
    ten_khuyen_mai NVARCHAR(50),
    phan_tram INT NOT NULL,
    ngay_bat_dau DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    ngay_ket_thuc DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE)
);
go
-- 1. Phòng ↔ Loại phòng
IF OBJECT_ID('FK_Phong_LoaiPhong','F') IS NOT NULL
  ALTER TABLE Phong DROP CONSTRAINT FK_Phong_LoaiPhong;
GO
ALTER TABLE Phong
  ADD CONSTRAINT FK_Phong_LoaiPhong
    FOREIGN KEY (loai_phong_id)
    REFERENCES LoaiPhong(loai_phong_id);
GO

-- 2. Nhân viên ↔ Vai trò
IF OBJECT_ID('FK_NhanVien_VaiTro','F') IS NOT NULL
  ALTER TABLE NhanVien DROP CONSTRAINT FK_NhanVien_VaiTro;
GO
ALTER TABLE NhanVien
  ADD CONSTRAINT FK_NhanVien_VaiTro
    FOREIGN KEY (vai_tro_id)
    REFERENCES VaiTro(vai_tro_id);
GO

-- 3. Đặt phòng ↔ Khách hàng, Phòng, Khuyến mãi, Nhân viên
--    Trong đó muốn xóa DatPhong khi xóa phòng, hoặc khi xóa khuyến mãi thì hủy booking
IF OBJECT_ID('FK_DatPhong_KhachHang','F')     IS NOT NULL ALTER TABLE DatPhong DROP CONSTRAINT FK_DatPhong_KhachHang;
IF OBJECT_ID('FK_DatPhong_Phong','F')         IS NOT NULL ALTER TABLE DatPhong DROP CONSTRAINT FK_DatPhong_Phong;
IF OBJECT_ID('FK_DatPhong_KhuyenMai','F')     IS NOT NULL ALTER TABLE DatPhong DROP CONSTRAINT FK_DatPhong_KhuyenMai;
IF OBJECT_ID('FK_DatPhong_NhanVien','F')      IS NOT NULL ALTER TABLE DatPhong DROP CONSTRAINT FK_DatPhong_NhanVien;
GO
ALTER TABLE DatPhong
  ADD CONSTRAINT FK_DatPhong_KhachHang
    FOREIGN KEY (khach_hang_id)
    REFERENCES KhachHang(khach_hang_id)
    ON DELETE NO ACTION,
   CONSTRAINT FK_DatPhong_Phong
    FOREIGN KEY (phong_id)
    REFERENCES Phong(phong_id)
    ON DELETE CASCADE,
   CONSTRAINT FK_DatPhong_KhuyenMai
    FOREIGN KEY (khuyen_mai_id)
    REFERENCES KhuyenMai(khuyen_mai_id)
    ON DELETE SET NULL,
   CONSTRAINT FK_DatPhong_NhanVien
    FOREIGN KEY (nhan_vien_id)
    REFERENCES NhanVien(nhan_vien_id)
    ON DELETE NO ACTION;
GO

-- 4. Dịch vụ đặt kèm ↔ Đặt phòng, Dịch vụ
--    Xóa DichVuDatPhong khi xóa DatPhong
IF OBJECT_ID('FK_DichVuDatPhong_DatPhong','F') IS NOT NULL ALTER TABLE DichVuDatPhong DROP CONSTRAINT FK_DichVuDatPhong_DatPhong;
IF OBJECT_ID('FK_DichVuDatPhong_DichVu','F')    IS NOT NULL ALTER TABLE DichVuDatPhong DROP CONSTRAINT FK_DichVuDatPhong_DichVu;
GO
ALTER TABLE DichVuDatPhong
  ADD CONSTRAINT FK_DichVuDatPhong_DatPhong
    FOREIGN KEY (dat_phong_id)
    REFERENCES DatPhong(dat_phong_id)
    ON DELETE CASCADE,
   CONSTRAINT FK_DichVuDatPhong_DichVu
    FOREIGN KEY (dich_vu_id)
    REFERENCES DichVu(dich_vu_id)
    ON DELETE NO ACTION;
GO

-- 5. Hóa đơn ↔ Đặt phòng, Nhân viên
--    Xóa Hóa đơn khi xóa DatPhong
IF OBJECT_ID('FK_HoaDon_DatPhong','F') IS NOT NULL ALTER TABLE HoaDon DROP CONSTRAINT FK_HoaDon_DatPhong;
IF OBJECT_ID('FK_HoaDon_NhanVien','F') IS NOT NULL ALTER TABLE HoaDon DROP CONSTRAINT FK_HoaDon_NhanVien;
GO
ALTER TABLE HoaDon
  ADD CONSTRAINT FK_HoaDon_DatPhong
    FOREIGN KEY (dat_phong_id)
    REFERENCES DatPhong(dat_phong_id)
    ON DELETE CASCADE,
   CONSTRAINT FK_HoaDon_NhanVien
    FOREIGN KEY (nhan_vien_id)
    REFERENCES NhanVien(nhan_vien_id)
    ON DELETE NO ACTION;
GO

-- 6. Thanh toán ↔ Hóa đơn
--    Xóa Thanh toán khi xóa Hóa đơn
IF OBJECT_ID('FK_ThanhToan_HoaDon','F') IS NOT NULL ALTER TABLE ThanhToan DROP CONSTRAINT FK_ThanhToan_HoaDon;
GO
ALTER TABLE ThanhToan
  ADD CONSTRAINT FK_ThanhToan_HoaDon
    FOREIGN KEY (hoa_don_id)
    REFERENCES HoaDon(hoa_don_id)
    ON DELETE CASCADE;
GO
--  Khóa ngoại NhanVien → VaiTro
IF OBJECT_ID('FK_NhanVien_VaiTro','F') IS NOT NULL
  ALTER TABLE NhanVien DROP CONSTRAINT FK_NhanVien_VaiTro;
GO
ALTER TABLE NhanVien
  ADD CONSTRAINT FK_NhanVien_VaiTro
    FOREIGN KEY (vai_tro_id)
    REFERENCES VaiTro(vai_tro_id)
    ON UPDATE CASCADE
    ON DELETE NO ACTION;
GO
INSERT INTO KhuyenMai (ten_khuyen_mai, phan_tram, ngay_bat_dau, ngay_ket_thuc) VALUES
  (N'Lễ 30/4 – 1/5 giảm 30%', 30, '2025-04-29', '2025-05-02'),
  (N'Khuyến mãi mùa hè giảm 20%', 20, '2025-06-01', '2025-06-30');
go
INSERT INTO VaiTro (ten_vai_tro) VALUES
(N'ADMIN'),     -- 1
(N'Quản Lý Khách Sạn'),     -- 2
(N'Nhân Viên Lễ Tân'),     -- 3
(N'Nhân Viên Buồng Phòng'),-- 4
(N'Nhân Viên Phục Vụ'),    -- 5
(N'Nhân Viên Bảo Vệ'),     -- 6
(N'Nhân Viên Kế Toán'),    -- 7
(N'Nhân Viên Kỹ Thuật');   -- 8
go
INSERT INTO LoaiPhong (ten_loai, mo_ta, gia_theo_dem) VALUES
(N'Phòng gia đình', N'Phòng 3 giường, rộng 60m2, view biển', 1500000),
(N'Giường đôi', N'Phòng 50m2, có bồn tắm', 2500000),
(N'Giường đơn', N'Phòng tiêu chuẩn 20m2', 800000);
go
INSERT INTO Phong (so_phong, loai_phong_id, trang_thai) VALUES
('P101', 1, 'trong'),
('P102', 1, 'dang_su_dung'),
('P203', 2, 'trong'),
('P204', 3, 'bao_tri'),
('P105', 1, 'trong'),
('P106', 1, 'dang_su_dung'),
('P207', 2, 'trong'),
('P208', 3, 'bao_tri'),
('P109', 1, 'trong'),
('P110', 1, 'dang_su_dung'),
('P211', 2, 'trong'),
('P212', 3, 'bao_tri');
go
INSERT INTO KhachHang (ho_ten, dia_chi, so_dien_thoai, email, cccd) VALUES
(N'Nguyễn Văn A', N'Hà Nội', '0912345678', 'a.nguyen@gmail.com', '123456789'),
(N'Trần Thị B', N'TP.HCM', '0987654321', 'b.tran@gmail.com', '987654321'),
(N'Lê Văn C', N'Đà Nẵng', '0901122334', NULL, '1122334455');
go
INSERT INTO DichVu (ten_dich_vu, mo_ta, gia) VALUES
(N'Minibar', N'Nước ngọt, bia, snack', 50000),
(N'Dọn phòng', N'Dọn phòng hàng ngày', 100000),
(N'Ăn sáng', N'Buffet sáng tại nhà hàng', 150000),
(N'Giặt ủi', N'Giặt ủi quần áo', 80000);
go
INSERT INTO NhanVien(ho_ten, sdt, vai_tro_id, ca_lam_viec, luong, tai_khoan, mat_khau) VALUES
(N'Nguyễn Văn Quản', '0912345678', 2, N'Giờ hành chính', 25000000, 'quanly', '123456'),
(N'Trần Thị Lễ', '0923456789', 3, N'Ca sáng', 12000000, 'letan1', '123456'),
(N'Lê Văn Tân', '0934567890', 3, N'Ca chiều', 12000000, 'letan2', '123456'),
(N'Phạm Thị Phòng', '0945678901', 4, N'Ca sáng', 10000000, '', ''),
(N'Đỗ Văn Phục', '0956789012', 5, N'Ca sáng', 9000000, '', ''),
(N'Vũ Văn Vệ', '0967890123', 6, N'Ca đêm', 8500000, '', ''),
(N'Bùi Thị Toán', '0978901234', 7, N'Giờ hành chính', 11000000, '', ''),
(N'Quản trị viên', '00000000', 1, N'24/24', 0, 'admin', '123');
-- select * from NhanVien

--SELECT * FROM DatPhong;

--SELECT * FROM Phong;
--SELECT * FROM LoaiPhong;

--drop database QLKS
-------------------------------- STORE --------------------------------
go
IF OBJECT_ID('dbo.sp_ThongTinHoaDon','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ThongTinHoaDon;
GO

CREATE PROCEDURE dbo.sp_ThongTinHoaDon
    @hoa_don_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        hd.hoa_don_id,
        hd.ngay_tao,
        hd.tong_tien,
        kh.ho_ten    AS khach_hang,
        p.so_phong  AS phong,
        dp.ngay_check_in,
        dp.ngay_check_out,
        dv.ten_dich_vu,
        dvdp.so_luong,
        dv.gia
    FROM HoaDon hd
    INNER JOIN DatPhong dp   ON hd.dat_phong_id = dp.dat_phong_id
    INNER JOIN Phong p       ON dp.phong_id      = p.phong_id
    INNER JOIN KhachHang kh  ON dp.khach_hang_id = kh.khach_hang_id
    LEFT  JOIN DichVuDatPhong dvdp ON dp.dat_phong_id = dvdp.dat_phong_id
    LEFT  JOIN DichVu dv     ON dvdp.dich_vu_id   = dv.dich_vu_id
    WHERE hd.hoa_don_id = @hoa_don_id;
END
go
IF OBJECT_ID('dbo.sp_ThongKeDatPhong','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ThongKeDatPhong;
GO
CREATE PROCEDURE dbo.sp_GetAllDatPhong
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        dp.dat_phong_id            AS MaDatPhong,
        dp.khach_hang_id           AS MaKhachHang,
        kh.ho_ten                  AS TenKhachHang,
        kh.dia_chi                 AS DiaChiKhachHang,
        kh.so_dien_thoai           AS DienThoaiKhachHang,

        dp.phong_id                AS MaPhong,
        p.so_phong                 AS SoPhong,
        p.trang_thai               AS TrangThaiPhong,

        p.loai_phong_id            AS MaLoaiPhong,
        lp.ten_loai                AS TenLoaiPhong,
        lp.gia_theo_dem            AS GiaTheoDem,

        dp.khuyen_mai_id           AS MaKhuyenMai,
        km.ten_khuyen_mai          AS TenKhuyenMai,
        km.phan_tram               AS PhanTramKhuyenMai,

        dp.nhan_vien_id            AS MaNhanVien,
        nv.ho_ten                  AS TenNhanVien,

        dp.ngay_dat                AS NgayDat,
        dp.ngay_check_in           AS NgayCheckIn,
        dp.ngay_check_out          AS NgayCheckOut,
        DATEDIFF(DAY, dp.ngay_check_in, dp.ngay_check_out) 
                                   AS SoDemO,

        dp.trang_thai              AS TrangThaiDatPhong
    FROM DatPhong dp
    INNER JOIN KhachHang      kh ON dp.khach_hang_id = kh.khach_hang_id
    INNER JOIN Phong          p  ON dp.phong_id      = p.phong_id
    INNER JOIN LoaiPhong      lp ON p.loai_phong_id   = lp.loai_phong_id
    LEFT  JOIN KhuyenMai      km ON dp.khuyen_mai_id = km.khuyen_mai_id
    INNER JOIN NhanVien       nv ON dp.nhan_vien_id   = nv.nhan_vien_id
    ORDER BY dp.ngay_dat DESC, dp.dat_phong_id;
END
GO
CREATE PROCEDURE dbo.sp_ThongKeDatPhong
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Các tổng cơ bản
    DECLARE @totalBookings   INT = (SELECT COUNT(*) FROM DatPhong);
    DECLARE @totalRooms      INT = (SELECT COUNT(*) FROM Phong);
    DECLARE @sameDayBookings INT = (
        SELECT COUNT(*) 
        FROM DatPhong 
        WHERE ngay_dat = ngay_check_in
    );

    -- 2. Xác định khoảng ngày & tháng trong data
    DECLARE @minDate    DATE = (SELECT MIN(ngay_dat) FROM DatPhong);
    DECLARE @maxDate    DATE = (SELECT MAX(ngay_dat) FROM DatPhong);
    DECLARE @periodDays   INT = CASE 
                                   WHEN @minDate IS NULL OR @maxDate IS NULL 
                                   THEN 0 
                                   ELSE DATEDIFF(DAY, @minDate, @maxDate) + 1 
                                 END;
    DECLARE @periodMonths INT = CASE 
                                   WHEN @minDate IS NULL OR @maxDate IS NULL 
                                   THEN 0 
                                   ELSE DATEDIFF(MONTH, @minDate, @maxDate) + 1 
                                 END;

    -- 3. Tính trung bình
    DECLARE @avgPerDay   DECIMAL(10,2) = CASE 
                                            WHEN @periodDays   = 0 THEN 0 
                                            ELSE ROUND(1.0 * @totalBookings   / @periodDays,   2) 
                                         END;
    DECLARE @avgPerMonth DECIMAL(10,2) = CASE 
                                            WHEN @periodMonths = 0 THEN 0 
                                            ELSE ROUND(1.0 * @totalBookings   / @periodMonths, 2) 
                                         END;

    -- 4. Lấy Top loại phòng và Top phòng
    ;WITH TopType AS
    (
        SELECT TOP 1 
            p.loai_phong_id,
            lp.ten_loai,
            COUNT(*) AS so_lan_dat
        FROM DatPhong dp
        JOIN Phong p      ON dp.phong_id      = p.phong_id
        JOIN LoaiPhong lp ON p.loai_phong_id  = lp.loai_phong_id
        GROUP BY p.loai_phong_id, lp.ten_loai
        ORDER BY COUNT(*) DESC
    ),
    TopRoom AS
    (
        SELECT TOP 1
            p.phong_id,
            p.so_phong,
            COUNT(*) AS so_lan_dat
        FROM DatPhong dp
        JOIN Phong p ON dp.phong_id = p.phong_id
        GROUP BY p.phong_id, p.so_phong
        ORDER BY COUNT(*) DESC
    )
    SELECT
        tt.loai_phong_id,
        tt.ten_loai               AS loai_phong_dat_nhieu_nhat,
        tt.so_lan_dat             AS so_lan_dat_loai_phong,

        tr.phong_id,
        tr.so_phong               AS phong_dat_nhieu_nhat,
        tr.so_lan_dat             AS so_lan_dat_phong,

        @totalBookings            AS tong_dat_phong,
        @totalRooms               AS tong_so_phong,
        CASE 
          WHEN @totalRooms = 0 THEN 0 
          ELSE ROUND(@totalBookings * 100.0 / @totalRooms, 2)
        END                        AS ty_le_dat_phong,

        @sameDayBookings          AS so_dat_ngay,
        CASE 
          WHEN @totalBookings = 0 THEN 0 
          ELSE ROUND(@sameDayBookings * 100.0 / @totalBookings, 2)
        END                        AS ty_le_dat_ngay,

        @periodDays               AS so_ngay_trong_data,
        @avgPerDay                AS trung_binh_dat_tren_ngay,    -- thêm

        @periodMonths             AS so_thang_trong_data,
        @avgPerMonth              AS trung_binh_dat_tren_thang   -- thêm
    FROM TopType tt
    CROSS JOIN TopRoom tr;
END
go
CREATE PROCEDURE dbo.sp_GetAllKhachHang
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        kh.khach_hang_id      AS MaKhachHang,
        kh.ho_ten             AS TenKhachHang,
        kh.dia_chi            AS DiaChi,
        kh.so_dien_thoai      AS DienThoai,
        kh.email              AS Email,
        kh.cccd               AS CCCD,
        -- Thống kê thêm:
        COUNT(DISTINCT dp.dat_phong_id)       AS SoLanDatPhong,
        COUNT(DISTINCT hd.hoa_don_id)         AS SoHoaDon,
        SUM(hd.tong_tien)                     AS TongChiTieu
    FROM KhachHang kh
    LEFT JOIN DatPhong dp 
        ON dp.khach_hang_id = kh.khach_hang_id
    LEFT JOIN HoaDon hd 
        ON hd.dat_phong_id = dp.dat_phong_id
    GROUP BY
        kh.khach_hang_id,
        kh.ho_ten,
        kh.dia_chi,
        kh.so_dien_thoai,
        kh.email,
        kh.cccd
    ORDER BY kh.ho_ten;
END
go
CREATE PROCEDURE dbo.sp_PhongThongTinVaBaoCao
AS
BEGIN
    SET NOCOUNT ON;

    ----------------------------------------
    -- 1) Chi tiết mỗi phòng
    ----------------------------------------
    SELECT
        p.phong_id            AS PhongID,
        p.so_phong            AS SoPhong,
        lp.ten_loai           AS LoaiPhong,
        lp.gia_theo_dem       AS GiaTheoDem,
        p.trang_thai          AS TrangThaiHienTai,

        -- Số lần đặt
        COUNT(dp.dat_phong_id)                                    AS SoLanDat,
        -- Tổng doanh thu của phòng
        SUM(ISNULL(hd.tong_tien,0))                               AS TongDoanhThu,
        -- Tổng số đêm sử dụng
        SUM(DATEDIFF(day, dp.ngay_check_in, dp.ngay_check_out))   AS TongSoDem
    FROM Phong p
    LEFT JOIN LoaiPhong lp  
        ON p.loai_phong_id = lp.loai_phong_id
    LEFT JOIN DatPhong dp  
        ON p.phong_id      = dp.phong_id
    LEFT JOIN HoaDon hd    
        ON dp.dat_phong_id = hd.dat_phong_id
    GROUP BY
        p.phong_id, p.so_phong, lp.ten_loai, lp.gia_theo_dem, p.trang_thai
    ORDER BY p.so_phong;

    ----------------------------------------
    -- 2) Báo cáo tổng hợp
    ----------------------------------------
    DECLARE 
        @TotalRooms           INT       = (SELECT COUNT(*) FROM Phong),
        @RoomsOccupied        INT       = (SELECT COUNT(*) FROM Phong WHERE trang_thai <> 'trong'),
        @RoomsAvailable       INT       = (SELECT COUNT(*) FROM Phong WHERE trang_thai = 'trong'),
        @TotalBookings        INT       = (SELECT COUNT(*) FROM DatPhong),
        @TotalNights          INT       = (SELECT ISNULL(SUM(DATEDIFF(day, ngay_check_in, ngay_check_out)),0) FROM DatPhong),
        @TotalRevenue         DECIMAL(18,2) = (SELECT ISNULL(SUM(tong_tien),0) FROM HoaDon);

    DECLARE
        @OccupancyRate        DECIMAL(5,2)  = CASE WHEN @TotalRooms=0 THEN 0 
                                                  ELSE ROUND(@RoomsOccupied*100.0/@TotalRooms,2) END,
        @AvgBookingsPerRoom   DECIMAL(10,2) = CASE WHEN @TotalRooms=0 THEN 0 
                                                  ELSE ROUND(@TotalBookings*1.0/@TotalRooms,2) END,
        @AvgNightsPerRoom     DECIMAL(10,2) = CASE WHEN @TotalRooms=0 THEN 0 
                                                  ELSE ROUND(@TotalNights*1.0/@TotalRooms,2) END;

    SELECT
        @TotalRooms           AS TotalRooms,            -- Tổng số phòng
        @RoomsOccupied        AS RoomsOccupied,         -- Phòng đang có khách
        @RoomsAvailable       AS RoomsAvailable,        -- Phòng trống
        @OccupancyRate        AS OccupancyRate,         -- Tỷ lệ lấp đầy (%)
        @TotalBookings        AS TotalBookings,         -- Tổng lượt đặt
        @AvgBookingsPerRoom   AS AvgBookingsPerRoom,    -- TB lượt đặt / phòng
        @TotalNights          AS TotalNights,           -- Tổng số đêm sử dụng
        @AvgNightsPerRoom     AS AvgNightsPerRoom,      -- TB số đêm / phòng
        @TotalRevenue         AS TotalRevenue;          -- Tổng doanh thu
END
GO
CREATE PROCEDURE dbo.sp_GetAllNhanVien
AS
BEGIN
    SET NOCOUNT ON;

    ----------------------------------------
    -- 1) Chi tiết từng nhân viên
    ----------------------------------------
    SELECT
        nv.nhan_vien_id        AS MaNhanVien,
        nv.ho_ten              AS TenNhanVien,
        vt.ten_vai_tro         AS TenVaiTro,
        nv.ca_lam_viec         AS CaLamViec,
        nv.luong               AS Luong,
        nv.tai_khoan           AS TaiKhoan,
        -- Thống kê:
        COUNT(DISTINCT dp.dat_phong_id)    AS SoLuotDatPhong,
        COUNT(DISTINCT hd.hoa_don_id)      AS SoHoaDon,
        SUM(ISNULL(hd.tong_tien,0))        AS TongDoanhThu
    FROM NhanVien nv
    INNER JOIN VaiTro vt
        ON nv.vai_tro_id = vt.vai_tro_id
    LEFT JOIN DatPhong dp
        ON dp.nhan_vien_id = nv.nhan_vien_id
    LEFT JOIN HoaDon hd
        ON hd.nhan_vien_id = nv.nhan_vien_id
    GROUP BY
        nv.nhan_vien_id, nv.ho_ten, vt.ten_vai_tro,
        nv.ca_lam_viec, nv.luong, nv.tai_khoan
    ORDER BY nv.ho_ten;

    ----------------------------------------
    -- 2) Báo cáo tổng hợp chung
    ----------------------------------------
    DECLARE
        @TotalNV       INT           = (SELECT COUNT(*) FROM NhanVien),
        @TotalDP       INT           = (SELECT COUNT(*) FROM DatPhong),
        @TotalHD       INT           = (SELECT COUNT(*) FROM HoaDon),
        @TotalRev      DECIMAL(18,2) = (SELECT ISNULL(SUM(tong_tien),0) FROM HoaDon);

    DECLARE
        @AvgDPPerNV    DECIMAL(10,2) = CASE WHEN @TotalNV=0 THEN 0 
                                           ELSE ROUND(@TotalDP*1.0/@TotalNV,2) END,
        @AvgHDPerNV    DECIMAL(10,2) = CASE WHEN @TotalNV=0 THEN 0 
                                           ELSE ROUND(@TotalHD*1.0/@TotalNV,2) END,
        @AvgRevPerNV   DECIMAL(18,2) = CASE WHEN @TotalNV=0 THEN 0 
                                           ELSE ROUND(@TotalRev*1.0/@TotalNV,2) END;

    SELECT
        @TotalNV      AS TotalNhanVien,
        @TotalDP      AS TotalDatPhong,
        @AvgDPPerNV   AS AvgDatPhongPerNV,
        @TotalHD      AS TotalHoaDon,
        @AvgHDPerNV   AS AvgHoaDonPerNV,
        @TotalRev     AS TotalDoanhThu,
        @AvgRevPerNV  AS AvgDoanhThuPerNV;

    ----------------------------------------
    -- 3) Top toàn bộ nhân viên theo doanh thu
    ----------------------------------------
    ;WITH RevByNV AS
    (
        SELECT
            nv.nhan_vien_id,
            nv.ho_ten,
            SUM(ISNULL(hd.tong_tien,0)) AS DoanhThu
        FROM NhanVien nv
        LEFT JOIN HoaDon hd
            ON hd.nhan_vien_id = nv.nhan_vien_id
        GROUP BY nv.nhan_vien_id, nv.ho_ten
    )
    SELECT TOP 1
        nv.nhan_vien_id      AS TopNV_ID,
        nv.ho_ten            AS TopNV_Ten,
        nv.DoanhThu          AS TopNV_DoanhThu
    FROM RevByNV nv
    ORDER BY nv.DoanhThu DESC;

    ----------------------------------------
    -- 4) Top nhân viên 'Lễ tân' theo doanh thu
    ----------------------------------------
    ;WITH RevLeTan AS
    (
        SELECT
            nv.nhan_vien_id,
            nv.ho_ten,
            SUM(ISNULL(hd.tong_tien,0)) AS DoanhThu
        FROM NhanVien nv
        INNER JOIN VaiTro vt
            ON nv.vai_tro_id = vt.vai_tro_id
           AND vt.ten_vai_tro = N'Lễ tân'
        LEFT JOIN HoaDon hd
            ON hd.nhan_vien_id = nv.nhan_vien_id
        GROUP BY nv.nhan_vien_id, nv.ho_ten
    )
    SELECT TOP 1
        lt.nhan_vien_id      AS TopLeTan_ID,
        lt.ho_ten            AS TopLeTan_Ten,
        lt.DoanhThu          AS TopLeTan_DoanhThu
    FROM RevLeTan lt
    ORDER BY lt.DoanhThu DESC;
END
GO
CREATE PROCEDURE dbo.sp_GetTopNhanVienDoanhThu
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        nv.nhan_vien_id   AS MaNhanVien,
        nv.ho_ten         AS TenNhanVien,
        vt.ten_vai_tro    AS TenVaiTro,
        SUM(ISNULL(hd.tong_tien, 0)) AS TongDoanhThu
    FROM NhanVien nv
    -- lấy doanh thu từ bảng hóa đơn
    LEFT JOIN HoaDon hd 
        ON hd.nhan_vien_id = nv.nhan_vien_id
    -- link tới VaiTro để lấy tên vai trò
    LEFT JOIN VaiTro vt 
        ON nv.vai_tro_id   = vt.vai_tro_id
    GROUP BY
        nv.nhan_vien_id,
        nv.ho_ten,
        vt.ten_vai_tro
    ORDER BY
        SUM(ISNULL(hd.tong_tien, 0)) DESC;
END
