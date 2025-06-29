namespace TuanAnhBacDatSang_DoAnWeb.Models
{
    public class GioHang
    {
        public List<SanPhamTrongGioHang> Items { get; set; } = new List<SanPhamTrongGioHang>();
        public void AddItem(SanPhamTrongGioHang item)
        {
            var existingItem = Items.FirstOrDefault(i => i.MaSanPham ==
           item.MaSanPham);
            if (existingItem != null)
            {
                existingItem.SoLuong += item.SoLuong;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.MaSanPham == productId);
        }
    }
}
