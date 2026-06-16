// Sirve para definir un punto del acuerdo (cada compromiso que se toma)
export interface IAgreementPoint {
  description: string; // Descripcion del punto
  deadline: string; // Fecha limite para cumplirlo
  complianceStatus: string; // pendiente, cumplido o incumplido
}

// Sirve para definir como viene un acuerdo desde el backend
export interface IAgreement {
  id: string; // ID del acuerdo
  caseId: string; // ID del caso al que pertenece
  mediatorId: string; // ID del mediador que lo redactó
  points: IAgreementPoint[]; // Lista de puntos del acuerdo
  confirmedByReporter: boolean; // true si el que reportó ya confirmó
  confirmedByRespondent: boolean; // true si el denunciado ya confirmó
  formalizedAt: string | null; // Fecha de formalización (null si no está formalizado)
  createdAt: string; // Fecha de creación del acuerdo
}
