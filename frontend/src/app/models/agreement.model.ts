export interface IAgreementPoint {
  description: string;
  deadline: string;
  complianceStatus: string;
}

export interface IAgreement {
  id: string;
  caseId: string;
  mediatorId: string;
  points: IAgreementPoint[];
  confirmedByReporter: boolean;
  confirmedByRespondent: boolean;
  formalizedAt: string | null;
  createdAt: string;
}

export interface ICreateAgreementPointDto {
  description: string;
  deadline: string;
}

export interface ICreateAgreementDto {
  caseId: string;
  agreementText: string;
  points: ICreateAgreementPointDto[];
}