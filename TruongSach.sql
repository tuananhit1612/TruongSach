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
	GoogleId Nvarchar(255) NULL,
	AvatarUrl Nvarchar(Max) NULL
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
	[NgayDang] Smalldatetime NULL,
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
	[SoTien] Decimal(18,2) NOT NULL,
	[HinhThuc] Nvarchar(20) NOT NULL,
	[NoiDung] Nvarchar(MAX) NULL,
	[NgayDongGop] Smalldatetime NULL,
	[TrangThai] Nvarchar(20) NOT NULL,
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
    PhiVanChuyen MONEY DEFAULT 0,
    ThueVAT MONEY DEFAULT 0,
	[HinhThucThanhToan] Nvarchar(50) NULL,
	DiaChi Nvarchar(500) NULL,
	[TrangThai] Nvarchar(50) NULL,
	[MaNguoiDung] Integer NOT NULL
Primary Key ([MaHoaDon])
) 

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
CREATE TABLE ChienDichYeuThich (
    [MaNguoiDung] INT NOT NULL,
    MaChienDich INT NOT NULL,
    NgayLuu DATETIME DEFAULT GETDATE(),

    CONSTRAINT PK_User_ChienDich PRIMARY KEY ([MaNguoiDung], MaChienDich),
    FOREIGN KEY ([MaNguoiDung]) REFERENCES NguoiDung([MaNguoiDung]),
    FOREIGN KEY (MaChienDich) REFERENCES Chiendich(MaChienDich)
);
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

--
INSERT INTO VAITRO (TenVaiTro, MoTa)
VALUES 
('User', N'Người dùng thông thường'),
('Admin', N'Người có quyền quản trị hệ thống');


INSERT INTO DONGGOP(SoTien, HinhThuc, NoiDung, MaNguoiDung, MaChienDich)
VALUES 
(1000000, N'VNPay', 'test', 10003,10001);

select * from CHIENDICH where MaChienDich = 10001




CREATE TRIGGER TRG_Update_Chiendich_After_DongGop
ON DONGGOP
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE CD
    SET CD.SoTienHienTai = CD.SoTienHienTai + 
        CAST(I.SoTien AS DECIMAL(18,0))
    FROM CHIENDICH CD
    INNER JOIN INSERTED I ON CD.MaChienDich = I.MaChienDich;

    UPDATE CD
    SET CD.TrangThai = N'Đã kết thúc'
    FROM CHIENDICH CD
    WHERE CD.SoTienHienTai >= CD.MucTieuQuyenGop
      AND CD.TrangThai != N'Đã kết thúc';
END;

CREATE TRIGGER TRG_Update_SoLuongSanPham_After_HoaDon
ON CHITIETHOADON
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE SP
    SET SP.SoLuongTon = SP.SoLuongTon - CT.SoLuong
    FROM SANPHAM SP
    INNER JOIN INSERTED CT ON SP.MaSanPham = CT.MaSanPham;

    -- Cập nhật trạng thái nếu hết hàng
    UPDATE SANPHAM
    SET TrangThai = N'Hết hàng'
    WHERE SoLuongTon <= 0;
END;

CREATE TRIGGER TRG_Convert_TienThanhDiem
ON HOADON
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ND
    SET ND.DiemThuong = ISNULL(ND.DiemThuong, 0) + (I.TongTien * 0.1)
    FROM NGUOIDUNG ND
    INNER JOIN INSERTED I ON ND.MaNguoiDung = I.MaNguoiDung;
END;

ALTER TABLE DONGGOP ADD LoaiDongGop NVARCHAR(10) DEFAULT 'TIEN';
GO

CREATE TRIGGER TRG_Tru_Diem_NguoiDung
ON DONGGOP
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ND
    SET ND.DiemThuong = ND.DiemThuong - I.SoTien
    FROM NGUOIDUNG ND
    INNER JOIN INSERTED I ON ND.MaNguoiDung = I.MaNguoiDung
    WHERE I.HinhThuc = 'DIEM';
END;

CREATE TRIGGER TRG_UpdateTongSoLuong_LoaiSP
ON SANPHAM
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE LSP
    SET LSP.TongSoLuong = LSP.TongSoLuong + 1
    FROM LOAISANPHAM LSP
    INNER JOIN INSERTED I ON LSP.MaLoaiSanPham = I.MaLoaiSanPham;
END;

