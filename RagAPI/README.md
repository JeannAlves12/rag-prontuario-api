# 🏥 Sistema de Prontuário Inteligente - RagAPI

Uma API construída em .NET que utiliza Inteligência Artificial local (LLMs) para gerar resumos clínicos automáticos a partir do histórico de pacientes.

## 🚀 O que o projeto faz
A **RagAPI** atua como um sistema RAG (Retrieval-Augmented Generation). Ela se comunica com um banco de dados hospitalar para buscar o histórico completo de consultas de um paciente e envia esses dados para um modelo de linguagem natural (Ollama), que retorna um resumo estruturado contendo:
* Diagnóstico Atual
* Sintomas Recentes
* Tratamentos Aplicados
* Resumo Clínico Geral

Este projeto é capaz de resolver conflitos de informações médicas e ignorar dados irrelevantes do histórico pregresso.

## 🛠️ Tecnologias Utilizadas
* **C# / .NET 8** (Backend da API)
* **Ollama** (Servidor de IA Local)
* **Phi-3** (Modelo de Linguagem Natural / LLM)
* **Entity Framework / MySQL** (Comunicação de dados através da HospitalAPI)
* **Vue.js / HTML** (Frontend do Painel Médico)

## ⚙️ Pré-requisitos
Para rodar este projeto na sua máquina, você vai precisar de:
1. [.NET SDK](https://dotnet.microsoft.com/) instalado.
2. [Ollama](https://ollama.com/) instalado e rodando.
3. Modelo Phi-3 baixado no Ollama (rode `ollama run phi3` no terminal).
4. A API do Banco de Dados Hospitalar rodando localmente.

## 📡 Endpoint Principal

`GET /api/Rag/summary/{cpf}`
Retorna o resumo gerado pela IA para o CPF informado.