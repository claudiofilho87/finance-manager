# Finance Manager

Gerencie suas finanças pessoais de forma simples e eficiente!  
Este projeto é composto por um backend em .NET, um frontend em React + Vite + TailwindCSS, e infraestrutura Docker para facilitar o desenvolvimento e o deploy.

---

## Estrutura do Projeto

```
proj-finance-manager/
├── Docker/           # Orquestração e infraestrutura (Docker, Nginx)
├── FinanceManager/   # Backend (.NET 8, WebAPI)
└── Frontend/         # Frontend (React, Vite, TailwindCSS)
```

---

## Tecnologias Utilizadas

- **Frontend:** React, Vite, TypeScript, TailwindCSS
- **Backend:** .NET 8, ASP.NET Core WebAPI, Entity Framework Core
- **Infraestrutura:** Docker, Docker Compose, Nginx

---

## Como rodar localmente

### Pré-requisitos

- [Docker](https://www.docker.com/)
- [Node.js](https://nodejs.org/) (opcional, para rodar o frontend fora do Docker)
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (opcional, para rodar o backend fora do Docker)

### 1. Configuração de variáveis de ambiente

Copie os arquivos de exemplo e ajuste conforme necessário:

```sh
cp Docker/.env.example Docker/.env
```

### 2. Subindo tudo com Docker

Na raiz do projeto, execute:

```sh
cd Docker
docker compose up --build
```

- O frontend estará disponível em: [http://localhost:3000](http://localhost:3000)
- O backend estará disponível em: [http://localhost:5000](http://localhost:5000) (ou conforme configuração)
- O Nginx pode ser usado como proxy reverso, conforme configurado.

### 3. Rodando manualmente (sem Docker)

#### Backend

```sh
cd FinanceManager
dotnet build
dotnet run
```

#### Frontend

```sh
cd Frontend
npm install
npm run dev
```

---

## Estrutura das Pastas

### Frontend

- **src/pages/**: Páginas principais (Login, Dashboard, Register, etc)
- **src/components/**: Componentes reutilizáveis (diálogos, UI, rotas protegidas)
- **src/contexts/**: Contextos React (ex: autenticação)
- **src/services/**: Serviços para comunicação com a API
- **src/types/**: Tipos TypeScript compartilhados
- **public/**: Arquivos estáticos

### Backend (FinanceManager)

- **Controllers/**: Controllers da API REST
- **DTOs/**: Data Transfer Objects
- **Models/**: Modelos de dados e enums
- **Services/**: Lógica de negócio e interfaces
- **Data/**: Contexto do Entity Framework
- **Migrations/**: Migrations do banco de dados
- **Logs/**: Logs de execução
- **Shared/**: Classes utilitárias e respostas padrão

### Docker

- **docker-compose.yaml**: Orquestração dos serviços
- **Backend/**: Dockerfile do backend
- **Frontend/**: Dockerfile do frontend
- **nginx/**: Configuração do Nginx

---

## Scripts Úteis

### Frontend

- `npm run dev` — inicia o servidor de desenvolvimento
- `npm run build` — gera build de produção

### Backend

- `dotnet run` — inicia a API
- `dotnet ef database update` — aplica migrations

---

## Observações

- Ajuste as variáveis de ambiente conforme seu ambiente de desenvolvimento.
- O projeto utiliza autenticação JWT.
- O frontend consome a API do backend via endpoints REST.

---

## Licença

Este projeto está sob a licença MIT.