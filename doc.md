# Documentação de Integração - SIAH API para RAG

Esta documentação descreve as principais rotas da **SIAH API** que podem ser consumidas pelo serviço de RAG (Recuperação Aumentada por Geração) para a construção de resumos clínicos, análise de histórico e obtenção de dados do paciente.

Todas as rotas abaixo foram adaptadas para receber o **CPF** do paciente como identificador (via query string ou rota), dispensando a necessidade de conhecer o `userId` (UUID) interno do banco de dados.

---

## 1. Consultas e Histórico Clínico

As rotas de agendamentos e histórico são fundamentais para o RAG gerar resumos das passagens do paciente pelo hospital.

### 1.1 Listar Consultas do Paciente (Criada especificamente para o RAG)
Retorna os dados das consultas formatados, incluindo os medicamentos parseados do campo prescrição.

- **Método:** `GET`
- **Rota:** `/api/Consultations/patient/{cpf}`
- **Parâmetros:** `cpf` (na rota)
- **Retorno Esperado (200 OK):**
```json
[
  {
    "id": "uuid-da-consulta",
    "reason": "Dor de cabeça",
    "finalDiagnosis": "Enxaqueca",
    "date": "2026-05-20T10:30:00",
    "observations": "Paciente relata episódios frequentes",
    "medications": [
      { "name": "Dipirona 500mg" },
      { "name": "Ibuprofeno 400mg" }
    ]
  }
]
```

### 1.2 Obter Histórico Completo de Atendimentos
Lista todo o histórico de consultas/atendimentos do paciente, com paginação opcional.

- **Método:** `GET`
- **Rota:** `/history/appointments?cpf={cpf}`
- **Parâmetros:** `cpf` (query string)
- **Retorno Esperado (200 OK):**
```json
[
  {
    "id": "uuid",
    "data": "2026-05-20T00:00:00",
    "horario": "10:30",
    "status": "completed",
    "medico": {
      "id": "uuid-medico",
      "nome": "Dr. João",
      "fotoUrl": "url-da-foto"
    },
    "especialidade": "Clínica Geral"
  }
]
```

### 1.3 Histórico Recente
Retorna os atendimentos mais recentes (limitado, ideal para resumos rápidos).

- **Método:** `GET`
- **Rota:** `/history/recent?cpf={cpf}`
- **Parâmetros:** `cpf` (query string)

---

## 2. Documentos Clínicos (Exames, Receitas e Atestados)

O RAG pode cruzar os dados das consultas com os documentos gerados para enriquecer o resumo clínico.

### 2.1 Listar Receitas Médicas
- **Método:** `GET`
- **Rota:** `/documents/prescriptions?cpf={cpf}`
- **Apenas Receitas Ativas:** `/documents/prescriptions/active?cpf={cpf}`
- **Retorno Esperado (200 OK):**
```json
[
  {
    "id": "uuid",
    "tipo": "prescription",
    "titulo": "Receita - Dipirona",
    "conteudo": "Tomar de 8 em 8 horas",
    "ativo": true,
    "criadoEm": "2026-05-20T10:30:00"
  }
]
```

### 2.2 Listar Exames
- **Método:** `GET`
- **Rota:** `/documents/exams?cpf={cpf}`

### 2.3 Listar Atestados
- **Método:** `GET`
- **Rota:** `/documents/certificates?cpf={cpf}`

---

## 3. Dados do Paciente (Perfil)

Para obter os dados básicos e de contato do paciente.

### 3.1 Obter Perfil Completo
- **Método:** `GET`
- **Rota:** `/profile?cpf={cpf}`
- **Parâmetros:** `cpf` (query string)
- **Retorno Esperado (200 OK):**
```json
{
  "id": "uuid",
  "nome": "João da Silva",
  "cpf": "12345678900",
  "email": "joao@email.com",
  "dataNascimento": "1990-01-01",
  "genero": "Masculino",
  "tipoSanguineo": "O+",
  "hospitalVinculado": "Hospital Central",
  "planoSaude": {
    "possui": true,
    "nome": "Plano Vida"
  }
}
```

---

## 4. Agendamentos Futuros

Caso o RAG precise informar ao paciente sobre próximas consultas.

### 4.1 Listar Próximos Agendamentos
- **Método:** `GET`
- **Rota:** `/appointments/upcoming?cpf={cpf}`
- **Parâmetros:** `cpf` (query string)

---

## Observações para o RAG

1. **Autenticação:** As rotas acima não dependem de `UserId` extraído de JWT. O próprio `cpf` enviado resolve a identidade do paciente no banco de dados.
2. **Tratamento de Erros:** Se um CPF não for encontrado na base de dados, as rotas retornarão um status `404 Not Found`.
3. **Casos Sensíveis (Case Insensitive):** Ao desserializar as respostas JSON no C#, utilize `PropertyNameCaseInsensitive = true` para garantir que o mapeamento ocorra corretamente.