-- Đồ thủ công - MaLoaiSanPham = 10001
INSERT INTO SANPHAM (TenSanPham, MoTa, SoLuongTon, GiaBan, TrangThai, NgayDang, HinhAnhDaiDien, MaLoaiSanPham) VALUES
(N'Giỏ mây tre đan', N'Giỏ đựng đồ làm thủ công từ mây tre truyền thống.', 10, 150000, N'Còn hàng', GETDATE(), N'https://bizweb.dktcdn.net/thumb/1024x1024/100/401/675/products/gio-may-ac-dan-thua.jpg?v=1668129323490', 10001),
(N'Túi xách vải bố', N'Túi thủ công bằng vải bố, thân thiện với môi trường.', 5, 120000, N'Còn hàng', GETDATE(), N'https://grepacobags.com/wp-content/uploads/2013/06/t%C3%BAi-tr%C6%A1n-600x565.jpg', 10001)
-- Phụ kiện - MaLoaiSanPham = 10002
INSERT INTO SANPHAM (TenSanPham, MoTa, SoLuongTon, GiaBan, TrangThai, NgayDang, HinhAnhDaiDien, MaLoaiSanPham) VALUES
(N'Vòng tay gỗ', N'Vòng tay làm từ gỗ tự nhiên, nhẹ và bền.', 20, 65000, N'Còn hàng', GETDATE(), N'https://bizweb.dktcdn.net/thumb/1024x1024/100/297/813/files/dsc00641.jpg?v=1692353589931', 10002),
(N'Kẹp tóc thủ công', N'Kẹp tóc bằng vải nỉ dễ thương.', 10, 40000, N'Còn hàng', GETDATE(), N'https://chus.vn/images/detailed/263/10760_02_F1.jpg', 10002)
-- Quần áo - MaLoaiSanPham = 10004
INSERT INTO SANPHAM (TenSanPham, MoTa, SoLuongTon, GiaBan, TrangThai, NgayDang, HinhAnhDaiDien, MaLoaiSanPham) VALUES
(N'Áo thun cotton', N'Áo thun làm từ vải cotton 100%, thoáng mát.', 30, 90000, N'Còn hàng', GETDATE(), N'https://lados.vn/wp-content/uploads/2024/07/120af8aefd7c6428a28edb526b91549d-1694508654424.jpeg', 10004),
(N'Váy hoa nhí', N'Váy bé gái hoa nhí nhẹ nhàng và dễ thương.', 8, 140000, N'Còn hàng', GETDATE(), N'https://lamia.com.vn/storage/anh-seo/vay-hoa-nhi-3.jpg', 10004);

INSERT INTO TRUONG (TenTruong, DiaChi, KhuVuc, GhiChu, ViDo, KinhDo) VALUES
(N'Trường Tiểu học Bình Minh', N'123 Đường Lê Lợi, Q.1, TP.HCM', N'TP.HCM', N'Trường vùng nội thành', '10.7769', '106.7009'),
(N'Trường THCS Hòa Bình', N'45 Nguyễn Trãi, Q.5, TP.HCM', N'TP.HCM', N'Trường trung học cơ sở', '10.7620', '106.6825'),
(N'Trường Tiểu học Nguyễn Văn Trỗi', N'75 Trần Hưng Đạo, Q.1, TP.HCM', N'TP.HCM', N'Trường tiểu học mẫu', '10.7735', '106.7062'),
(N'Trường Tiểu học D Phú Hữu', N'Ấp Phú Thành, Xã Phú Hữu, Huyện An Phú, An Giang', N'An Giang', N'Điểm trường vùng biên giới, học sinh Campuchia‑gốc Việt, điều kiện khó khăn', '10.54083', '104.97306'),
(N'Trường Tiểu học B Nhơn Hưng', N'Xã Nhơn Hưng, Huyện Tịnh Biên, An Giang', N'An Giang', N'Vùng dân cư thưa, điểm trường xuống cấp được xây mới', '10.63191', '104.99611'),
(N'Trường Tiểu học Kim Đồng', N'210 Cách Mạng Tháng 8, Q.10, TP.HCM', N'TP.HCM', N'Trường tiểu học lớn', '10.7762', '106.6679'),
(N'Trường THPT Lê Quý Đôn', N'110 Nguyễn Thị Minh Khai, Q.3, TP.HCM', N'TP.HCM', N'Trường trung học phổ thông chất lượng cao', '10.7753', '106.6895'),
(N'Trường THCS Nguyễn Du', N'5 Tôn Thất Tùng, Q.1, TP.HCM', N'TP.HCM', N'Trường trung học nổi tiếng', '10.7702', '106.6930'),
(N'Trường Tiểu học Trần Quốc Toản', N'78 Nguyễn Thái Sơn, Gò Vấp, TP.HCM', N'TP.HCM', N'Trường tiểu học ngoại ô', '10.8231', '106.6823'),
(N'Trường THPT Gia Định', N'111 Lê Văn Duyệt, Bình Thạnh, TP.HCM', N'TP.HCM', N'Trường THPT danh tiếng', '10.8024', '106.6966'),
(N'Trường THCS Lý Thường Kiệt', N'90 Lý Thường Kiệt, Q.Tân Bình, TP.HCM', N'TP.HCM', N'Trường THCS gần khu công nghiệp', '10.7925', '106.6527'),
(N'Trường Tiểu học Lê Văn Tám', N'15 Nguyễn Văn Trỗi, Phú Nhuận, TP.HCM', N'TP.HCM', N'Trường tiểu học khu dân cư', '10.7992', '106.6820'),
(N'THCS Liên Sơn', N'Xã Liên Sơn, Huyện Chi Lăng, Tỉnh Lạng Sơn', N'Lạng Sơn', N'Điểm trường lẻ vùng cao, giao thông khó khăn', '21.65278', '106.47139'),
(N'Mầm non Chi Lăng', N'Xã Vân An, Huyện Chi Lăng, Tỉnh Lạng Sơn', N'Lạng Sơn', N'Mầm non vùng cao, phụ huynh khó tiếp cận', '21.65278', '106.47139'),
(N'Tiểu học – THCS Đội Cấn', N'Xã Cán Tỷ, Huyện Quản Bạ, Hà Giang', N'Hà Giang', N'Trường DT bán trú, nhận hỗ trợ gạo', '23.10194', '105.04361');
