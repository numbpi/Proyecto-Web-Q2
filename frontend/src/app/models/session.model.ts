export interface IMediationSession {
  id: string;
  caseId: string;
  mediatorId: string;
  scheduledDate: string;
  scheduledTime: string;
  modality: string;
  meetingLink: string | null;
  status: string;
  sessionNotes: string | null;
  createdAt: string;
}

export interface ICreateSessionDto {
  caseId: string;
  scheduledDate: string;
  scheduledTime: string;
  modality: string;
  meetingLink: string;
  sessionNotes: string;
}