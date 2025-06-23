CREATE DATABASE TruongSach;
GO
USE TruongSach;
GO

Create table [NGUOIDUNG]
(
	[MaNguoiDung] Integer IDENTITY(10000,1) NOT NULL,
	[HoTen] Nvarchar(50) NOT NULL,
	[Email] Nvarchar(50) NULL,
	[MatKhau] Nvarchar(255) NULL,
	[SoDienThoai] NVARCHAR(20) NULL,
	[NgayDangKy] Smalldatetime NULL,
	[TrangThai] Nvarchar(50) NULL,
	[DiemThuong] DECIMAL(10,2) NULL,
	[MaVaiTro] Integer NOT NULL,
Primary Key ([MaNguoiDung])
) 
ALTER TABLE NGUOIDUNG ADD
    GoogleId NVARCHAR(255) NULL,
    AvatarUrl NVARCHAR(MAX) NULL; 
go

Create table [VAITRO]
(
	[MaVaiTro] Integer IDENTITY(10000,1) NOT NULL,
	[TenVaiTro] Nvarchar(20) NOT NULL,
	[MoTa] Nvarchar(500) NOT NULL,
Primary Key ([MaVaiTro])
) 
go

Create table [SANPHAM]
(
	[MaSanPham] Integer IDENTITY(10000,1) NOT NULL,
	[TenSanPham] Nvarchar(50) NOT NULL,
	[MoTa] Nvarchar(500) NULL,
	[SoLuongTon] Integer NOT NULL,
	[GiaBan] Decimal(18,2) NOT NULL,
	[TrangThai] Nvarchar(50) NULL,
	[HinhAnhDaiDien] NVARCHAR(MAX) NULL,
	[MaLoaiSanPham] Integer NOT NULL,
Primary Key ([MaSanPham])
) 
go


Create table [BAIVIET]
(
	[MaBaiViet] Integer IDENTITY(10000,1) NOT NULL,
	[TieuDe] Nvarchar(200) NOT NULL,
	[NoiDung] Nvarchar(MAX) NOT NULL,
	[HinhAnh] Nvarchar(500) NOT NULL,
	[TrangThai] Nvarchar(50) NULL,
	[NgayDang] Smalldatetime NULL,
	[MaTruong] Integer NOT NULL,
Primary Key ([MaBaiViet])
) 
go

Create table [LOAISANPHAM]
(
	[MaLoaiSanPham] Integer IDENTITY(10000,1) NOT NULL,
	[TenLoaiSanPham] Nvarchar(20) NOT NULL,
	[TongSoLuong] Integer NOT NULL,
Primary Key ([MaLoaiSanPham])
) 
go

Create table [CHIENDICH]
(
	[MaChienDich] Integer IDENTITY(10000,1) NOT NULL,
	[TieuDe] Nvarchar(200) NOT NULL,
	[MoTa] NVARCHAR(MAX) NOT NULL,
	[SoTienHienTai ] Decimal(18,0) NOT NULL,
	[MucTieuQuyenGop] Decimal(18,0) NOT NULL,
	[HinhAnhDaiDien] Nvarchar(500) NOT NULL,
	[TrangThai] Nvarchar(50) NULL,
	[NguoiDaiDien] Nvarchar(50) NULL,
	[NgayTao] Smalldatetime NULL,
	[MaTruong] Integer NOT NULL,
Primary Key ([MaChienDich])
) 
go

Create table [DONGGOP]
(
	[MaDongGop] Integer IDENTITY(10000,1) NOT NULL,
	[SoTien] Nvarchar(50) NOT NULL,
	[HinhThuc] Nvarchar(20) NOT NULL,
	[NoiDung] Nvarchar(MAX) NULL,
	[MaNguoiDung] Integer NOT NULL,
	[MaChienDich] Integer NOT NULL,
Primary Key ([MaDongGop])
) 
go

Create table [HOADON]
(
	[MaHoaDon] Integer IDENTITY(10000,1) NOT NULL,
	[NgayLap] Smalldatetime NULL,
	[TongTien] Money NULL,
	[HinhThucThanhToan] Nvarchar(50) NULL,
	[TrangThai] Nvarchar(50) NULL,
	[MaNguoiDung] Integer NOT NULL,
Primary Key ([MaHoaDon])
) 
go

