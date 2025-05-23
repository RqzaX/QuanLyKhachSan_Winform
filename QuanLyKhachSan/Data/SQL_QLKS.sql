USE [QLKS]
GO
/****** Object:  Table [dbo].[DatPhong]    Script Date: 23/05/2025 12:42:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatPhong](
	[dat_phong_id] [int] IDENTITY(1,1) NOT NULL,
	[khach_hang_id] [int] NOT NULL,
	[phong_id] [int] NOT NULL,
	[khuyen_mai_id] [int] NULL,
	[nhan_vien_id] [int] NOT NULL,
	[ngay_check_in] [date] NOT NULL,
	[ngay_check_out] [date] NOT NULL,
	[ngay_dat] [date] NOT NULL,
	[trang_thai] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[dat_phong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DichVu]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DichVu](
	[dich_vu_id] [int] IDENTITY(1,1) NOT NULL,
	[ten_dich_vu] [nvarchar](100) NULL,
	[mo_ta] [nvarchar](max) NULL,
	[gia] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[dich_vu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DichVuDatPhong]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DichVuDatPhong](
	[dich_vu_dat_phong_id] [int] IDENTITY(1,1) NOT NULL,
	[dat_phong_id] [int] NOT NULL,
	[dich_vu_id] [int] NOT NULL,
	[so_luong] [int] NULL,
	[ngay_su_dung] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[dich_vu_dat_phong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDon]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDon](
	[hoa_don_id] [int] IDENTITY(1,1) NOT NULL,
	[dat_phong_id] [int] NOT NULL,
	[nhan_vien_id] [int] NOT NULL,
	[ten_nhan_vien] [nvarchar](100) NULL,
	[ngay_tao] [date] NOT NULL,
	[tong_tien] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[hoa_don_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[khach_hang_id] [int] IDENTITY(1,1) NOT NULL,
	[ho_ten] [nvarchar](100) NOT NULL,
	[dia_chi] [nvarchar](max) NULL,
	[so_dien_thoai] [nvarchar](15) NULL,
	[email] [nvarchar](100) NULL,
	[cccd] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[khach_hang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[cccd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhuyenMai]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhuyenMai](
	[khuyen_mai_id] [int] IDENTITY(1,1) NOT NULL,
	[ten_khuyen_mai] [nvarchar](50) NULL,
	[phan_tram] [int] NOT NULL,
	[ngay_bat_dau] [date] NOT NULL,
	[ngay_ket_thuc] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[khuyen_mai_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiPhong]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiPhong](
	[loai_phong_id] [int] IDENTITY(1,1) NOT NULL,
	[ten_loai] [nvarchar](50) NULL,
	[mo_ta] [nvarchar](max) NULL,
	[gia_theo_dem] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[loai_phong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[nhan_vien_id] [int] IDENTITY(1,1) NOT NULL,
	[ho_ten] [nvarchar](100) NULL,
	[sdt] [nvarchar](10) NULL,
	[vai_tro_id] [int] NULL,
	[ca_lam_viec] [nvarchar](50) NULL,
	[luong] [decimal](10, 2) NULL,
	[tai_khoan] [nvarchar](20) NULL,
	[mat_khau] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[nhan_vien_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phong]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phong](
	[phong_id] [int] IDENTITY(1,1) NOT NULL,
	[so_phong] [nvarchar](10) NOT NULL,
	[loai_phong_id] [int] NOT NULL,
	[trang_thai] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[phong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[so_phong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThanhToan]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThanhToan](
	[thanh_toan_id] [int] IDENTITY(1,1) NOT NULL,
	[hoa_don_id] [int] NOT NULL,
	[so_tien] [decimal](10, 2) NOT NULL,
	[phuong_thuc] [nvarchar](10) NULL,
	[ngay_thanh_toan] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[thanh_toan_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VaiTro]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaiTro](
	[vai_tro_id] [int] IDENTITY(1,1) NOT NULL,
	[ten_vai_tro] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[vai_tro_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ten_vai_tro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DatPhong] ADD  DEFAULT (CONVERT([date],getdate())) FOR [ngay_dat]
GO
ALTER TABLE [dbo].[DichVuDatPhong] ADD  DEFAULT ((1)) FOR [so_luong]
GO
ALTER TABLE [dbo].[HoaDon] ADD  DEFAULT (CONVERT([date],getdate())) FOR [ngay_tao]
GO
ALTER TABLE [dbo].[KhuyenMai] ADD  DEFAULT (CONVERT([date],getdate())) FOR [ngay_bat_dau]
GO
ALTER TABLE [dbo].[KhuyenMai] ADD  DEFAULT (CONVERT([date],getdate())) FOR [ngay_ket_thuc]
GO
ALTER TABLE [dbo].[ThanhToan] ADD  DEFAULT (CONVERT([date],getdate())) FOR [ngay_thanh_toan]
GO
ALTER TABLE [dbo].[DatPhong]  WITH CHECK ADD  CONSTRAINT [FK_DatPhong_KhachHang] FOREIGN KEY([khach_hang_id])
REFERENCES [dbo].[KhachHang] ([khach_hang_id])
GO
ALTER TABLE [dbo].[DatPhong] CHECK CONSTRAINT [FK_DatPhong_KhachHang]
GO
ALTER TABLE [dbo].[DatPhong]  WITH CHECK ADD  CONSTRAINT [FK_DatPhong_KhuyenMai] FOREIGN KEY([khuyen_mai_id])
REFERENCES [dbo].[KhuyenMai] ([khuyen_mai_id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[DatPhong] CHECK CONSTRAINT [FK_DatPhong_KhuyenMai]
GO
ALTER TABLE [dbo].[DatPhong]  WITH CHECK ADD  CONSTRAINT [FK_DatPhong_NhanVien] FOREIGN KEY([nhan_vien_id])
REFERENCES [dbo].[NhanVien] ([nhan_vien_id])
GO
ALTER TABLE [dbo].[DatPhong] CHECK CONSTRAINT [FK_DatPhong_NhanVien]
GO
ALTER TABLE [dbo].[DatPhong]  WITH CHECK ADD  CONSTRAINT [FK_DatPhong_Phong] FOREIGN KEY([phong_id])
REFERENCES [dbo].[Phong] ([phong_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DatPhong] CHECK CONSTRAINT [FK_DatPhong_Phong]
GO
ALTER TABLE [dbo].[DichVuDatPhong]  WITH CHECK ADD  CONSTRAINT [FK_DichVuDatPhong_DatPhong] FOREIGN KEY([dat_phong_id])
REFERENCES [dbo].[DatPhong] ([dat_phong_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DichVuDatPhong] CHECK CONSTRAINT [FK_DichVuDatPhong_DatPhong]
GO
ALTER TABLE [dbo].[DichVuDatPhong]  WITH CHECK ADD  CONSTRAINT [FK_DichVuDatPhong_DichVu] FOREIGN KEY([dich_vu_id])
REFERENCES [dbo].[DichVu] ([dich_vu_id])
GO
ALTER TABLE [dbo].[DichVuDatPhong] CHECK CONSTRAINT [FK_DichVuDatPhong_DichVu]
GO
ALTER TABLE [dbo].[HoaDon]  WITH CHECK ADD  CONSTRAINT [FK_HoaDon_DatPhong] FOREIGN KEY([dat_phong_id])
REFERENCES [dbo].[DatPhong] ([dat_phong_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HoaDon] CHECK CONSTRAINT [FK_HoaDon_DatPhong]
GO
ALTER TABLE [dbo].[HoaDon]  WITH CHECK ADD  CONSTRAINT [FK_HoaDon_NhanVien] FOREIGN KEY([nhan_vien_id])
REFERENCES [dbo].[NhanVien] ([nhan_vien_id])
GO
ALTER TABLE [dbo].[HoaDon] CHECK CONSTRAINT [FK_HoaDon_NhanVien]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_VaiTro] FOREIGN KEY([vai_tro_id])
REFERENCES [dbo].[VaiTro] ([vai_tro_id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK_NhanVien_VaiTro]
GO
ALTER TABLE [dbo].[Phong]  WITH CHECK ADD  CONSTRAINT [FK_Phong_LoaiPhong] FOREIGN KEY([loai_phong_id])
REFERENCES [dbo].[LoaiPhong] ([loai_phong_id])
GO
ALTER TABLE [dbo].[Phong] CHECK CONSTRAINT [FK_Phong_LoaiPhong]
GO
ALTER TABLE [dbo].[ThanhToan]  WITH CHECK ADD  CONSTRAINT [FK_ThanhToan_HoaDon] FOREIGN KEY([hoa_don_id])
REFERENCES [dbo].[HoaDon] ([hoa_don_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThanhToan] CHECK CONSTRAINT [FK_ThanhToan_HoaDon]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllDatPhong]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllDatPhong]
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
/****** Object:  StoredProcedure [dbo].[sp_GetAllKhachHang]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllKhachHang]
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
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllNhanVien]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllNhanVien]
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
/****** Object:  StoredProcedure [dbo].[sp_GetTopNhanVienDoanhThu]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTopNhanVienDoanhThu]
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
GO
/****** Object:  StoredProcedure [dbo].[sp_PhongThongTinVaBaoCao]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_PhongThongTinVaBaoCao]
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
/****** Object:  StoredProcedure [dbo].[sp_ReportNhanVien]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ReportNhanVien]
    @FromDate DATE,
    @ToDate   DATE
AS
BEGIN
    SET NOCOUNT ON;

    ----------------------------------------
    -- 1) Chi tiết mỗi nhân viên
    ----------------------------------------
    SELECT
        nv.nhan_vien_id      AS MaNhanVien,
        nv.ho_ten            AS TenNhanVien,
        vt.ten_vai_tro       AS VaiTro,
        nv.ca_lam_viec       AS CaLamViec,
        nv.luong             AS LuongCoBan,
        COUNT(DISTINCT dp.dat_phong_id)  AS SoLuotDatPhong,
        COUNT(DISTINCT hd.hoa_don_id)    AS SoHoaDon,
        SUM(ISNULL(hd.tong_tien,0))      AS TongDoanhThu
    FROM NhanVien nv
    INNER JOIN VaiTro vt
        ON nv.vai_tro_id = vt.vai_tro_id
    LEFT JOIN DatPhong dp
        ON dp.nhan_vien_id = nv.nhan_vien_id
       AND dp.ngay_dat BETWEEN @FromDate AND @ToDate
    LEFT JOIN HoaDon hd
        ON hd.nhan_vien_id = nv.nhan_vien_id
       AND hd.ngay_tao BETWEEN @FromDate AND @ToDate
    GROUP BY
        nv.nhan_vien_id, nv.ho_ten, vt.ten_vai_tro,
        nv.ca_lam_viec, nv.luong
    ORDER BY TongDoanhThu DESC;

    ----------------------------------------
    -- 2) Nhân viên có doanh thu cao nhất
    ----------------------------------------
    SELECT TOP 1
        nv.nhan_vien_id      AS TopNhanVienID,
        nv.ho_ten            AS TopTenNhanVien,
        SUM(ISNULL(hd.tong_tien,0)) AS TopDoanhThu
    FROM NhanVien nv
    INNER JOIN HoaDon hd
        ON hd.nhan_vien_id = nv.nhan_vien_id
       AND hd.ngay_tao BETWEEN @FromDate AND @ToDate
    GROUP BY nv.nhan_vien_id, nv.ho_ten
    ORDER BY TopDoanhThu DESC;

    ----------------------------------------
    -- 3) Tổng doanh thu toàn bộ
    ----------------------------------------
    SELECT
        SUM(ISNULL(hd.tong_tien,0)) AS TongDoanhThu
    FROM HoaDon hd
    WHERE hd.ngay_tao BETWEEN @FromDate AND @ToDate;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ThongKeDatPhong]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ThongKeDatPhong]
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
GO
/****** Object:  StoredProcedure [dbo].[sp_ThongTinHoaDon]    Script Date: 23/05/2025 12:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_ThongTinHoaDon]
    @hoa_don_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        hd.hoa_don_id,
        hd.ngay_tao,
        hd.tong_tien,

        -- Ngày check in / check out
        dp.ngay_check_in    AS ngay_check_in,
        dp.ngay_check_out   AS ngay_check_out,

        -- Thông tin khách hàng
        kh.ho_ten           AS khach_hang,
        kh.dia_chi          AS dia_chi_khach_hang,

        -- Thông tin phòng & giá
        p.so_phong          AS phong,
        lp.gia_theo_dem     AS tien_mot_dem,
        DATEDIFF(
            DAY, 
            dp.ngay_check_in, 
            dp.ngay_check_out
        )                   AS tong_ngay_su_dung,
        (lp.gia_theo_dem * 
            DATEDIFF(DAY, dp.ngay_check_in, dp.ngay_check_out)
        )                   AS thanh_tien_phong,

        -- Thông tin khuyến mãi
        km.phan_tram        AS phan_tram_khuyen_mai,
        CASE 
            WHEN dp.khuyen_mai_id IS NOT NULL 
            THEN (lp.gia_theo_dem 
                  * DATEDIFF(DAY, dp.ngay_check_in, dp.ngay_check_out)
                  * km.phan_tram 
                  / 100.0)
            ELSE 0
        END                 AS tien_giam_gia,

        -- Thông tin dịch vụ phát sinh
        dv.ten_dich_vu      AS ten_dich_vu,
        dvdp.so_luong       AS so_luong_dich_vu,
        dv.gia              AS gia_dich_vu,

        -- Nhân viên thanh toán
        hd.nhan_vien_id,
        hd.ten_nhan_vien    AS nhan_vien_thanh_toan

    FROM HoaDon hd
    INNER JOIN DatPhong dp      ON hd.dat_phong_id     = dp.dat_phong_id
    INNER JOIN Phong p          ON dp.phong_id        = p.phong_id
    INNER JOIN LoaiPhong lp     ON p.loai_phong_id    = lp.loai_phong_id
    LEFT  JOIN KhuyenMai km     ON dp.khuyen_mai_id   = km.khuyen_mai_id
    INNER JOIN KhachHang kh     ON dp.khach_hang_id   = kh.khach_hang_id
    LEFT  JOIN DichVuDatPhong dvdp ON dp.dat_phong_id = dvdp.dat_phong_id
    LEFT  JOIN DichVu dv        ON dvdp.dich_vu_id    = dv.dich_vu_id
    WHERE hd.hoa_don_id = @hoa_don_id;
END

GO
