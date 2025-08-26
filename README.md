# 🗂️ Portfolio: API .NET com Amazon S3 (via LocalStack)

Este projeto foi criado para **demonstrar minhas habilidades em integração com Amazon S3** usando .NET Core e LocalStack. Ele serve como um exemplo prático de como construir uma API que interage com serviços de armazenamento na nuvem, totalmente rodando em **Docker Compose**.

---

## 🚀 Objetivo do Projeto

- Mostrar conhecimentos em **Amazon S3** sem precisar de conta AWS real.  
- Demonstrar **boas práticas em APIs .NET Core** com endpoints RESTful.  
- Automatizar o ambiente de teste com **Docker Compose**.

---

## 🛠️ Tecnologias e Ferramentas

- **.NET Core** – Backend da API  
- **LocalStack** – Simulação local do Amazon S3  
- **Docker / Docker Compose** – Orquestração dos containers  
- **Swagger** – Documentação e teste dos endpoints da API  

---

## ⚡ Funcionalidades da API

- 🆕 **Adicionar arquivos** ao S3 simulado  
- 🔍 **Buscar arquivos** por chave  
- ❌ **Excluir arquivos**  
- 🐳 **Setup completo via Docker Compose** (API + LocalStack)  
- 📂 **Swagger** disponível em `http://localhost:8080/swagger` para testes imediatos  

---

## 🐳 Como rodar

1. Clone o projeto:

git clone <seu-repositorio>
cd <pasta-do-projeto>

2. Suba os containers com Docker Compose:

docker-compose up

3. Acesse a documentação e teste os endpoints no Swagger:

http://localhost:8080/swagger

> Com isso, você já tem **uma API .NET funcional integrada ao S3 local** sem nenhuma configuração manual extra.

---

## 💡 Observações Técnicas

- LocalStack simula o S3 local, permitindo testes offline.  
- Todos os endpoints foram implementados para **cobrir operações básicas de CRUD** de arquivos.  
- Ideal para demonstração em entrevistas ou portfólio de projetos de cloud.  
