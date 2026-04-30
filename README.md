# Подготовка проекта

- очистка начального проекта
- настроить горячие клавиши Shift + Space на лампочку
- расширение CSharpier и использовать форматирование по Shift + Alt + F
- сборка должна быть чистой без ошибок и предупреждений
- удалить сразу стандартный пакет OpenApi
- проверить установку dotnet-ef утилиты
- сразу создавать свои маперы под свои dto


## Моделирование

- сразу определиться с английскими названиями
- быть готовым работать с DateTime на уровне PostgreSQL
- сразу определиться использовать перечисления с выносом в дополнительные таблицы и настройкой сериализации из числа в строку
- порядок именования поля DateTime: суффикс Date в конце

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