Create table [TRUONG]
(
	[MaTruong] Integer IDENTITY(10000,1) NOT NULL,
	[TenTruong] Nvarchar(50) NULL,
	[DiaChi] Nvarchar(100) NULL,
	[KhuVuc] Nvarchar(50) NULL,
	[GhiChu] Nvarchar(50) NULL,
	[ViDo] Nvarchar(50) NULL,
	[KinhDo] Nvarchar(50) NULL,
Primary Key ([MaTruong])
) 
go

Create table [HINHANHCHIENDICH]
(
	[MaHinhAnh] Integer IDENTITY(10000,1) NOT NULL,
	[HinhAnh] NVARCHAR(MAX) NULL,
	[MaChienDich] Integer NOT NULL,
Primary Key ([MaHinhAnh])
) 
go

Create table [HINHANHSANPHAM]
(
	[MaHinhAnh] Integer IDENTITY(10000,1) NOT NULL,
	[HinhAnh] NVARCHAR(MAX) NOT NULL,
	[MaSanPham] Integer NOT NULL,
Primary Key ([MaHinhAnh])
) 
go

Create table [BINHLUAN]
(
	[MaBinhLuan] Integer IDENTITY(10000,1) NOT NULL,
	[NoiDung] Nvarchar(500) NULL,
	[ThoiGian] Smalldatetime NULL,
	[MaBaiViet] Integer NOT NULL,
Primary Key ([MaBinhLuan])
) 
go

Create table [LUOTTHICH]
(
	[MaLuotThich] Integer IDENTITY(10000,1) NOT NULL,
	[ThoiGian] Smalldatetime NULL,
	[MaBaiViet] Integer NOT NULL,
Primary Key ([MaLuotThich])
) 
go

Create table [HINHANHBAIVIET]
(
	[MaHinhAnh] Integer IDENTITY(10000,1) NOT NULL,
	[HinhAnh] NVARCHAR(MAX) NULL,
	[MaBaiViet] Integer NOT NULL,
Primary Key ([MaHinhAnh])
) 
go

Create table [CHITIETHOADON]
(
	[MaSanPham] Integer NOT NULL,
	[MaHoaDon] Integer NOT NULL,
	[SoLuong] Integer NOT NULL,
Primary Key ([MaSanPham],[MaHoaDon])
) 
go


Alter table [DONGGOP] add  foreign key([MaNguoiDung]) references [NGUOIDUNG] ([MaNguoiDung])  on update no action on delete no action 
go
Alter table [HOADON] add  foreign key([MaNguoiDung]) references [NGUOIDUNG] ([MaNguoiDung])  on update no action on delete no action 
go
Alter table [NGUOIDUNG] add  foreign key([MaVaiTro]) references [VAITRO] ([MaVaiTro])  on update no action on delete no action 
go
Alter table [HINHANHSANPHAM] add  foreign key([MaSanPham]) references [SANPHAM] ([MaSanPham])  on update no action on delete no action 
go
Alter table [CHITIETHOADON] add  foreign key([MaSanPham]) references [SANPHAM] ([MaSanPham])  on update no action on delete no action 
go
Alter table [HINHANHBAIVIET] add  foreign key([MaBaiViet]) references [BAIVIET] ([MaBaiViet])  on update no action on delete no action 
go
Alter table [BINHLUAN] add  foreign key([MaBaiViet]) references [BAIVIET] ([MaBaiViet])  on update no action on delete no action 
go
Alter table [LUOTTHICH] add  foreign key([MaBaiViet]) references [BAIVIET] ([MaBaiViet])  on update no action on delete no action 
go
Alter table [SANPHAM] add  foreign key([MaLoaiSanPham]) references [LOAISANPHAM] ([MaLoaiSanPham])  on update no action on delete no action 
go
Alter table [HINHANHCHIENDICH] add  foreign key([MaChienDich]) references [CHIENDICH] ([MaChienDich])  on update no action on delete no action 
go
Alter table [DONGGOP] add  foreign key([MaChienDich]) references [CHIENDICH] ([MaChienDich])  on update no action on delete no action 
go
Alter table [CHITIETHOADON] add  foreign key([MaHoaDon]) references [HOADON] ([MaHoaDon])  on update no action on delete no action 
go
Alter table [CHIENDICH] add  foreign key([MaTruong]) references [TRUONG] ([MaTruong])  on update no action on delete no action 
go
Alter table [BAIVIET] add  foreign key([MaTruong]) references [TRUONG] ([MaTruong])  on update no action on delete no action 
go


Set quoted_identifier on
go


Set quoted_identifier off
go

