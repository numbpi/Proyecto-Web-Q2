export interface ILoginResponse {
  token: string;
}

export interface IRegisterResponse {
  id: string;
  fullName: string;
  password: string;
  email: string;
  role: string;
}

export interface IForgotPasswordRequest {
  email: string;
}

export interface IResetPasswordRequest {
  token: string;
  newPassword: string;
}

export interface ICurrentUser {
  id: string;
  fullName: string;
  email: string;
  role: string;
}
