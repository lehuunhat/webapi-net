using System.Globalization;
using HienTangToc.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// C?u hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:5500", "http://127.0.0.1:5500") // Thay ??i URL này thành ngu?n g?c c?a trang web c?a b?n
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Add services to the DI container
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<NguoiMuonService>();
builder.Services.AddScoped<NhanVienSevice>();
builder.Services.AddScoped<PhongBanService>();
builder.Services.AddScoped<HopDongLaoDongService>();
builder.Services.AddScoped<HocVanService>();
builder.Services.AddScoped<NghiPhepService>();
builder.Services.AddScoped<KhachHangSevice>();
builder.Services.AddScoped<HoaDonService>();
builder.Services.AddScoped<ChiTietHoaDonService>();
builder.Services.AddScoped<GioHangService>();
builder.Services.AddScoped<BinhLuanService>();
builder.Services.AddScoped<KhuyenMaiService>();

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseStaticFiles(); // Ph?c v? các t?p t?nh t? wwwroot

// Thêm middleware ?? ph?c v? các t?p t? th? m?c data/images
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "data/images")),
    RequestPath = "/data/images"
});

app.UseAuthorization();
app.MapControllers();

app.Run();
