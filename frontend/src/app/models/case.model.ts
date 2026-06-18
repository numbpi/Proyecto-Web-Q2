// Sirve para definir como viene un caso desde el backend
export interface ICase {
  id: string;
  reporterId: string; // ID del usuario que reportó
  reporterName: string; // Nombre del que reportó
  respondentId: string; // ID del usuario denunciado
  respondentName: string; // Nombre del denunciado
  conflictType: string; // Tipo de conflicto (Ruido, Mascotas, etc)
  description: string; // Descripción del conflicto
  address: string; // Dirección donde ocurre
  status: string; // Estado: nuevo, asignado, en mediacion, resuelto, cerrado sin acuerdo
  mediatorId: string | null; // ID del mediador asignado (null si no tiene)
  evidenceUrls: string[]; // URLs de las evidencias (fotos, documentos)
  createdAt: string; // Fecha de creación
  assignedAt: string | null; // Fecha de asignación del mediador
  closedAt: string | null; // Fecha de cierre del caso
}

// Sirve para enviar los datos al crear un caso nuevo
export interface ICreateCaseDto {
  respondentId: string; // ID de la persona denunciada
  conflictType: string; // Tipo de conflicto
  description: string; // Descripción del problema
  address: string; // Dirección del conflicto
}

// Sirve para guardar el resultado cuando se busca un usuario por email
export interface IUserSearchResult {
  id: string;
  fullName: string;
  email: string;
  role: string;
}
