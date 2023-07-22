
using StoreApp.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // api kullancağımı belirtmek için

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); //razor pages kullanılabilmesi için

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSession();
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration();
builder.Services.ConfigureRouting(); // yurl küçük harf
builder.Services.ConfigureApplicationCookie();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseStaticFiles(); // wwwroot dosayasını kullanacağımı belirtmek için yazdım. Uygulama statik dosyalar kullanacak
app.UseSession(); //session kullan

app.UseHttpsRedirection();
app.UseRouting();

//oturum açma işlemleri yaspılıyorsa bu ifadeler routing ve endpoint arasında tanımlanır.
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(//Admin Areası için tanımlandı.
        name:"Admin",
        areaName:"Admin",
        pattern:"Admin/{controller=Dashboard}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages(); //razor pages kullanılabilmesi için

    endpoints.MapControllers(); // API için
});

app.MapGet("/btk", () => "BTK");

app.ConfigureAndCheckMigration();
app.ConfigureLocalization();
app.ConfigureDefaultAdminUser();
app.Run();