# Проверка VSCode

- настройка `Hot-Reload` при сохранениии для flutter
- проверка в `File -> Autosave`
- расширение CSharpier и использовать форматирование по Shift + Alt + F
- настроить горячие клавиши `Shift + Space` на лампочку
- установка расширений для .NET и Flutter
- могут слетать расширения и не показываться ошибки, следить за этим

# Проверка .NET

- все проверялось на .NET 8
- проверка версии `dotnet --version`
- проверить установку `dotnet-ef` утилиты

# Подготовка проекта API

- удалить сразу стандартный пакет OpenApi
- очистка начального проекта
- сборка должна быть чистой без ошибок и предупреждений

# По архитектуре 

- сразу создавать свои маперы под свои dto
- можно выносить enum в отдельную папку, но лучше держать их вместе с моделями для лучшей ориентации

## Моделирование предметной области

- сразу определиться с английскими названиями
- добавил enum в моделях, в базе они будут храниться как int. При выводе в API можно настроить как числом, так и строкой. При таком подходе в коде становиться легче ориентироваться
- порядок именования полей с типом DateTime: суффикс `Date` в конце
- если придется работать с GUID в качестве первичного ключа. Значение по умолчанию в postgres для типа uuid: `gen_random_uuid()`
- лучше constraint через чистый sql или через dbeaver

# Импорт данных

- если есть ресурсы для импорта, то проанализировать названия файлов и содержимого. Так можно понять какие модели и атрибуты нужны
- в Excel файлах сделать автоподбор ширины столбца по содержимому

## Парсинг json

```Csharp
var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, // или SnakeCaseUpper
    PropertyNameCaseInsensitive = true,
};

app.MapGet(
    "/parsing-products",
    async (AppDbContext db) =>
    {

        var data = await File.ReadAllTextAsync("Resources/products.json");
        var products = JsonSerializer.Deserialize<Product[]>(data, options);
        Console.WriteLine(products?.Count());

        try
        {
            await db.Products.AddRangeAsync(products!);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.InnerException!.Message}");
        }
    }
);
```
- обязательно настроить копирование в сборку в `csproj`

```xml
  <!-- Копирование в сборку bin -->
  <ItemGroup>
    <None Update="Resources\**\*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
```



## Импорт данных из Excel в Dbeaver

- для импорта данных надо подготовить данные в Excel по столбцам в строгом соответствии так, как они идут в базе (перемещение столбцов зажать Shift и навести между столбцами)
- проставить в Excel внешние ключи вместо явных строковых значений
- вставить в Dbeaver командой `Ctrl + Shift + V` и не забыть потом сохранить `Ctrl + S`
- смотреть в данных на тип первичного ключа: int Id или Guid Id
- методика вставки в строку не целиком: сначала создать n - записей, потом выделить все строки без столбца Id и потом Ctrl + Shift + V и сохранить

## Восстановление базы данных с данными

- после того, как данные будут импортированые сделать `dump` с данными средствами Dbeaver (База данных - Задачи - Создать задачу)
- в параметрах указать формат Plain, кодировка UTF-8 и поставить Insert Into (первый чекбокс). Это надо для тебя, хотя в задании и критериях этого не требуется

# Презентация

- в Dbeaver сделать полную ERD-диаграмму (ее потом используешь в презентации)

# Git

- коммиты ОБЯЗАТЕЛЬНО делать осмысленными (feature: новая функция, fix: исправил проблему)
- заполнять и оформлять по структуре README


# CRUD через generic

- создать базовый контроллер CRUD через generic
- копировать на основе одного
- с учетом, что контроллеры могут быть не простыми, а с моделями или dto, то generic могут не очень помочь, потому что будут простыми
- но можно реализовать generic контроллер в день -1

# Проблемы с датой DateTime при выводе из PostgresSQL

- заменить `DateTime.Now` на `DateTime.Utc.Now`
- проверить что в базе тип данные date time with time zone
- статичекий конструктор в контексте со старым поведением


