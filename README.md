# TruongSach - Đồ án Lập Trình Web

**TruongSach** là một nền tảng web hỗ trợ hoạt động thiện nguyện trong lĩnh vực giáo dục, cho phép người dùng:

- **Mua sắm sản phẩm** để tích lũy điểm thưởng (10% giá trị đơn hàng).
- **Quyên góp** điểm thưởng hoặc tiền mặt cho các **chiến dịch xây dựng trường học**.
- **Đăng tải bài viết, chiến dịch** lên cộng đồng để lan tỏa và kêu gọi đóng góp.
- **Tương tác cộng đồng** thông qua like, bình luận.
- **Nhận hỗ trợ từ chatbot AI 24/7** và đảm bảo nội dung được kiểm duyệt trước khi hiển thị.

## Các chức năng chính

### Front-end
- **Trang chủ**: Giới thiệu tổng quan dự án.
- **Charity/Campaign**: Hiển thị các chiến dịch – danh mục, danh sách, trạng thái, quyên góp.
- **Shop**: Trưng bày sản phẩm, chi tiết sản phẩm, giỏ hàng, thanh toán.
- **Đóng Góp**: Hiển thị chiến dịch cần quyên góp, chọn tiền, lời nhắn, thanh toán.
- **Login/Register**: Đăng nhập và đăng ký tài khoản.
- **Profile**: Trang cá nhân người dùng.

### Back-end
- **Tích điểm & tạo niềm tin**: Khi user mua sắm, sẽ nhận được 10% giá trị đơn hàng quy đổi thành điểm.
- **Thanh toán chiến dịch**: User có thể donate bằng tiền hoặc bằng điểm tích lũy.
- **Chatbot AI 24/7**: Hỗ trợ người dùng mọi lúc qua chatbot tích hợp AI.
- **Quản lý Chiến Dịch**: CRUD, tìm kiếm.
- **Quản lý Sản Phẩm**: CRUD, tìm kiếm.
- **Quản lý User**: CRUD, tìm kiếm.
- **Thống kê Dashboard**: Thống kê người dùng, đơn hàng, doanh số.

---

## Hướng dẫn thiết lập Google OAuth

Để sử dụng tính năng đăng nhập Google, mở tệp:

```
D:\Dev\Project\SRC_TruongSach\TuanAnhBacDatSang_DoAnWeb\appsettings.json
```

Và thêm đoạn sau vào:

```json
"GoogleKeys": {
  "ClientId": "",
  "ClientSecret": ""
}
```

Thay `ClientId` và `ClientSecret` bằng thông tin ứng dụng từ Google Cloud Console.

---
