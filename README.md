# Подготовка проекта

- очистка начального проекта
- настроить горячие клавиши Shift + Space на лампочку
- расширение CSharpier и использовать форматирование по Shift + Alt + F
- сборка должна быть чистой без ошибок и предупреждений
- удалить сразу стандартный пакет OpenApi
- проверить установку dotnet-ef утилиты
- сразу создавать свои маперы под свои dto


## Моделирование

- если есть импорт, то проанализировать названия файлов и содержимого. Так можно понять какие модели и атрибуты нужны
- сразу определиться с английскими названиями
- быть готовым работать с DateTime на уровне PostgreSQL
- сразу определиться использовать перечисления с выносом в дополнительные таблицы и настройкой сериализации из числа в строку
- порядок именования поля DateTime: суффикс Date в конце
- в Excel файлах сделать автоподбор ширины столбца по содержимому
- значение по умолчанию в postgres для типа uuid: `gen_random_uuid()`

# Импорт данных

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

        var data = await File.ReadAllTextAsync("resources/products/products.json");
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

## Импорт данных из Excel в Dbeaver

- для импорта данных надо подготовить данные в Excel по столбцам в строгом соответствии так, как они идут в базе (перемещение столбцов зажать Shift и навести между столбцами)
- проставить в Excel внешние ключи вместо явных строк
- вставить в Dbeaver командой `Ctrl + Shift + V` и не забыть потом сохранить `Ctrl + S`
- смотреть в данных на первичные ключи: int Id или Guid Id
- методика вставки: сначала создать n - записей, потом выделить все строки без столбца Id и потом Ctrl + Shift + V и сохранить

## Восстановление базы данных с данными

- после того, как данные будут импортированые сделай dump с данными средствами Dbeaver (База данных - Задачи - Создать задачу)
- в параметрах указать формат Plain, кодировка UTF-8 и поставить Insert Into (первый чекбокс). Это надо для тебя, хотя в задании и критериях этого не требуется
- в Dbeaver сделать полную ERD-диаграмму (ее потом используешь в презентации)
- лучше constraint через чистый sql или через dbeaver
- коммиты ОБЯЗАТЕЛЬНО делать осмысленными (feature: новая функция, fix: исправил проблему)
- заполнять и оформлять по структуре README
 - добавил enum в моделях, в базе они будут храниться как int. При выводе в API можно настроить как числом, так и строкой. При таком подходе в коде становиться легче ориентироваться


# CRUD

- создать базовый контроллер CRUD через generic
- копировать на основе одного
- с учетом, что контроллеры могут быть не простыми, а с моделями или dto, то generic могут не очень помочь, потому что будут простыми

### Настройка Swagger

```xml
  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="10.1.7" />
    <PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="10.1.7" />
    <PackageReference Include="Swashbuckle.AspNetCore.Swagger" Version="10.1.7" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="10.1.7" />
  </ItemGroup>
```


### Хеширование пароля

```Csharp
    /// <summary>
    /// Хеширует пароль с автоматической генерацией соли
    /// </summary>
    public string HashPassword(string password)
    {
        // WorkFactor: 12 - хороший баланс между безопасностью и производительностью
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }
    
    /// <summary>
    /// Проверяет пароль
    /// </summary>
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
```

## Минимальная настройка jwt

- ключ в appsettings 
- "tokenKey": "MySuperStrongPassword_12345678910!". Должен быть от 256 бит длиной, иначе получим исключение
- на верхнем уровне формирование объекта ключа

```Csharp
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["tokenKey"]!));
```

- пакеты, версии надо контролировать

```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.7" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.17.0" />
  </ItemGroup>
```

- настройка jwt-аутентификации в builder

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
- можно всегда отредактировать миграцию до ее выполнения

# Enum

- вместо string выносим в enum
- на уровне базы данных там будет int
- на уровне кода удобней работать


# Обработка ошибок глобально в связке с Result Pattern или любой моделью ответа

- первый в конвейере

Program.cs
```Csharp
app.UseMiddleware<ExceptionHandlerMiddleware>();
```

- middleware
- обрабатывать все возможные исключения, а статус коды проставлять в модели ответа
- убираем все try-catch из приложения

```Csharp
public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var message = "";

        try
        {
            await next(context);
        }
        catch (System.Exception ex)
        {
            switch (ex)
            {
                case Exception:
                    message = ex.Message;
                    break;
            }

            context.Response.ContentType = "application/json";
            var model = Result<string>.Failure(message);
            await context.Response.WriteAsJsonAsync(model);
        }
    }
}
```