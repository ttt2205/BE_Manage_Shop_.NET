using Manage_Store.Data;
using Manage_Store.Services;
using Manage_Store.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Manage_Store.Middleware;
using Microsoft.AspNetCore.Mvc;
using Manage_Store.Responses;
using Microsoft.AspNetCore.Mvc;
using Manage_Store.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

// ✅ Ghi đè phản hồi mặc định khi ModelState không hợp lệ
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        // Lấy danh sách lỗi validation
        var errorMessages = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        // Ghép tất cả lỗi thành 1 chuỗi (ví dụ: "CategoryName is required, Price must be positive")
        var message = string.Join(", ", errorMessages);

        // Tạo phản hồi theo cấu trúc ApiResponse
        var response = ApiResponse<object>.Builder()
            .WithSuccess(false)
            .WithStatus(StatusCodes.Status400BadRequest)
            .WithMessage(message)
            .WithData(null)
            .Build();

        return new BadRequestObjectResult(response);
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    ));

// Đăng ký DI cho Service
builder.Services.AddScoped<IAuth, AuthImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<ICategoryService, CategoryImpl>();
builder.Services.AddScoped<IProductService, ProductImpl>();
builder.Services.AddScoped<IPromotionService, PromotionImpl>();
builder.Services.AddScoped<IOrderService, OrderImpl>();
builder.Services.AddScoped<IPaymentService, PaymentImpl>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
