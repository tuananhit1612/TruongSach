using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TruongSach_API.DTOs;
using TruongSach_API.Models;
using TruongSach_API.Repositories;

namespace TruongSach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepository _repo;

        public HoaDonController(IHoaDonRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHoaDon([FromBody] HoaDonDTO dto)
        {
            var hoaDon = new Hoadon
            {
                NgayLap = DateTime.Now,
                TongTien = dto.TongTien,
                PhiVanChuyen = dto.PhiVanChuyen,
                ThueVat = dto.ThueVAT,
                HinhThucThanhToan = dto.HinhThucThanhToan,
                TrangThai = dto.TrangThai,
                MaNguoiDung = dto.MaNguoiDung,
                DiaChi = dto.DiaChi
            };

            var chiTietList = dto.ChiTiet?.Select(c => new Chitiethoadon
            {
                MaSanPham = c.MaSanPham,
                SoLuong = c.SoLuong
            }).ToList() ?? new();

            var result = await _repo.AddAsync(hoaDon, chiTietList);


            return Ok(result.MaHoaDon);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var list = await _repo.GetAllByUserIdAsync(userId);
            var result = list.Select(hd => new HoaDonDTO_Output
            {
                MaHoaDon = hd.MaHoaDon,
                NgayLap = hd.NgayLap,
                TongTien = hd.TongTien,
                PhiVanChuyen = hd.PhiVanChuyen,
                ThueVAT = hd.ThueVat,
                HinhThucThanhToan = hd.HinhThucThanhToan,
                TrangThai = hd.TrangThai,
                MaNguoiDung = hd.MaNguoiDung,
                DiaChi = hd.DiaChi,
                ChiTiet = hd.Chitiethoadons.Select(c => new ChiTietHoaDonDTO_Output
                {
                    DonGia = c.MaSanPhamNavigation.GiaBan,
                    TenSanPham = c.MaSanPhamNavigation.TenSanPham,
                    MaSanPham = c.MaSanPham,
                    SoLuong = c.SoLuong
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hd = await _repo.GetByIdAsync(id);
            if (hd == null) return NotFound();

            var dto = new HoaDonDTO_Output
            {
                MaHoaDon = hd.MaHoaDon,
                NgayLap = hd.NgayLap,
                TongTien = hd.TongTien,
                PhiVanChuyen = hd.PhiVanChuyen,
                ThueVAT = hd.ThueVat,
                HinhThucThanhToan = hd.HinhThucThanhToan,
                TrangThai = hd.TrangThai,
                MaNguoiDung = hd.MaNguoiDung,
                DiaChi = hd.DiaChi,
                ChiTiet = hd.Chitiethoadons.Select(c => new ChiTietHoaDonDTO_Output
                {
                    DonGia = c.MaSanPhamNavigation.GiaBan,
                    TenSanPham = c.MaSanPhamNavigation.TenSanPham,
                    MaSanPham = c.MaSanPham,
                    SoLuong = c.SoLuong
                }).ToList()
            };

            return Ok(dto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHoaDon(int id, [FromBody] HoaDonDTO dto)
        {
            var existingHoaDon = await _repo.GetByIdAsync(id);
            if (existingHoaDon == null)
                return NotFound();

            existingHoaDon.NgayLap = dto.NgayLap ?? existingHoaDon.NgayLap;
            existingHoaDon.TongTien = dto.TongTien ?? existingHoaDon.TongTien;
            existingHoaDon.PhiVanChuyen = dto.PhiVanChuyen ?? existingHoaDon.PhiVanChuyen;
            existingHoaDon.ThueVat = dto.ThueVAT ?? existingHoaDon.ThueVat;
            existingHoaDon.HinhThucThanhToan = dto.HinhThucThanhToan ?? existingHoaDon.HinhThucThanhToan;
            existingHoaDon.TrangThai = dto.TrangThai ?? existingHoaDon.TrangThai;
            existingHoaDon.MaNguoiDung = dto.MaNguoiDung;
            existingHoaDon.DiaChi = dto.DiaChi;

            if (dto.ChiTiet != null)
            {
                existingHoaDon.Chitiethoadons = dto.ChiTiet.Select(c => new Chitiethoadon
                {
                    MaHoaDon = existingHoaDon.MaHoaDon,
                    MaSanPham = c.MaSanPham,
                    SoLuong = c.SoLuong
                }).ToList();
            }

            await _repo.UpdateAsync(existingHoaDon);

            var response = new HoaDonDTO_Output
            {
                MaHoaDon = existingHoaDon.MaHoaDon,
                NgayLap = existingHoaDon.NgayLap,
                TongTien = existingHoaDon.TongTien,
                PhiVanChuyen = existingHoaDon.PhiVanChuyen,
                ThueVAT = existingHoaDon.ThueVat,
                HinhThucThanhToan = existingHoaDon.HinhThucThanhToan,
                TrangThai = existingHoaDon.TrangThai,
                MaNguoiDung = existingHoaDon.MaNguoiDung,
                DiaChi = existingHoaDon.DiaChi,
                ChiTiet = existingHoaDon.Chitiethoadons?.Select(c => new ChiTietHoaDonDTO_Output
                {
                    DonGia = c.MaSanPhamNavigation.GiaBan,
                    TenSanPham = c.MaSanPhamNavigation.TenSanPham,
                    MaSanPham = c.MaSanPham,
                    SoLuong = c.SoLuong
                }).ToList()
            };

            return Ok(response);
        }

    }

}
