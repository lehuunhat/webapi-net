using HienTangToc.Models;
using Microsoft.EntityFrameworkCore;

namespace HienTangToc.Data
{

    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> opt) : base(opt)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<NguoiMuonModel> NguoiMuon { get; set; }
        public DbSet<NguoiHienModel> NguoiHien { get; set; }
        public DbSet<SalonTocModel> SalonToc { get; set; }
        public DbSet<FormDKmuonModel> FormDKNM { get; set; }
        public DbSet<FormDKhienModel> FormDKNH { get; set; }
        public DbSet<FormDKsalonModel> FormDKSL { get; set; }

 /*       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HoaDon>()
                .HasMany(h => h.ChiTietHoaDons)
                .WithOne(ct => ct.HoaDon)
                .HasForeignKey(ct => ct.MaHD);
            modelBuilder.Entity<GioHang>()
               .HasMany(g => g.ChiTietGioHangs)
               .WithOne(ct => ct.GioHang)
               .HasForeignKey(ct => ct.GioHangId);


            modelBuilder.Entity<BinhLuan>()
                .HasOne(bl => bl.KhachHang)
                .WithMany(kh => kh.BinhLuans)
                .HasForeignKey(bl => bl.MaKH);

            modelBuilder.Entity<BinhLuan>()
                .HasOne(bl => bl.SanPham)
                .WithMany(sp => sp.BinhLuans)
                .HasForeignKey(bl => bl.MaSP);


            modelBuilder.Entity<KhuyenMai>()
                .HasMany(km => km.SanPhamModels)
                .WithMany(sp => sp.KhuyenMais)
                .UsingEntity<Dictionary<string, object>>(
                    "SanPhamKhuyenMai",
                    j => j
                        .HasOne<SanPhamModel>()
                        .WithMany()
                        .HasForeignKey("MaSP")
                        .HasConstraintName("FK_SanPhamKhuyenMai_SanPhamModel_MaSP")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<KhuyenMai>()
                        .WithMany()
                        .HasForeignKey("MaKM")
                        .HasConstraintName("FK_SanPhamKhuyenMai_KhuyenMai_MaKM")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }
 */
    }
}