# Настройка Swagger

- пакеты

```xml
  <!-- Swagger -->
  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="10.1.7" />
    <PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="10.1.7" />
    <PackageReference Include="Swashbuckle.AspNetCore.Swagger" Version="10.1.7" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="10.1.7" />
  </ItemGroup>
```

## Подставление jwt

- в новой версии надо подставлять только токен, без добавления Bearer, он подставиться автоматически

```Csharp
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "bearer" }
    );

    o.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
    });
});
```

## Аннотации Swagger

```Csharp
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo());
    o.EnableAnnotations();
});
```
- после 10 точкек трудно ориентироваться, поэтому обязательно подписывать

- на каждую конечную точку атрибут
```Csharp
[SwaggerOperation(Summary = "Получение списка")]
```


### Хеширование пароля

```Csharp
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
```

## Минимальная настройка jwt

- ключ в `appsettings.json`

- Должен быть от 256 бит, поэтому примерно запомнить длину, иначе получим исключение
  
```json 
"tokenKey": "MySuperStrongPassword_12345678910!".  
```

- на верхнем уровне `Program.cs` создаем объект ключа

```Csharp
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["tokenKey"]!));
```

- пакеты для Jwt

```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.7" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.17.0" />
  </ItemGroup>
```

- настройка jwt-аутентификации в builder. Минимальная конфигурация

```csharp
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ValidateIssuerSigningKey = true,
            
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
```

- быстрое формирование токена, можно сделать отдельный сервис или метод
- создаем утверждения, из записываем в токен, токен передаем в метод

```Csharp
app.MapGet(
    "/token",
    () =>
    {
        // утверждения
        var claims = new[] { new Claim(ClaimTypes.Name, "test") };

        // токен
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Results.Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
);
```

# Каскадное удаление

- применяется по дефолту `Cascade`, если в модели прописать внешний ключ и навигационное свойство в паре
- всегда редактируем миграцию до ее выполнения, особенно первую. Проверяем, что типы данных заданы неизбыточно, делаем ограничение типов по смыслу


# Обработка ошибок глобально в связке с Result Pattern или любой моделью ответа

- middleware
- обрабатывать все возможные исключения, а статус коды проставлять в модели ответа
- убираем все try-catch из приложения

```Csharp
public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var message = "An internal server error occurred";
        int statusCode = StatusCodes.Status500InternalServerError;

        try
        {
            await next(context);
        }
        catch (System.Exception ex)
        {
            switch (ex)
            {
                case ArgumentException
                or ArgumentNullException
                or InvalidOperationException
                or ParseException:
                    message = ex.Message;
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case Exception:
                    message = $"{ex.GetType().FullName} {ex.Message}";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var model = Result<string>.Failure(message);
            await context.Response.WriteAsJsonAsync(model);
        }
    }
}
```

- подключение, первый в конвейере

```Csharp
app.UseMiddleware<ExceptionHandlerMiddleware>();
```

# Поиск, фильтрация, пагинация, сортировка

- создаем модель `Option`


```Csharp
public class Option
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;

    public string? Search { get; set; } = string.Empty;
    public string? Filter { get; set; } = string.Empty;
    public required string? Sort { get; set; }
    public required string? SortDirection { get; set; } = "asc";
}
```

- главная конечная точка

```Csharp
public ActionResult<ApiResult<UserDto>> GetUsers([FromQuery] Option opt)
    {
        var users = db.Users.AsQueryable();

        if (opt.Search != string.Empty)
            users = users.Where(u => u.Login.Contains(opt.Search!));

        if (opt.Filter != string.Empty)
            users = users.Where(u => u.Login == opt.Filter);

        if (opt.Sort != string.Empty && opt.SortDirection != string.Empty)
            users = users.OrderBy($"{opt.Sort!} {opt.SortDirection}");

        users = users.Skip((opt.PageNumber - 1) * opt.PageSize).Take(opt.PageSize);

        return Ok(
            new ApiResult<UserDto>()
            {
                Success = true,
                PageNumber = opt.PageNumber,
                PageSize = opt.PageSize,
                Count = db.Users.Count(),
                Data = users.Select(u => u.ToDto()).ToList(),
            }
        );
    }

```

