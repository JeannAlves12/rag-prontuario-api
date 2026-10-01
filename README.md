# 🏥 Sistema de Prontuário Inteligente — RAG API

🇬🇧 **Summary:** .NET 8 API that fetches a patient's consultation history from a hospital API and sends it to a local LLM (Ollama + Phi-3) to produce a structured clinical summary (RAG pattern). Study project; the front-end was built with AI assistance.

🇧🇷 API em **.NET 8** que busca o histórico de consultas de um paciente e usa uma IA local (**Ollama + Phi-3**) para gerar um resumo clínico estruturado.

## O que faz

Funciona como um sistema **RAG (Retrieval-Augmented Generation)**:

1. Recebe o CPF do paciente
2. Busca o histórico de consultas na API hospitalar (HospitalAPI)
3. Envia esses dados ao modelo de linguagem, que devolve um resumo com:
   - Diagnóstico atual
   - Sintomas recentes
   - Tratamentos aplicados
   - Resumo clínico geral

O prompt orienta o modelo a resolver conflitos de informação e ignorar dados irrelevantes do histórico antigo.

## Arquitetura

```
Painel (HTML/Vue) ──► RagAPI (.NET 8) ──► HospitalAPI (dados)
                          │
                          └──► Ollama + Phi-3 (resumo)
```

> A **HospitalAPI** é uma API externa a este repositório, que fornece os dados dos pacientes. Este projeto contém apenas o RAG e o painel.

## Tecnologias

- C# / .NET 8 (API)
- Ollama + Phi-3 (LLM local)
- Entity Framework / MySQL (dados via HospitalAPI)
- Vue.js / HTML (painel médico, **desenvolvido com apoio de IA**)

## Pré-requisitos

1. [.NET SDK](https://dotnet.microsoft.com/)
2. [Ollama](https://ollama.com/) rodando
3. Modelo baixado: `ollama run phi3`
4. HospitalAPI rodando localmente

## Como rodar

```bash
git clone https://github.com/JeannAlves12/rag-prontuario-api
cd rag-prontuario-api
dotnet run --project RagAPI
```

## Endpoint

`GET /api/Rag/summary/{cpf}` — retorna o resumo gerado pela IA para o CPF informado.

## ⚠️ Aviso

Projeto de estudo. Modelos de linguagem podem errar, e o resumo **não substitui avaliação médica**. Use apenas dados fictícios.

Documentação adicional em [`doc.md`](doc.md).
