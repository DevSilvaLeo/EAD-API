-- THEONEEAD — banco MySQL legado (somente leitura na API)
-- Ajuste o nome do banco se diferente de `estoque`.
-- Usuário da connection string precisa de SELECT nestas tabelas.

-- Vínculo aluno ↔ curso (usado por LegacyDbContext / ILegacyAcademicoReadRepository)
CREATE TABLE IF NOT EXISTS legado_aluno_curso (
  AlunoId BIGINT NOT NULL,
  CursoId BIGINT NOT NULL,
  PRIMARY KEY (AlunoId, CursoId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exemplo: aluno legado 1 matriculado nos cursos 10 e 20 (ajuste aos IDs reais do legado)
-- INSERT INTO legado_aluno_curso (AlunoId, CursoId) VALUES (1, 10), (1, 20);
