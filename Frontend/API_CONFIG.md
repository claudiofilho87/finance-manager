# Configuração da API

## URL da API Backend

A URL base da API está configurada no arquivo `src/services/api.ts`.

Por padrão, está configurada para: `http://localhost:5000`

### Para alterar a URL da API:

1. Abra o arquivo `src/services/api.ts`
2. Localize a linha:
   ```typescript
   const API_BASE_URL = 'http://localhost:5000';
   ```
3. Altere para a URL do seu backend .NET:
   ```typescript
   const API_BASE_URL = 'https://sua-api.com';
   ```

## Endpoints Utilizados

O frontend consome os seguintes endpoints da API:

### Autenticação
- `POST /api/v1/users/login` - Login
- `POST /api/v1/users/register` - Cadastro
- `POST /api/v1/users/refresh-token` - Refresh do token JWT

### Bills (Contas)
- `GET /api/v1/bills` - Lista todas as contas
- `GET /api/v1/bills/{id}` - Busca uma conta por ID
- `POST /api/v1/bills` - Cria nova conta
- `PUT /api/v1/bills/{id}` - Atualiza conta
- `DELETE /api/v1/bills/{id}` - Deleta conta

### Bill Ocorrences (Ocorrências)
- `GET /api/v1/bill/ocorrences` - Lista todas as ocorrências
- `GET /api/v1/bill/ocorrences/{id}` - Busca ocorrência por ID
- `POST /api/v1/bill/ocorrences` - Cria nova ocorrência
- `PUT /api/v1/bill/ocorrences/{id}` - Atualiza ocorrência
- `DELETE /api/v1/bill/ocorrences/{id}` - Deleta ocorrência

### Recurrence Types (Tipos de Recorrência)
- `GET /api/v1/recurrence-types` - Lista tipos de recorrência

## CORS

Certifique-se de que sua API .NET está configurada para aceitar requisições do frontend.

No arquivo `Program.cs`, adicione:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080") // URL do frontend
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// ...

app.UseCors("AllowFrontend");
```

## Autenticação JWT

O sistema utiliza JWT com refresh token:

1. Ao fazer login, o access token e refresh token são salvos no `localStorage`
2. O access token é enviado automaticamente em todas as requisições via interceptor do Axios
3. Quando o token expira (erro 401), o sistema tenta renovar automaticamente usando o refresh token
4. Se o refresh token também estiver inválido, o usuário é redirecionado para a tela de login

## Storage

Os seguintes dados são armazenados no `localStorage`:
- `accessToken` - Token JWT de acesso
- `refreshToken` - Token para renovação
- `tokenExpiresIn` - Tempo de expiração do token