- теперь при выводе всех элементов будет выводиться специальная модель, которая будет содержать служебную информацию и данные

```Csharp
public class ApiResult<T>
    where T : class
{
    public bool Success { get; set; }
    public required int Count { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages
    {
        get => (int)Math.Ceiling(Count / (double)PageSize);
    }

    public bool HasNext
    {
        get => PageNumber < TotalPages;
    }

    public bool HasPreview
    {
        get => PageNumber > 1;
    }
    public required List<T> Data { get; set; }
}
```

- для работы сортировки динамически надо установить специальный пакет
`System.Linq.Dynamic.Core` обязательно

- в результате получаем запрос в полном виде со всеми параметрами. Каждый параметр можно убирать или добавлять
- `http://localhost:5128/api/Users/option?PageSize=10&PageNumber=1&Search=User&Filter=User1&Sort=Id&SortDirection=Asc`
- на клиенте используем данную точку для гибкого управления данными


# Экспорт в csv

- пакеты

```xml
  <ItemGroup>
    <PackageReference Include="Magicodes.IE.Core" Version="2.8.2" />
    <PackageReference Include="Magicodes.IE.Csv" Version="2.8.2" />
    <PackageReference Include="Magicodes.IE.Html" Version="2.8.2" />
    <PackageReference Include="Magicodes.IE.Pdf" Version="2.8.2" />
    <PackageReference Include="SixLabors.ImageSharp" Version="3.1.12" />
  </ItemGroup>
```


```Csharp
    [HttpGet("export/csv")]
    [SwaggerOperation(Summary = "Экспорт в csv")]
    public async Task<FileContentResult> ExportToCsv()
    {
        var data = db.Set<User>().ToList();
        var csvExporter = new CsvExporter();
        var csvBytes = await csvExporter.ExportAsByteArray(data); // Возвращает byte[]
        return File(csvBytes, $"text/csv", $"export_{nameof(User)}.csv");
    }
```

# Экспорт в html

```Csharp
    [HttpGet("export/html")]
    [SwaggerOperation(Summary = "Экспорт в html")]
    public async Task<ContentResult> ExportToHtml()
    {
        var data = db.Set<User>().ToList();
        var htmlExporter = new HtmlExporter();
        var htmlString = await htmlExporter.ExportListByTemplate(data); // Возвращает string (HTML-код)
        return Content(htmlString, "text/html");
    }
```

# Экспорт в pdf

```Csharp
    [HttpGet("export/pdf")]
    [SwaggerOperation(Summary = "Экспорт в pdf")]
    public async Task<FileContentResult> ExportToPdf()
    {
        var data = db.Set<User>().ToList();
        var pdfExporter = new PdfExporter();
        var pdfBytes = await pdfExporter.ExportListBytesByTemplate(data, ""); // Возвращает byte[]
        return File(pdfBytes, "application/pdf", "export.pdf");
    }
```


# Настройка SignalR в API

- создаем папку Hubs и хаб VendingHub

```Csharp
public class VendingHub : Hub
{
    public async Task SendVendingUpdate(VendingMachine machine)
    {
        await Clients.All.SendAsync("VendingUpdated", machine);
    }
}
```

```Csharp
builder.Services.AddSignalR();

app.MapHub<VendingHub>("/vendingHub");
```

- передаем хаб через конструктор `IHubContext<VendingHub> hub`
- отправляем сообщение всем клиентам, которые установили соединение с хабом
- внимание метод должен быть асинхронным

```Csharp
    [HttpGet]
    [SwaggerOperation(Summary = "Получение списка пользователей")]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        await hub.Clients.All.SendAsync("MachineUpdated", db.Users.ToList());
        return Ok(db.Users.Select(u => u.ToDto()));
    }
```