-- data
-- Thêm dữ liệu mẫu vào bảng TRUONG
INSERT INTO TRUONG (TenTruong, DiaChi, KhuVuc, GhiChu, ViDo, KinhDo) VALUES
(N'Trường Tiểu học Bình Minh', N'123 Đường Lê Lợi, Q.1, TP.HCM', N'TP.HCM', N'Trường vùng nội thành', '10.7769', '106.7009'),
(N'Trường THCS Hòa Bình', N'45 Nguyễn Trãi, Q.5, TP.HCM', N'TP.HCM', N'Trường trung học cơ sở', '10.7620', '106.6825'),
(N'Trường Tiểu học Nguyễn Văn Trỗi', N'75 Trần Hưng Đạo, Q.1, TP.HCM', N'TP.HCM', N'Trường tiểu học mẫu', '10.7735', '106.7062');

-- Thêm dữ liệu mẫu vào bảng CHIENDICH
INSERT INTO CHIENDICH (TieuDe, MoTa, SoTienHienTai, MucTieuQuyenGop, HinhAnhDaiDien, TrangThai, NguoiDaiDien, NgayTao, MaTruong) VALUES
(N'Ủng hộ sách cho học sinh nghèo', N'Gây quỹ để mua sách giáo khoa cho học sinh vùng khó khăn.', 1500000, 5000000, 'https://hoanghamobile.com/tin-tuc/wp-content/uploads/2024/07/anh-truong-hoc-13.jpg', N'Đang diễn ra', N'Nguyễn Văn A', GETDATE(), 10000),
(N'Cải thiện cơ sở vật chất trường học', N'Sửa chữa lớp học xuống cấp và mua sắm bàn ghế mới.', 2000000, 8000000, N'https://thquyetthangvl.quangtri.edu.vn/upload/30922/20181006/42551288_1898751446876867_8963859607340449792_o38.jpg', N'Đang diễn ra', N'Trần Thị B', GETDATE(), 10001),
(N'Học bổng cho học sinh vượt khó', N'Trao học bổng cho các em có hoàn cảnh khó khăn.', 1000000, 3000000, N'https://hoanghamobile.com/tin-tuc/wp-content/uploads/2024/07/anh-truong-hoc-3.jpg', N'Chưa bắt đầu', N'Lê Văn C', GETDATE(), 10002),
(N'Nhà Vệ Sinh Sạch', N'Sửa chữa cơ sở vật chất', 1000000, 3000000, N'https://hoanghamobile.com/tin-tuc/wp-content/uploads/2024/07/anh-truong-hoc-3.jpg', N'Đã kết thúc', N'Lê Văn C', GETDATE(), 10002);

-- Thêm dữ liệu mẫu vào bảng HINHANHCHIENDICH
INSERT INTO HINHANHCHIENDICH (HinhAnh, MaChienDich) VALUES
(N'https://png.pngtree.com/png-vector/20190927/ourlarge/pngtree-school-building-vector-illustration-school-building-cartoon-png-image_1745834.jpg', 10001),
(N'https://png.pngtree.com/png-vector/20190927/ourlarge/pngtree-school-building-vector-illustration-school-building-cartoon-png-image_1745834.jpg', 10002),
(N'https://png.pngtree.com/png-vector/20190927/ourlarge/pngtree-school-building-vector-illustration-school-building-cartoon-png-image_1745834.jpg', 10003),
(N'https://png.pngtree.com/png-vector/20190927/ourlarge/pngtree-school-building-vector-illustration-school-building-cartoon-png-image_1745834.jpg', 10004);

--------------------
INSERT INTO VAITRO (TenVaiTro, MoTa)
VALUES 
('User', N'Người dùng thông thường'),
('Admin', N'Người có quyền quản trị hệ thống');

CREATE TRIGGER TRG_Update_Chiendich_After_DongGop
ON DONGGOP
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật SoTienHienTai
    UPDATE CD
    SET CD.SoTienHienTai = CD.SoTienHienTai + 
        CAST(I.SoTien AS DECIMAL(18,0))
    FROM CHIENDICH CD
    INNER JOIN INSERTED I ON CD.MaChienDich = I.MaChienDich;

    -- Cập nhật TrangThai nếu đạt mục tiêu
    UPDATE CD
    SET CD.TrangThai = 'HoanThanh'
    FROM CHIENDICH CD
    WHERE CD.SoTienHienTai >= CD.MucTieuQuyenGop
      AND CD.TrangThai != 'HoanThanh';
END;
