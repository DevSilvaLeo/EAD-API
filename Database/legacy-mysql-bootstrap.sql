-- Exemplo de tabela de vínculo aluno-curso para o DbContext legado (Database First).
-- Ajuste nomes/colunas ao schema real e aponte a connection string "Legacy" no appsettings.
-- Mapeamento EF: entidade LegacyAlunoCurso → tabela legado_aluno_curso

CREATE TABLE IF NOT EXISTS legado_aluno_curso (
  AlunoId BIGINT NOT NULL,
  CursoId BIGINT NOT NULL,
  PRIMARY KEY (AlunoId, CursoId)
);

-- Demo: aluno 1 matriculado no curso legado 1 (compatível com seed do SQL Server EAD)
INSERT IGNORE INTO legado_aluno_curso (AlunoId, CursoId) VALUES (1, 1);
