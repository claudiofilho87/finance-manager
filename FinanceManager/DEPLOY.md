# Deploy em Produção - Finance Manager

## 📋 Pré-requisitos

- Docker e Docker Compose instalados no servidor
- Acesso SSH ao servidor
- Domínio configurado (opcional)

## 🚀 Passo a Passo

### 1. Clone o repositório no servidor

```bash
git clone <seu-repositorio>
cd FinanceManager
```

### 2. Configure as variáveis de ambiente

```bash
# Copie o arquivo de exemplo
cp .env.docker.example .env

# Edite com suas credenciais de produção
nano .env
```

**⚠️ IMPORTANTE:** Gere um JWT Token seguro:

```bash
# Gerar token aleatório de 64 caracteres
openssl rand -base64 64 | tr -d '\n'
```

### 3. Configure o arquivo .env

```env
DB_USER=postgres
DB_PASSWORD=SuaSenhaSegura123!@#
JWT_SECRET_TOKEN=seu_token_gerado_com_openssl_acima
```

### 4. Inicie os containers

```bash
# Build e start
docker-compose -f docker-compose.production.yml up -d --build

# Verificar status
docker-compose -f docker-compose.production.yml ps

# Ver logs
docker-compose -f docker-compose.production.yml logs -f backend
```

### 5. Verifique se está funcionando

```bash
# Health check do backend
curl http://localhost:5050/health

# Health check do banco
docker-compose -f docker-compose.production.yml exec db pg_isready -U postgres
```

## 📂 Localização dos Dados

### Logs da Aplicação
Os logs ficam salvos em um **volume Docker**:

```bash
# Ver localização exata do volume
docker volume inspect financemanager_backend-logs

# Acessar logs diretamente
docker-compose -f docker-compose.production.yml exec backend ls -la /app/logs

# Copiar logs para o host
docker cp financemanager-backend:/app/Logs ./logs-backup
```

### Banco de Dados
Dados do PostgreSQL:

```bash
# Localização do volume
docker volume inspect financemanager_postgres-data

# Backup do banco
docker-compose -f docker-compose.production.yml exec db pg_dump -U postgres FinanceManagerDB > backup.sql

# Restore do banco
docker-compose -f docker-compose.production.yml exec -T db psql -U postgres FinanceManagerDB < backup.sql
```

## 🔄 Atualizações

### Para atualizar a aplicação:

```bash
# 1. Baixar últimas mudanças
git pull

# 2. Rebuild e restart
docker-compose -f docker-compose.production.yml up -d --build

# 3. Verificar logs
docker-compose -f docker-compose.production.yml logs -f backend
```

## 🛑 Parar os serviços

```bash
# Parar containers (mantém volumes)
docker-compose -f docker-compose.production.yml stop

# Parar e remover containers (mantém volumes)
docker-compose -f docker-compose.production.yml down

# Remover TUDO (incluindo volumes - cuidado!)
docker-compose -f docker-compose.production.yml down -v
```

## 📊 Monitoramento de Logs

### Ver logs em tempo real:

```bash
# Todos os serviços
docker-compose -f docker-compose.production.yml logs -f

# Apenas backend
docker-compose -f docker-compose.production.yml logs -f backend

# Últimas 100 linhas
docker-compose -f docker-compose.production.yml logs --tail=100 backend
```

### Acessar logs salvos no volume:

```bash
# Entrar no container
docker-compose -f docker-compose.production.yml exec backend bash

# Listar arquivos de log
ls -lh /app/logs

# Ver log do dia atual
tail -f /app/logs/finance-manager-$(date +%Y%m%d).log
```

## 🔒 Segurança

### Checklist de Segurança:

- [ ] JWT Token com no mínimo 64 caracteres aleatórios
- [ ] Senha forte do banco de dados
- [ ] Arquivo `.env` com permissões restritas: `chmod 600 .env`
- [ ] `.env` NÃO está no git
- [ ] HTTPS configurado (usar Nginx ou Traefik como reverse proxy)
- [ ] Firewall configurado (apenas portas necessárias abertas)
- [ ] Backups automáticos do banco configurados

### Configurar permissões do .env:

```bash
chmod 600 .env
```

## 🌐 Configurar HTTPS (Opcional mas Recomendado)

Use **Nginx** ou **Traefik** como reverse proxy com Let's Encrypt:

```bash
# Exemplo com Certbot
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d seudominio.com
```

## 📈 Recursos dos Containers

### Ver uso de recursos:

```bash
docker stats financemanager-backend financemanager-db
```

## ❓ Troubleshooting

### Backend não conecta no banco:

```bash
# Verificar se o banco está rodando
docker-compose -f docker-compose.production.yml ps db

# Testar conexão
docker-compose -f docker-compose.production.yml exec backend ping financemanager-db
```

### Ver variáveis de ambiente do container:

```bash
docker-compose -f docker-compose.production.yml exec backend env | grep -E 'ConnectionStrings|Jwt'
```

### Logs de erro:

```bash
docker-compose -f docker-compose.production.yml logs backend | grep -i error
```
